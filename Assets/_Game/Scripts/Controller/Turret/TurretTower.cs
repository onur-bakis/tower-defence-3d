using Scripts.Controller.Unit;
using Scripts.Enums;
using Scripts.Keys;
using Scripts.Manager;
using Scripts.Signals;
using UnityEngine;
using Zenject;

namespace Scripts.Controller.Turret
{
    public class TurretTower : UnitActionBase
    {
        public TurretHead turretHead;
        
        public int id;
        public int upgradeLevel;
        
        private SignalBus _signalBus;
        [Inject] private TurretManager TurretManager;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<OnLevelStart>(OnLevelStart);
        }

        private void OnLevelStart()
        {
            active = true;
        }
        
        public override void TakeAction(UnitActionBase enemy)
        {
            base.TakeAction(enemy);
            turretHead.Attack(enemy);
        }

        public override void Defeat()
        {
            base.Defeat();
            _signalBus.Fire<OnTowerDestroyed>();
            gameObject.SetActive(false);
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
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("UnitActionBase"))
                return;

            UnitActionBase unitActionBaseOther = other.gameObject.GetComponent<UnitActionBase>();
            if (unitActionBaseOther.unitTeams == unitTeams)
                return;
            
            TakeAction(unitActionBaseOther);
        }
        
        public void SetValues(int level)
        {
            bool upgrade = turretHead.upgradeLevel != 0 && level > turretHead.upgradeLevel;

            health = upgradeLevel * 100f;
            upgradeLevel = level;
            turretHead.SetUpgrade(level,upgrade);
        }
        
        public void Select()
        {
            turretHead.Select();
        }

        public void DeSelect()
        {
            turretHead.DeSelect();
        }
    }
}