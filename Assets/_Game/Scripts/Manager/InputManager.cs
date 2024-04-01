using Scripts.Keys;
using Scripts.Signals;
using UnityEngine;
using Zenject;

namespace Scripts.Manager
{
    public class InputManager : MonoBehaviour
    {

        private InputDataParams _inputDataParams;
        private float _clickInterval = 0.4f;
        private float _lastClickTime;
        private SignalBus _signalBus;
        private OnTapSignal _onTapSignal;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _lastClickTime = int.MinValue;
            _onTapSignal = new OnTapSignal();
        }

        public void Update()
        {
            if (Time.timeSinceLevelLoad - _lastClickTime < _clickInterval) return;
            
            if(Input.GetMouseButton(0))
            {
                _lastClickTime = Time.timeSinceLevelLoad;
                _inputDataParams.width = (int)Input.mousePosition.x;
                _inputDataParams.height = (int)Input.mousePosition.y;

                _onTapSignal.InputDataParams = _inputDataParams;
                _signalBus.Fire(_onTapSignal);
            }
        }
    }
}