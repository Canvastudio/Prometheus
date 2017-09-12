using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class HudBase : GameBase {

	public HudStateType state = HudStateType.Hidden;
	public HudType _type;

	public int Z_deep = 0;
	public int index;

	public RectTransform rectTran;

	public Canvas canvas;

	public override void InitAwake ()
	{
		base.InitAwake ();

		Messenger.AddListener(HudInstruction.UI_REFRESH, OnRefresh);
	}

	public virtual void OnEnable() {
	
		OnShow();
		index = tran.GetSiblingIndex ();
	
	}

	public void SetIndex(int _index) {
	
		index = _index;
		tran.SetSiblingIndex (index);
	
	}

	public void OnDisable() { OnClose(); }

    public virtual void OnRefresh() {
    }

	public virtual void OnShow() {}

	public virtual void OnClose() {

		if(state == HudStateType.Hidden)
			
			this.gameObject.SetActive(false);
		
		else if(state == HudStateType.Destroy) {
			
            Messenger.RemoveListener(HudInstruction.UI_REFRESH, OnRefresh);

			Remove();
		}

	}

	public virtual void OnRemove() {}

	public void OnDrawGizmos() {
		this.BindHud();
	}

}

public enum HudStateType {

	None,
	Hidden,
	Show,
	Destroy,
	Hand,

}

public enum HudOpenType {

	Open,
	AddOpen,

}