using System.Collections.Generic;
using Scripts.Models;
using Scripts.Signals;
using Scripts.Turret;
using Scripts.Keys;
using UnityEngine;
using Zenject;

namespace Scripts.Manager
{
    public class TurretManager : MonoBehaviour
    {
        [SerializeField] private int maxTowerCount = 3;
        
        private SignalBus _signalBus;
        public List<TurretTower> turretTowers;
        public TurretTower currentSelected;
                
        private Vector3 _cacheCameraRay;
        private Vector3 _cacheCameraRayResults;
        private LayerMask _floorMask;
        private bool _inMenu;
        
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
            _floorMask = LayerMask.GetMask("Floor");
            Reset();
        }
        public void Reset()
        {
            for (int i = 0; i < GameModel.TowerNumber; i++)
            {
                turretTowers[i].gameObject.SetActive(true);
                turretTowers[i].id = i;
                turretTowers[i].SetUpgrade(GameModel.TowerUpgrade(i));
                turretTowers[i].DeSelect();
                turretTowers[i].health = 1000f;
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
            GameModel.TowerNumber++;
            Reset();
            
            _signalBus.Fire<OnStartUIUpdate>();
        }

        public void OnTapSignal(OnTapSignal onTapSignal)
        {
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
                if (Vector3.Distance(_cacheCameraRayResults, turretTower.transform.position)<4f)
                {
                    currentSelected?.DeSelect();
                    turretTower.Select();
                    currentSelected = turretTower;
                    break;
                }
            }
        }

        public void OnTowerUpgrade()
        {
            currentSelected.SetUpgrade(currentSelected.upgradeLevel+1);
            GameModel.TowerUpgrade(currentSelected.id, currentSelected.upgradeLevel);
        }
        
    }
}