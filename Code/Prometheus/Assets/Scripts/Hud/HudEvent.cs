using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HudEvent : EventTrigger {

	public delegate void VoidDelegate (GameObject go);

	public VoidDelegate onDown;
	public VoidDelegate onEnter;
	public VoidDelegate onExit;
	public VoidDelegate onUp;
	public VoidDelegate onSelect;
	public VoidDelegate onUpdateSelect;

	public VoidDelegate onDrag;

	public Callback onClick = null;
    public Callback onLongPress = null;

    public float pressTriggerTime = 0.4f;
        
    public Vector3 scaleVec = Vector3.one * 1.2f;
    public Vector3 orignalVec;

    private float pointDownTime = float.MaxValue;
    private bool isDown = false;
    private bool isLongPressTriggerd = false;
    private Button button;

	void Awake() {
	
		button = this.GetComponent<Button>();

		if(button != null) {
			button.onClick.AddListener(OnClick);
		}

        orignalVec = transform.lossyScale;
        scaleVec = orignalVec * 1.1f;
	
	}

	public static HudEvent Get(GameObject go) {

		HudEvent hudEvent = go.GetComponent<HudEvent>();

		if(hudEvent == null) hudEvent = go.AddComponent<HudEvent>();

		return hudEvent;

	}
    
	private void OnClick() {

        if (isLongPressTriggerd) return;

        Debug.Log("HueEvent: Click: " + gameObject.name);

        CommonEvent();
        //SoundManager.Instance.Play("button2", 0.1f);
		if(onClick != null) { 


            onClick();
        
        }
	
	}

	void OnDestroy() {

	
	}

    private void Update()
    {
        if (!isLongPressTriggerd && isDown)
        {
            if (Time.timeSinceLevelLoad - pointDownTime >= pressTriggerTime)
            {
                if (onLongPress != null)
                {
                    onLongPress.Invoke();
                }

                isLongPressTriggerd = true;
                Debug.Log("HueEvent: LongPress: " + gameObject.name);
            }
        }
    }

    /*****************************new**********************************/
    public override void OnPointerDown (PointerEventData eventData){

        isDown = true;
        isLongPressTriggerd = false;

        pointDownTime = Time.timeSinceLevelLoad;

        CommonEvent();

		if(onDown != null) onDown(gameObject);
	}
    
	public override void OnPointerEnter (PointerEventData eventData){

        CommonEvent();
		if(onEnter != null) onEnter(gameObject);
	}
	public override void OnPointerExit (PointerEventData eventData){

        pointDownTime = float.MaxValue;

        CommonEvent();
		if(onExit != null) onExit(gameObject);
	}
	public override void OnPointerUp (PointerEventData eventData){

        isDown = false;

        CommonEvent();
		if(onUp != null) onUp(gameObject);
	}
	public override void OnSelect (BaseEventData eventData){

        if (onSelect != null) onSelect(gameObject);
	}

    public void CommonEvent() {
    
    
    }

}
