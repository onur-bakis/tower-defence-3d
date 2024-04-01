using System;
using Scripts.Enums;
using Scripts.Manager;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Handler
{
    public class UIEventSubscriber : MonoBehaviour
    {
        
        [SerializeField] private UIEventSubscriptionTypes type;
        [SerializeField] private Button button;

        [Inject] public UIManager UIManager { get; set; }
        

        public void ButtonClick()
        {
            switch (type)
            {
                case UIEventSubscriptionTypes.OnPlay:
                {
                    UIManager.OnPlay();
                    break;
                }
                case UIEventSubscriptionTypes.OnTowerUpgrade:
                {
                    UIManager.OnTowerUpgrade();
                    break;
                }
                case UIEventSubscriptionTypes.OnFireBallClick:
                {
                    UIManager.OnFireBallClick();
                    break;
                }
                case UIEventSubscriptionTypes.OnIceBallClick:
                {
                    UIManager.OnIceBallClick();
                    break;
                }
                case UIEventSubscriptionTypes.OnSoldierMeleeClick:
                {
                    UIManager.OnSoldierMeleeClick();
                    break;
                }
                case UIEventSubscriptionTypes.OnSoldierRangedClick:
                {
                    UIManager.OnSoldierRangedClick();
                    break;
                }
                case UIEventSubscriptionTypes.OnSoldierBomberClick:
                {
                    UIManager.OnSoldierBomberClick();
                    break;
                }
                case UIEventSubscriptionTypes.OnReloadClick:
                {
                    UIManager.OnReloadClick();
                    break;
                }
                case UIEventSubscriptionTypes.OnTowerUnlock:
                {
                    UIManager.OnTowerUnlock();
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void OnEnable()
        {
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            button.onClick.AddListener(ButtonClick);
            
        }

        private void UnSubscribeEvents()
        {
            button.onClick.RemoveListener(ButtonClick);
        }

        public void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}