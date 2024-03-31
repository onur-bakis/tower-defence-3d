using Scripts.Enums;
using Scripts.Keys;
using Scripts.Signals;
using Scripts.Unit;
using UnityEngine;
using Zenject;

namespace Scripts.Soldier
{
    public class SoldierBase : UnitActionBase
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Material teamDefendMaterial;
        [SerializeField] private Material teamAttackMaterial;
        
        public float speed = 1f;
        public float slowSpeed = 1f;
        public bool inAction;
        public SoldierTypes soldierType;
        private UnitActionBase _enemy;

        public ActionTypes actionType;
        public float actionImpact;
        public float actionRadius;

        private ActionParams _actionParams;

        private SignalBus _signalBus;
        private Vector3 _cacheDirection;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<OnReloadClick>(OnReloadClick);
        }

        private void OnReloadClick()
        {
            if(active)
                Defeat();
        }

        public override void ActionReceived(ActionParams actionParams)
        {
            base.ActionReceived(actionParams);
            if (actionParams.ActionTypes == ActionTypes.Attack)
            {
                health -= actionParams.Impact;
                if (health < 0 && active)
                {
                    Defeat();
                }
            }
            else if (actionParams.ActionTypes == ActionTypes.Damage)
            {
                health -= actionParams.Impact*Time.deltaTime;
                if (health < 0 && active)
                {
                    Defeat();
                }
            }
            else if (actionParams.ActionTypes == ActionTypes.Slow)
            {
                slowSpeed = 0.5f;
                CancelInvoke();
                Invoke(nameof(BreakSlow),5f);
                if (health < 0 && active)
                {
                    Defeat();
                }
            }
        }

        public void BreakSlow()
        {
            slowSpeed = 1f;
        }

        public void Update()
        {
            if (!inAction)
            {
                BaseAction();
                return;
            }

            if (_enemy == null)
            {
                inAction = false;
                return;
            }
            
            if (Vector3.Distance(_enemy.transform.position, transform.position) < (range+size+_enemy.size))
            {
                Attack(_enemy);
            }
            else
            {
                MovePosition(_enemy);
            }
        }

        public override void Defeat()
        {
            base.Defeat();
            if (unitTeams != UnitTeams.TeamDefend)
            {
                _signalBus.Fire<OnEnemyKilled>();
            }
        }

        private void BaseAction()
        {
            if (unitTeams == UnitTeams.TeamAttack)
            {
                transform.position -= Vector3.forward * (Time.deltaTime * speed);
            }
            else
            {
                transform.position += Vector3.forward * (Time.deltaTime * speed);
            }
        }
        public override void TakeAction(UnitActionBase enemy)
        {
            base.TakeAction(enemy);
            _enemy = enemy;
            inAction = true;
        }

        public virtual void Attack(UnitActionBase enemy)
        {
        }
        
        private void MovePosition(UnitActionBase enemy)
        {
            _cacheDirection = (enemy.transform.position - transform.position).normalized;
            _cacheDirection.y = 0;
            transform.position += _cacheDirection * (slowSpeed*Time.deltaTime * speed);
        }

        public void Reset()
        {
            slowSpeed = 1f;
            ChangeVisual();
        }

        public void ChangeVisual()
        {
            
            if (unitTeams == UnitTeams.TeamDefend)
            {
                _meshRenderer.materials = new Material[] { teamDefendMaterial };
            }
            else
            {
                _meshRenderer.materials = new Material[] { teamAttackMaterial };
            }
        }

        
    }
}