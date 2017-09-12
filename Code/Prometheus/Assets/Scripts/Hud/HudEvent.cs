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

    public Vector3 scaleVec = Vector3.one * 1.2f;
    public Vector3 orignalVec;

    private Button button;

	void Awake() {
	
		button = this.GetComponent<Button>();
		if(button != null) {
		
			button.onClick.AddListener(OnClick);

            //guide event
            //GuideManager.Instance.OnBtnGuide(this.gameObject);
		
		}

        orignalVec = transform.lossyScale;
        scaleVec = orignalVec * 1.1f;
	
	}

//	static public HudEvent Get (GameObject go)
//	{
//		HudEvent listener = go.GetComponent<HudEvent>();
//		if (listener == null) listener = go.AddComponent<HudEvent>();
//		return listener;
//	}

	public static HudEvent Get(GameObject go) {

		HudEvent hudEvent = go.GetComponent<HudEvent>();

		if(hudEvent == null) hudEvent = go.AddComponent<HudEvent>();

		return hudEvent;

	}

	private void OnClick() {
	
        CommonEvent();
        //SoundManager.Instance.Play("button2", 0.1f);
		if(onClick != null) { 

            onClick();
        
        }
	
	}

	void OnDestroy() {

	
	}

	/*****************************new**********************************/

//	public override void OnPointerClick(PointerEventData eventData)
//	{
//		if(onClick != null) 	onClick(gameObject);
//	}
	public override void OnPointerDown (PointerEventData eventData){
        CommonEvent();
        //LeanTween.scale(this.gameObject, scaleVec, 0.2f);
		if(onDown != null) onDown(gameObject);
	}
	public override void OnPointerEnter (PointerEventData eventData){
        CommonEvent();
		if(onEnter != null) onEnter(gameObject);
	}
	public override void OnPointerExit (PointerEventData eventData){
        CommonEvent();
		if(onExit != null) onExit(gameObject);
	}
	public override void OnPointerUp (PointerEventData eventData){
        CommonEvent();
        //LeanTween.scale(this.gameObject, orignalVec, 0.2f);
		if(onUp != null) onUp(gameObject);
	}
	public override void OnSelect (BaseEventData eventData){
		if(onSelect != null) onSelect(gameObject);
	}
	public override void OnUpdateSelected (BaseEventData eventData){
		if(onUpdateSelect != null) onUpdateSelect(gameObject);
	}


    public void CommonEvent() {
    
    
    }

}
