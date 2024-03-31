using Scripts.Controller;
using Scripts.Models;
using Scripts.Signals;
using Scripts.Soldier;
using UnityEngine;
using Zenject;

namespace Scripts.Manager
{
    public class GamePlayManager : MonoBehaviour
    {
        public SignalBus _signalBus;
        [Inject] private GameModel GameModel;
        [Inject] private SoldierMelee.Factory _soldierMeleeFactory;
        [Inject] private SoldierRanged.Factory _soldierRangedFactory;
        [Inject] private SoldierBomber.Factory _soldierBomberFactory;
        
        private SoldierSpawnController _soldierSpawnController;
        private int _currentDestroyedTowers;
        private bool inPlay;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;

            _soldierSpawnController = new SoldierSpawnController(_soldierMeleeFactory,_soldierRangedFactory,_soldierBomberFactory);
            
            _signalBus.Subscribe<OnTowerDestroyed>(OnTowerDestroyed);
            _signalBus.Subscribe<OnEnemyKilled>(OnEnemyKilled);
            _signalBus.Subscribe<OnLevelStart>(OnLevelStart);
            _signalBus.Subscribe<OnLevelEnd>(OnLevelEnd);
            _signalBus.Subscribe<OnReloadClick>(OnReloadClick);
            _signalBus.Subscribe<OnSoldierMeleeClick>(OnSoldierMeleeClick);
            _signalBus.Subscribe<OnSoldierRangedClick>(OnSoldierRangedClick);
            _signalBus.Subscribe<OnSoldierBomberClick>(OnSoldierBomberClick);
        }

        private void OnReloadClick()
        {
            
        }

        private void OnSoldierMeleeClick()
        {
            _soldierSpawnController.OnSoldierMeleeClick();
        }
        private void OnSoldierRangedClick()
        {
            _soldierSpawnController.OnSoldierRangedClick();
        }
        private void OnSoldierBomberClick()
        {
            _soldierSpawnController.OnSoldierBomberClick();
        }
        private void OnLevelEnd(OnLevelEnd onLevelEnd)
        {
            if(!inPlay)
                return;
            
            inPlay = false;
            
            if (onLevelEnd.Win)
            {
                GameModel.LevelNumber++;
            }
        }

        private void OnLevelStart()
        {
            inPlay = true;
        }

        private void OnEnemyKilled()
        {
            if(!inPlay)
                return;
            
            GameModel.CoinNumber += 10;
            _signalBus.Fire(new OnCoinUpdated(){amount = GameModel.CoinNumber});
        }

        private void OnTowerDestroyed()
        {
            _currentDestroyedTowers++;
            if (_currentDestroyedTowers >= GameModel.TowerNumber)
            {
                _signalBus.Fire(new OnLevelEnd(){Win = false});
            }
        }
    }
}