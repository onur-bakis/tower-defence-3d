using Scripts.Enums;
using Scripts.Manager;
using Scripts.Models;
using Scripts.Signals;
using Scripts.Soldier;
using Scripts.Managers;
using UnityEngine;
using Zenject;

namespace Scripts.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private TurretManager _turretManager;
        [SerializeField] private InputManager _inputManager;
        
        [SerializeField] private SoldierMelee _soldierMelee;
        [SerializeField] private SoldierRanged _soldierRanged;
        [SerializeField] private SoldierBomber _soldierBomber;
        public override void InstallBindings()
        {
            InstallManagers();
            InstallSignals();
            InstallModels();
            InstallFactories();
        }

        private void InstallFactories()
        {
            Container.BindFactory<int, UnitTeams, SoldierTypes, SoldierMelee, SoldierMelee.Factory>()
                .FromPoolableMemoryPool<int, UnitTeams, SoldierTypes, SoldierMelee, SoldierMeleePool>(poolBinder => poolBinder
                    .WithInitialSize(50)
                    .FromComponentInNewPrefab(_soldierMelee)
                    .UnderTransformGroup("SoldierMelee"));
            Container.BindFactory<int, UnitTeams, SoldierTypes, SoldierRanged, SoldierRanged.Factory>()
                .FromPoolableMemoryPool<int, UnitTeams, SoldierTypes, SoldierRanged, SoldierRangedPool>(poolBinder => poolBinder
                    .WithInitialSize(50)
                    .FromComponentInNewPrefab(_soldierRanged)
                    .UnderTransformGroup("SoldierRanged"));
            Container.BindFactory<int, UnitTeams, SoldierTypes, SoldierBomber, SoldierBomber.Factory>()
                .FromPoolableMemoryPool<int, UnitTeams, SoldierTypes, SoldierBomber, SoldierBomberPool>(poolBinder => poolBinder
                    .WithInitialSize(50)
                    .FromComponentInNewPrefab(_soldierBomber)
                    .UnderTransformGroup("SoldierBomber"));
        }

        private void InstallModels()
        {
            Container.Bind<GameModel>().AsSingle();
        }

        private void InstallManagers()
        {
            Container.Bind<UIManager>().FromInstance(_uiManager).AsSingle();
            Container.Bind<TurretManager>().FromInstance(_turretManager).AsSingle();
            // Container.Bind<InputManager>().FromInstance(_inputManager).AsSingle();
        }

        private void InstallSignals()
        {
            SignalBusInstaller.Install(Container);
            
            Container.DeclareSignal<OnTowerUpgrade>();
            Container.DeclareSignal<OnTowerUnlock>();
            Container.DeclareSignal<OnFireBallClick>();
            Container.DeclareSignal<OnIceBallClick>();
            Container.DeclareSignal<OnSoldierMeleeClick>();
            Container.DeclareSignal<OnSoldierRangedClick>();
            Container.DeclareSignal<OnSoldierBomberClick>();
            Container.DeclareSignal<OnReloadClick>();
            
            Container.DeclareSignal<OnLevelStart>();
            Container.DeclareSignal<OnLevelEnd>();
            
            Container.DeclareSignal<OnProgressBarUpdated>();
            Container.DeclareSignal<OnCoinUpdated>();
            
            Container.DeclareSignal<OnTapSignal>();
            Container.DeclareSignal<OnEnemyKilled>();
            Container.DeclareSignal<OnTowerDestroyed>();
            Container.DeclareSignal<OnStartUIUpdate>();
        }

        class SoldierMeleePool : MonoPoolableMemoryPool<int, UnitTeams, SoldierTypes, IMemoryPool, SoldierMelee>
        {
            
        }
        class SoldierRangedPool : MonoPoolableMemoryPool<int, UnitTeams, SoldierTypes, IMemoryPool, SoldierRanged>
        {
            
        }        
        class SoldierBomberPool : MonoPoolableMemoryPool<int, UnitTeams, SoldierTypes, IMemoryPool, SoldierBomber>
        {
            
        }
    }

}
