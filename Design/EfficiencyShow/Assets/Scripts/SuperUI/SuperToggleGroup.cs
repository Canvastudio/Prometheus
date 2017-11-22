using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.SuperUI
{
    [System.Serializable]
    public class OneArgEvent : UnityEvent<int> { }
    public class SuperToggleGroup : MonoBehaviour
    {
        public bool enable = true;

        public int triggerNum
        {
            get { return triggers.Count; }
        }

        /// <summary>
        /// 默认是哪一个按钮开启
        /// </summary>
        [SerializeField]
        private int _defaultOn;
        /// <summary>
        /// 设置它不会执行点击事件，只是改变其显示状态，负值表示全部都关闭
        /// </summary>
        public int defaultOn
        {
            get { return _defaultOn; }
            set
            {
                _defaultOn = value;
                //Count为0发生在动态添加trigger的时候
                if (triggers.Count <= 0 || _defaultOn >= triggers.Count) return;
                if (_defaultOn >= triggers.Count && _defaultOn > 0)
                {
                    throw new ArgumentException("开启的开关被设置为了一个超过其数量的值，如果要全部关闭，设置为一个负数");
                }
                if (_defaultOn < 0)
                {
                    foreach (SuperUiTrigger t in triggers)
                        t.IsOn = false;
                    return;
                }
                triggers[_defaultOn].IsOn = true;

            }
        }

        /// <summary>
        /// 点击一个已经是开启的开关，是否响应事件
        /// </summary>
        public bool isRespondWhenClickOn = false;

        [SerializeField]
        private List<SuperUiTrigger> triggers;
        [SerializeField]
        public OneArgEvent m_OnClick = new OneArgEvent();

        private bool _alow = true;
        public bool Alow
        {
            set
            {
                _alow = value;
                foreach (var trigger in triggers)
                {
                    trigger.alow = _alow;
                }
            }
            get { return _alow; }
        }

        private List<bool> triggerStates;
        public delegate object ComparFunc(GameObject gameObject);


        /// <summary>
        /// 初始化不能放到start里，因为trigger的start会提前执行
        /// </summary>
        void Awake()
        {
            if (!enable) return;
            MessageCenter.Instance.AddListener("IsOnTrueInSupperTrigger", OnTriggerIsOnChange);

            triggerStates = new List<bool>();
            if (global::SuperTool.HasRepeat(triggers)) throw new ArgumentException("重复的Trigger！");
            for (int i = 0; i < triggers.Count; i++)
            {
                triggers[i].triggerCallBack += OnClick;
                triggers[i].hasGroup = true;
            }
            //动态添加时，riggers.Count会为0
            if (triggers.Count <= 0) return;
            defaultOn = _defaultOn;
        }


        void OnDestroy()
        {
            MessageCenter.Instance.RemoveListener("IsOnTrueInSupperTrigger", OnTriggerIsOnChange);
        }

        /// <summary>
        /// 当组下某个Trigger被开启，调用此方法
        /// </summary>
        private void OnTriggerIsOnChange(object o)
        {
            SuperUiTrigger uiTrigger = o as SuperUiTrigger;

            if (triggers.Contains(uiTrigger))
            {
                for (int i = 0; i < triggers.Count; i++)
                {
                    if (triggers[i] != uiTrigger)
                        triggers[i].IsOn = false;
                }
            }
        }

        /// <summary>
        /// 清空绑定的按钮，参数指定是否销毁按钮物体
        /// </summary>
        /// <param name="isDestroy">是否销毁按钮物体</param>
        public void ClearTrigger(bool isDestroy)
        {
            if (isDestroy)
            {
                foreach (SuperUiTrigger t in triggers)
                {
                    Destroy(t.gameObject);
                }
            }
            else
            {
                foreach (SuperUiTrigger t in triggers)
                    t.triggerCallBack -= OnClick;
                //SetIndex(_defaultOn);
            }
            triggers.Clear();
            triggerStates.Clear();
        }

        /// <summary>
        /// 动态增加绑定的开关
        /// </summary>
        public void AddTrigger(SuperUiTrigger uiTrigger)
        {
            if (triggers.Contains(uiTrigger)) throw new ArgumentException("重复的Trigger被添加！");
            uiTrigger.hasGroup = true;
            uiTrigger.triggerCallBack += OnClick;
            uiTrigger.alow = Alow;
            triggers.Add(uiTrigger);
            triggerStates.Add(uiTrigger.IsOn);
            uiTrigger.IsOn = triggers.Count - 1 == _defaultOn;
        }

        /// <summary>
        /// 获取一个开关上的组建
        /// </summary>
        public T GetTriggerComponent<T>(int index)
        {
            return triggers[index].GetComponent<T>();
        }


        /// <summary>
        /// 内部比较哪一个Trigger符合开启条件
        /// </summary>
        /// <param name="obj">要比较的目标参数</param>
        /// <param name="func">比较方法，回调参数是Trigger所在的GameObject，返回的值与目标参数做比较</param>
        public void SetTriggerOn(object obj, ComparFunc func)
        {
            for (int i = 0; i < triggers.Count; i++)
            {
                if (func(triggers[i].gameObject) == obj)
                {
                    defaultOn = i;
                    return;
                }
            }
            throw new ArgumentException("没有找到要设置开启的Trigger");
        }


        private void OnClick(SuperUiTrigger bt)
        {
            for (int i = 0; i < triggers.Count; i++)
            {
                if (triggers[i] != bt)
                {
                    if (triggers[i].IsOn) triggers[i].IsOn = false;
                }
                else
                {
                    //如果点击的是已经开启的开关，需要用isRespondWhenClickOn来控制是否响应
                    if (_defaultOn != i)
                    {
                        m_OnClick.Invoke(i);
                        _defaultOn = i;
                    }
                    else if (isRespondWhenClickOn)
                    {
                        m_OnClick.Invoke(i);
                    }

                }
            }
        }


    }
}