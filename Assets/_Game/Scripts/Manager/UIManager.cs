using Scripts.Signals;
using UnityEngine;
using Zenject;

namespace Scripts.Manager
{
    public class UIManager : MonoBehaviour
    {
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        public void OnPlay()
        {
            _signalBus.Fire<OnLevelStart>();
        }

        public void OnTowerUpgrade()
        {
            _signalBus.Fire<OnTowerUpgrade>();
        }

        public void OnFireBallClick()
        {
            _signalBus.Fire<OnFireBallClick>();
        }

        public void OnIceBallClick()
        {
            _signalBus.Fire<OnIceBallClick>();
        }

        public void OnSoldierMeleeClick()
        {
            _signalBus.Fire<OnSoldierMeleeClick>();
        }

        public void OnSoldierRangedClick()
        {
            _signalBus.Fire<OnSoldierRangedClick>();
        }

        public void OnSoldierBomberClick()
        {
            _signalBus.Fire<OnSoldierBomberClick>();
        }

        public void OnReloadClick()
        {
            _signalBus.Fire<OnReloadClick>();
        }

        public void OnTowerUnlock()
        {
            _signalBus.Fire<OnTowerUnlock>();
        }
    }
}