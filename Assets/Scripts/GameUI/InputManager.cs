using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class InputManager : MonoBehaviour{
 
        public static InputManager Singleton { get; private set; }
        
        public Button leftButton;
        public Button rightButton;
        private bool _leftButtonPressed;
        private bool _rightButtonPressed;

        private void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            leftButton.onClick.AddListener(OnLeftButtonClick);
            rightButton.onClick.AddListener(OnRightButtonClick);
        }

        public void LateUpdate()
        {
            _leftButtonPressed = false;
            _rightButtonPressed = false;
        }

        private void OnLeftButtonClick()
        {
            _leftButtonPressed = true;
        }

        private void OnRightButtonClick()
        {
            _rightButtonPressed = true;
        }
        
        public bool GetLeftInput() => Input.GetKeyDown(KeyCode.LeftArrow) || _leftButtonPressed;

        public bool GetRightInput() => Input.GetKeyDown(KeyCode.RightArrow) || _rightButtonPressed;
    }
}