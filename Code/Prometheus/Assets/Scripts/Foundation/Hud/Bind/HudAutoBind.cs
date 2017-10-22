using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using UnityEngine.UI;

public static class HudAutoBind {

	public static void Bind(object tobj, GameObject obj) {
	
//		Type t = tobj.GetType();
//
//		FieldInfo[] fields = t.GetFields();
//
//		foreach (FieldInfo field in fields) {
//
//			string name = field.Name;
//
//			Transform tranFind = BoneHelper.GetTran(obj.transform, name);
//
//			if (tranFind != null) {
//
//				if (name.Contains("Text_")) {
//
//					field.SetValue(tobj, tranFind.GetComponent<Text>());
//
//				} 
//
//				if (name.Contains("Slider")) {
//
//					field.SetValue(tobj, tranFind.GetComponent<Slider>());
//
//				}
//
//				if (name.Contains("Button_")) {
//
//					field.SetValue(tobj, tranFind.gameObject);
//
//				}
//
//				if (name.Contains("Image_")) {
//				
//					field.SetValue(tobj, tranFind.GetComponent<Image>());
//				
//				}
//
//			}
//
//		}

		Bind("", tobj, obj);
	
	}

	public static void BindItem(object tobj, GameObject obj) {
	
		Bind("Item", tobj, obj);
	
	}

	private static void Bind(string tag, object tobj, GameObject obj) {
	
		Type t = tobj.GetType();

		FieldInfo[] fields = t.GetFields();

		string bind_Text = "Text" + tag + "_";
		string bind_Slider = "Slider" + tag + "_";
		string bind_Button = "Button" + tag + "_";
		string bind_Image = "Image" + tag + "_";

		foreach (FieldInfo field in fields) {

			string name = field.Name;

			Transform tranFind = BoneHelper.GetTran(obj.transform, name);

			if (tranFind != null) {

				if (name.Contains(bind_Text)) {

					field.SetValue(tobj, tranFind.GetComponent<Text>());

				} 

				if (name.Contains(bind_Slider)) {

					field.SetValue(tobj, tranFind.GetComponent<Slider>());

				}

				if (name.Contains(bind_Button)) {

					field.SetValue(tobj, tranFind.gameObject);

				}

				if (name.Contains(bind_Image)) {

					field.SetValue(tobj, tranFind.GetComponent<Image>());

				}

			}

		}
	
	}

	public static void BindHud(this HudBase hud) {

		hud.obj = hud.gameObject;
		hud.canvas = hud.GetComponent<Canvas>();
		hud._type = (HudType)System.Enum.Parse(typeof(HudType), hud.obj.name, false);
		hud.tran = hud.transform;
		hud.rectTran = hud.GetComponent<RectTransform>();
	
		List<string> autoList = new List<string>() {};

		if(autoList.Contains(hud.name)) return;

		Type t = hud.GetType();

		FieldInfo[] fields = t.GetFields();

		foreach(FieldInfo field in fields) {

			string name = field.Name;

			Transform tranFind = BoneHelper.GetTran(hud.transform, name);

			if(tranFind != null) {

				if (name.Contains("Text_")) {

					field.SetValue(hud, tranFind.GetComponent<Text>());

				} 

				if (name.Contains("Slider")) {

					field.SetValue(hud, tranFind.GetComponent<Slider>());

				}

				if(name.Contains("Button_")){

					field.SetValue(hud, tranFind.gameObject);

				}

				if (name.Contains("ImageB_")) {

					field.SetValue(hud, tranFind.GetComponent<Image>());

				}

				if (name.Contains("InputField_")) {
				
					field.SetValue(hud, tranFind.GetComponent<InputField>());
				
				}

			}


		}
	
	}

}
