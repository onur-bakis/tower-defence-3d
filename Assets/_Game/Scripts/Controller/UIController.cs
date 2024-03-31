using Scripts.Models;
using Scripts.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Controller
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinTMP;
        [SerializeField] private TextMeshProUGUI _waveTMP;
        [SerializeField] private Slider _slider;
        [SerializeField] private GameObject _unlockButton;

        private SignalBus _signalBus;
        [Inject] private GameModel GameModel;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            
            _coinTMP.text = GameModel.CoinNumber.ToString();
            
            _signalBus.Subscribe<OnProgressBarUpdated>(OnProgressBarUpdated);
            _signalBus.Subscribe<OnCoinUpdated>(OnCoinUpdated);
            _signalBus.Subscribe<OnLevelStart>(OnLevelStart);
            _signalBus.Subscribe<OnStartUIUpdate>(OnStartUIUpdate);
            
            if (GameModel.TowerNumber == 3)
            {
                _unlockButton.SetActive(false);
            }
        }

        private void OnStartUIUpdate()
        {
            if (GameModel.TowerNumber == 3)
            {
                _unlockButton.SetActive(false);
            }
        }

        private void OnLevelStart()
        {
            _slider.value = 1f;
            _waveTMP.text = "Wave 1";
        }

        public void OnProgressBarUpdated(OnProgressBarUpdated onProgressBarUpdated)
        {
            _waveTMP.text = "Wave " + onProgressBarUpdated.waveNumber;
            _slider.value = onProgressBarUpdated.value;
        }
        public void OnCoinUpdated(OnCoinUpdated onCoinUpdated)
        {
            _coinTMP.text = onCoinUpdated.amount.ToString();
        }
    }
}