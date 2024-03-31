using Scripts.Enums;
using Scripts.Projectile;
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
            
            _floorMask = LayerMask.GetMask("Floor");
        }

        private void OnLevelEnd()
        {
            _inGame = false;
            _primed = false;
        }

        private void OnLevelStart()
        {
            _inGame = true;
            _primed = false;
        }

        private void OnTapSignal(OnTapSignal onTapSignal)
        {
            if(!_inGame)
                return;
            
            if(!_primed)
                return;
            
            _primed = false;

            switch (_selectedProjectileType)
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
            
            RaycastHit hit;
            _tapPoint.x = onTapSignal.InputDataParams.width;
            _tapPoint.y = onTapSignal.InputDataParams.height;
            Ray ray = Camera.main.ScreenPointToRay(_tapPoint);
            if (Physics.Raycast(ray, out hit,1000f,_floorMask))
            {
                ProjectileBase projectileBase = Instantiate(_cache, Vector3.up*20, Quaternion.identity);
                projectileBase.Move(hit.point);
            }
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