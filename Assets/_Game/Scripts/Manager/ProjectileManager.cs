using System.Collections.Generic;
using Scripts.Controller.Projectile;
using Scripts.Enums;
using Scripts.Signals;
using UnityEngine;
using Zenject;

namespace Scripts.Manager
{
    public class ProjectileManager : MonoBehaviour
    {
        [SerializeField] private ProjectileBase _fireBall;
        [SerializeField] private ProjectileBase _iceBall;
        [SerializeField] private ProjectileBase _towerBall;
        
        private SignalBus _signalBus;
        private bool _inGame;
        private ProjectileType _selectedProjectileType;
        private bool _primed;
        private Vector2 _tapPoint;
        private LayerMask _floorMask;

        private ProjectileBase _cache;

        private List<ProjectileBase> _projectileBases;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            
            _signalBus.Subscribe<OnFireBallClick>(OnFireBallClick);
            _signalBus.Subscribe<OnIceBallClick>(OnIceBallClick);
            _signalBus.Subscribe<OnTapSignal>(OnTapSignal);
            _signalBus.Subscribe<OnLevelStart>(OnLevelStart);
            _signalBus.Subscribe<OnLevelEnd>(OnLevelEnd);
            
            _tapPoint = Vector2.zero;

            _projectileBases = new List<ProjectileBase>();
            
            _floorMask = LayerMask.GetMask("Floor");
        }

        private void OnLevelEnd()
        {
            _inGame = false;
            _primed = false;
            foreach (var projectileBase in _projectileBases)
            {
                if(projectileBase != null)
                    Destroy(projectileBase.gameObject);
            }
        }

        private void OnLevelStart()
        {
            _inGame = true;
            _primed = false;
            _projectileBases.Clear();
        }

        private void OnTapSignal(OnTapSignal onTapSignal)
        {
            if(!_inGame)
                return;
            
            if(!_primed)
                return;
            
            _primed = false;

            
            
            RaycastHit hit;
            _tapPoint.x = onTapSignal.InputDataParams.width;
            _tapPoint.y = onTapSignal.InputDataParams.height;
            Ray ray = Camera.main.ScreenPointToRay(_tapPoint);
            if (Physics.Raycast(ray, out hit,1000f,_floorMask))
            {
                SendProjectile(_selectedProjectileType,Vector3.back*20,hit.point);
                
            }
        }

        public void SendProjectile(ProjectileType projectileType,Vector3 startPosition,Vector3 movePosition)
        {
            switch (projectileType)
            {
                case ProjectileType.Fire:
                    _cache = _fireBall;
                    break;
                case ProjectileType.Ice:
                    _cache = _iceBall;
                    break;
                case ProjectileType.Tower:
                    _cache = _towerBall;
                    break;
                default:
                    _cache = _fireBall;
                    break;
            }
            ProjectileBase projectileBase = Instantiate(_cache, startPosition, Quaternion.identity);
            projectileBase.Move(movePosition);
            _projectileBases.Add(projectileBase);
        }

        private void OnIceBallClick()
        {
            _selectedProjectileType = ProjectileType.Ice;
            _primed = true;
        }

        private void OnFireBallClick()
        {
            _selectedProjectileType = ProjectileType.Fire;
            _primed = true;
        }
    }
}