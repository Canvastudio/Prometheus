using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.SuperUI
{
    public class SuperUIBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        /// <summary>
        /// 总开关，一旦关闭所有按钮将不响应点击
        /// </summary>
        public  static bool Alow = true;
        public float interval = 0.5f;
        /// <summary>
        /// 单个按钮是否允许点击
        /// </summary>
        [HideInInspector] public bool alow = true;

        private bool isOutOfRage;
        private bool btDown;
        private float lastInvokeTime;
        private bool isShowLongpress;

        void Update()
        {
            if (!alow || !Alow) return;
            if (!isOutOfRage && !isShowLongpress && btDown)
            {
                if (Time.time - lastInvokeTime > interval)
                {
                    Bt_OnLongpress();
                    isShowLongpress = true;
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!alow || !Alow) return;
            Bt_Dowm();
            btDown = true;
            isShowLongpress = false;
            isOutOfRage = false;
            lastInvokeTime = Time.time;
        }


        public void OnPointerUp(PointerEventData eventData)
        {
            //即使是开关关闭也要响应抬起，否则会出现在按下时关闭开关，放开手指按钮不抬起的情况
            btDown = false;
            Bt_Up();
            if (!alow || !Alow) return;
            if (!isShowLongpress &&  !isOutOfRage && !eventData.dragging)
            {
                Bt_Click();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isOutOfRage =true;
        }
    

        /// <summary>
        /// 按钮被按下
        /// </summary>
        protected virtual void Bt_Dowm() { }

        /// <summary>
        /// 按钮被抬起，移出范围也判定为抬起
        /// </summary>
        protected virtual void Bt_Up() { }

        /// <summary>
        /// 点击
        /// </summary>
        protected virtual void Bt_Click() { }

        /// <summary>
        /// 长按
        /// </summary>
        protected virtual void Bt_OnLongpress() { }



    }
}
