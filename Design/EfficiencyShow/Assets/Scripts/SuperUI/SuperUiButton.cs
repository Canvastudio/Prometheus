using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.SuperUI
{
    /// <summary>
    /// 必须把可以响应点击的物体（可见物体）放在本脚本所在gameobject的子物体上
    /// </summary>
    public class SuperUiButton : SuperUIBase
    {
        public GameObject up;
        public GameObject down;
        public GameObject failure;
        public SupperButtonState defaultState;

        public UnityEvent m_OnLongpress = new UnityEvent();
        public UnityEvent m_OnClick = new UnityEvent();
        //public UnityEvent m_OnDown = new UnityEvent();
        //public UnityEvent m_OnUp = new UnityEvent();

        private SupperButtonState _buttonState;

        public SupperButtonState ButtonState
        {
            set
            {
                _buttonState = value;
                if (value == SupperButtonState.Normal)
                {
                    IsDown = _isDown;
                    if (failure != null) failure.SetActive(false);
                }
                else
                {
                    if (up != null) up.SetActive(false);
                    if (down != null) down.SetActive(false);
                    if (failure != null) failure.SetActive(true);
                }
            }
            get { return _buttonState; }
        }

        private bool _isDown;
        private bool IsDown
        {
            set
            {
                _isDown = value;
                if (value)
                {
                    if (up != null) up.SetActive(false);
                    if (down != null) down.SetActive(true);
                }
                else
                {
                    if (up != null) up.SetActive(true);
                    if (down != null) down.SetActive(false);
                }
            }
            get { return _isDown; }
        }

        void Awake()
        {
            ButtonState = defaultState;
        }

        protected override void Bt_Click()
        {
            if (ButtonState == SupperButtonState.Normal)
                m_OnClick.Invoke();
        }

        protected override void Bt_Dowm()
        {
            if (ButtonState == SupperButtonState.Normal)
                IsDown = true;
        }

        protected override void Bt_OnLongpress()
        {
            if (ButtonState == SupperButtonState.Normal)
                m_OnLongpress.Invoke();
        }

        protected override void Bt_Up()
        {
            if (ButtonState == SupperButtonState.Normal && IsDown)
                IsDown = false;
        }
    }

    public enum SupperButtonState
    {
        Normal,
        Failure
    }
}