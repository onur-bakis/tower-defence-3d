using Scripts.Enums;
using Scripts.Keys;
using Scripts.Manager;
using Scripts.Signals;
using Scripts.Unit;
using UnityEngine;
using Zenject;

namespace Scripts.Turret
{
    public class TurretTower : UnitActionBase
    {
        public TurretHead turretHead;
        [SerializeField] private Material headMaterial;
        [SerializeField] private Material selecredMaterial;
        [SerializeField] private MeshRenderer _meshRenderer;
        
        public int id;
        public int upgradeLevel;
        
        private SignalBus _signalBus;
        [Inject] private TurretManager TurretManager;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            TurretManager.turretTowers.Add(this);
            _signalBus.Subscribe<OnLevelStart>(OnLevelStart);
        }

        private void OnLevelStart()
        {
            active = true;
        }

        public void SetUpgrade(int level)
        {
            upgradeLevel = level;
            turretHead.upgradeLevel = level;
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

        public void Select()
        {
            _meshRenderer.materials = new Material[]{selecredMaterial};
        }

        public void DeSelect()
        {
            _meshRenderer.materials =  new Material[]{headMaterial};

        }
    }
}