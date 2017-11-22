using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.SuperUI
{
    public class SuperUiTrigger : SuperUIBase
    {
        public GameObject turnOn;
        public GameObject turnOff;
        public DefaultState defaultState =DefaultState.开;

        /// <summary>
        /// 仅会在从关闭到开启时执行
        /// </summary>
        public UnityEvent m_OnClickTurnOn = new UnityEvent();

        /// <summary>
        /// 点击执行
        /// </summary>
        public UnityEvent m_OnClick = new UnityEvent();

        public delegate void ToggleCallBack(SuperUiTrigger bt);
        /// <summary>
        /// 由组（SuperToggleGroup）进行赋值与调用
        /// </summary>
        [HideInInspector] public ToggleCallBack triggerCallBack;
        [HideInInspector] public bool hasGroup = false;
        //private GameObject[][] switchImage;
        private bool _isOn;
        public bool IsOn
        {
            get { return _isOn; }
            set
            {
                _isOn = value;
                if (value)
                {
                    if (turnOn != null) turnOn.SetActive(true);
                    if (turnOff != null) turnOff.SetActive(false);
                }
                else
                {
                    if (turnOn != null) turnOn.SetActive(false);
                    if (turnOff != null) turnOff.SetActive(true);
                }
           
                if (hasGroup && value)
                {
                    MessageCenter.Instance.PostMsg("IsOnTrueInSupperTrigger", this);
                }
            }
        }

        void Awake()
        {
            if (!hasGroup) IsOn = defaultState==DefaultState.开;
        }

        protected override void Bt_Click()
        {
            //如果没有组绑定，没有绑定点击一定会切换状态
            if (!hasGroup)
            {
                IsOn = !IsOn;
            }
            else
            {
                //如果有组绑定，重复点击一个开关，不一定会切换状态
                if (!IsOn) IsOn = true;
                triggerCallBack(this);
            }
            //从关闭到开启时执行
            if (IsOn) m_OnClickTurnOn.Invoke();
            //点击就执行
            m_OnClick.Invoke();
        }
    }
}

public enum DefaultState
{
    开,
    关,
}