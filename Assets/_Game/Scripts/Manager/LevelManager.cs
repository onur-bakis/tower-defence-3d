using Scripts.Data;
using Scripts.Data.UnityObject;
using Scripts.Enums;
using Scripts.Models;
using Scripts.Signals;
using Scripts.Soldier;
using UnityEngine;
using Zenject;

namespace Scripts
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private GameData _gameData;

        [Inject] private GameModel GameModel;

        [Inject] private SoldierMelee.Factory _soldierMeleeFactory;
        [Inject] private SoldierRanged.Factory _soldierRangedFactory;
        [Inject] private SoldierBomber.Factory _soldierBomberFactory;
        
        private SignalBus _signalBus;

        private int _currentLevelNumber;
        private LevelData _currentLevelData;
        private int _currentWaveCount;
        private int _currentEnemyDeployed;
        private int _currentEnemyKilled;
        private int _enemyKilledPreviousWaves;
        private WaveData _currentWaveData;
        private bool onPlay;
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            
            _signalBus.Subscribe<OnLevelStart>(OnLevelStart);
            _signalBus.Subscribe<OnLevelEnd>(OnLevelEnd);
            _signalBus.Subscribe<OnEnemyKilled>(OnEnemyKilled);
        }

        private void OnLevelEnd()
        {
            onPlay = false;
        }


        private void OnLevelStart()
        {
            _currentLevelNumber = GameModel.LevelNumber;
            _currentLevelData = _gameData.LevelData[_currentLevelNumber%_gameData.LevelData.Length];
            
            _currentWaveCount = 0;
            _currentEnemyKilled = 0;
            _currentEnemyDeployed = 0;
            _enemyKilledPreviousWaves = 0;
            onPlay = true;
            DeployWave();
        }
        
        

        public void DeployWave()
        {
            if(!onPlay)
                return;
            
            _currentWaveData = _currentLevelData.waveData[_currentWaveCount];
            int delay = 0;
            for (int i = 0; i < _currentWaveData.WavePartData.Length; i++)
            {
                SoldierTypes soldierTypes = _currentWaveData.WavePartData[i].soldierType;
                for (int j = 0; j < _currentWaveData.WavePartData[i].soldierCount; j++)
                {
                    delay++;
                    _currentEnemyDeployed++;
                    DeploySoldiers(soldierTypes,delay);
                }
            }

            _currentWaveCount++;
            _enemyKilledPreviousWaves = _currentEnemyKilled;
            UpdateSlider();
        }
        
        private void OnEnemyKilled()
        {
            if(!onPlay)
                return;
            
            _currentEnemyKilled++;

            UpdateSlider();

            if (_currentEnemyKilled >= _currentEnemyDeployed)
            {
                if (_currentWaveCount == _currentLevelData.waveData.Length)
                {
                    _signalBus.Fire(new OnLevelEnd(){Win = true});
                }
                else
                {
                    DeployWave();
                }
            }
        }

        private void UpdateSlider()
        {
            float slider = (float)(_currentEnemyKilled - _enemyKilledPreviousWaves) /
                           (_currentEnemyDeployed - _enemyKilledPreviousWaves);
            slider = 1f - slider;
            
            _signalBus.Fire(new OnProgressBarUpdated(){value = slider,waveNumber = _currentWaveCount});
        }

        private void DeploySoldiers(SoldierTypes soldierTypes,float delay)
        {
            if(!onPlay)
                return;
            
            switch (soldierTypes)
            {
                case SoldierTypes.SoldierMelee:
                    SoldierMelee soldierMelee = _soldierMeleeFactory.Create(1, UnitTeams.TeamAttack, SoldierTypes.SoldierMelee);
                    soldierMelee.transform.position = 
                        new Vector3(0, 1.5f, 8f)+Vector3.forward*4f*delay+Vector3.right*UnityEngine.Random.Range(0f,2f);
                    break;
                case SoldierTypes.SoldierRanged:
                    SoldierRanged soldierRanged = _soldierRangedFactory.Create(1, UnitTeams.TeamAttack, SoldierTypes.SoldierRanged);
                    soldierRanged.transform.position = 
                        new Vector3(0, 1.5f, 8f)+Vector3.forward*4f*delay+Vector3.right*UnityEngine.Random.Range(0f,2f);
                    break;
                case SoldierTypes.SoldierBomber:
                    SoldierBomber soldierBomber = _soldierBomberFactory.Create(1, UnitTeams.TeamAttack, SoldierTypes.SoldierBomber);
                    soldierBomber.transform.position = 
                        new Vector3(0, 1.5f, 8f)+Vector3.forward*4f*delay+Vector3.right*UnityEngine.Random.Range(0f,2f);
                    break;
                default:
                    break;
            }
        }
    }
}