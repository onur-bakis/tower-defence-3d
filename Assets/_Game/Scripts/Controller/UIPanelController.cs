using Scripts.Signals;
using TMPro;
using UnityEngine;
using Zenject;

namespace Scripts.Controller
{
    public class UIPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject startUI;
        [SerializeField] private GameObject levelUI;
        [SerializeField] private GameObject endUI;
        [SerializeField] private TextMeshProUGUI winLoseText;

        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            
            _signalBus.Subscribe<OnLevelStart>(OnLevelStart);
            _signalBus.Subscribe<OnLevelEnd>(OnLevelEnd);
            _signalBus.Subscribe<OnReloadClick>(OnReloadClick);
        }

        private void OnReloadClick()
        {
            endUI.SetActive(false);
            startUI.SetActive(true);
        }

        private void OnLevelEnd(OnLevelEnd onLevelEnd)
        {
            levelUI.SetActive(false);
            endUI.SetActive(true);
            winLoseText.text = onLevelEnd.Win ? "WIN" : "LOSE";
        }

        private void OnLevelStart()
        {
            startUI.SetActive(false);
            levelUI.SetActive(true);
        }
    }
}