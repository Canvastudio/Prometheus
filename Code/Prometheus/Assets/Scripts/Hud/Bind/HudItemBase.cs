using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudItemBase : MonoBehaviour {

	public int index = 0;

	public virtual void OnEnable() {

		OnShow();

	}

	public virtual void OnShow() {
	}

	public virtual void OnItemRefresh() {
	}

	public void OnDrawGizmos() {
		HudAutoBind.BindItem(this, this.gameObject);
		index = this.transform.GetSiblingIndex();
	}
}
