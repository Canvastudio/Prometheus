using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#if UNITY_EDITOR

using System;
using System.Reflection;
using System.Collections.Generic;

#endif

public class HudAutoCode : GameBase {

#if UNITY_EDITOR

    public void OnDrawGizmos() {
		AutoBind();
    }

	public virtual void AutoBind() {
	
		List<string> autoList = new List<string>() {


		};

		if(!autoList.Contains(this.name)) return;

		Type t = this.GetType();

		FieldInfo[] fields = t.GetFields();

		foreach(FieldInfo field in fields) {

			string name = field.Name;

			Transform tranFind = BoneHelper.GetTran(this.transform, name);
			
			if(tranFind != null) {

				if (name.Contains("Text_")) {

					field.SetValue(this, tranFind.GetComponent<Text>());

				} 

				if (name.Contains("Slider")) {
				
					field.SetValue(this, tranFind.GetComponent<Slider>());
				
				}

				if(name.Contains("Button_")){
				
					field.SetValue(this, tranFind.gameObject);
				
				}
			}
		}
	}

    #endif
	
}
