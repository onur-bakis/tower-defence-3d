using System.Collections.Generic;
using Scripts.Controller.Turret;
using Scripts.Keys;
using Scripts.Models;
using Scripts.Signals;
using UnityEngine;
using Zenject;

namespace Scripts.Manager
{
    public class TurretManager : MonoBehaviour
    {
        private SignalBus _signalBus;
        public List<TurretTower> turretTowers;
        public TurretTower currentSelected;
                
        private Vector3 _cacheCameraRay;
        private Vector3 _cacheCameraRayResults;
        private LayerMask _floorMask;
        private bool _inMenu;

        private OnCoinUpdated _onCoinUpdated;
        
        [Inject] public GameModel GameModel;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<OnTapSignal>(OnTapSignal);
            _signalBus.Subscribe<OnTowerUpgrade>(OnTowerUpgrade);
            _signalBus.Subscribe<OnReloadClick>(OnReloadClick);
            _signalBus.Subscribe<OnLevelStart>(OnLevelStart);
            _signalBus.Subscribe<OnTowerUnlock>(OnTowerUnlock);

            _inMenu = true;
            _cacheCameraRay = Vector3.zero;
            _onCoinUpdated = new OnCoinUpdated();
            _floorMask = LayerMask.GetMask("TowerSelection");
            Reset();
        }
        public void Reset()
        {
            for (int i = 0; i < turretTowers.Count; i++)
            {
                turretTowers[i].gameObject.SetActive(i<GameModel.TowerNumber);
                turretTowers[i].id = i;
                turretTowers[i].SetValues(GameModel.TowerUpgrade(i));
                turretTowers[i].DeSelect();
            }
        }
        private void OnLevelStart()
        {
            _inMenu = false;
            Reset();
        }
        private void OnReloadClick()
        {
            _inMenu = true;
            Reset();
        }
        private void OnTowerUnlock()
        {
            if(GameModel.CoinNumber < 100)
                return;

            GameModel.CoinNumber -= 100;
            GameModel.TowerNumber++;
            Reset();
            
            _onCoinUpdated.amount = GameModel.CoinNumber;
            _signalBus.Fire(_onCoinUpdated);
            _signalBus.Fire<OnStartUIUpdate>();
        }
        public void OnTowerUpgrade()
        {
            if(GameModel.CoinNumber < 50)
                return;
            
            if(currentSelected==null)
                return;
            
            GameModel.CoinNumber -= 50;
            _onCoinUpdated.amount = GameModel.CoinNumber;
            _signalBus.Fire(_onCoinUpdated);
            currentSelected.SetValues(currentSelected.upgradeLevel+1);
            GameModel.TowerUpgrade(currentSelected.id, currentSelected.upgradeLevel);
        }
        public void OnTapSignal(OnTapSignal onTapSignal)
        {
            if (!_inMenu)
                return;
            
            InputDataParams inputDataParams = onTapSignal.InputDataParams;
            _cacheCameraRay.x = inputDataParams.width;
            _cacheCameraRay.y = inputDataParams.height;
            _cacheCameraRayResults = Camera.main.ScreenToWorldPoint(_cacheCameraRay);
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(_cacheCameraRay);
            if (Physics.Raycast(ray, out hit,1000f,_floorMask))
            {
                _cacheCameraRayResults =hit.point;
            }
            else
            {
                return;
            }
            
            foreach (var turretTower in turretTowers)
            {
                if (Vector3.Distance(_cacheCameraRayResults, turretTower.turretHead.transform.position)<2f)
                {
                    currentSelected?.DeSelect();
                    turretTower.Select();
                    currentSelected = turretTower;
                    break;
                }
            }
        }

        
    }
}