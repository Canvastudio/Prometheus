using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class HudHelper {

	public const string Str_Text_F = "Text_";
	public const string Str_Text_L = "TextL_";
	public const string Str_Button = "Button_";
	public const string Str_Image = "ImageB_";
	public const string Str_Slider = "Slider_";

	public static string _property = "";
	public static string _initevent = "";
	public static string _function = "";

	public static void CreateHuds() {
	
		GameObject obj = Selection.activeGameObject;

		foreach(Transform tran in obj.transform) {
		
			CreateHud(tran.gameObject);
		
		}
	
	}


	public static void CreateHud( ) {

		GameObject obj = Selection.activeGameObject;
		CreateHud(obj);

	}

	public static void CreateHud(GameObject obj) {

		string name = obj.name;

		string templatePath = Application.dataPath + "/Scripts/CFramework/Hud/Editor/HudTemplate";
		string savePath = Application.dataPath + "/Scripts/Logic/Hud/" + name + ".cs";
		string templateStr = IOManager.GetTxt(templatePath);

		_property = "";
		_initevent = "";
		_function = "";

		Transform tran = obj.transform;

		List<Transform> tranInputField = tran.BlurFind("InputField_");
		CreateInputField(tranInputField);

		List<Transform> tranTextF = tran.BlurFind(Str_Text_F);
		CreateText(tranTextF);

		List<Transform> tranButton = tran.BlurFind(Str_Button);
		CreateButton(tranButton);

		List<Transform> tranImage = tran.BlurFind(Str_Image);
		CreateImage(tranImage);

		List<Transform> tranSlider = tran.BlurFind(Str_Slider);
		CreateSlider(tranSlider);

		templateStr = templateStr.Replace("#name#", name);
		templateStr = templateStr.Replace("#Property#", _property);
		templateStr = templateStr.Replace("#InitEvent#", _initevent);
		templateStr = templateStr.Replace("#Function#", _function);

		Debug.Log(templateStr);

		IOManager.SaveFile(savePath, templateStr);

		List<Transform> tranItem = tran.BlurFind("ItemF_");

		for (int i = 0; i < tranItem.Count; i++) {
		
			CreateHudItem(tranItem[i].gameObject);
		
		}

		List<Transform> tranTextL = tran.BlurFind(Str_Text_L);

		//BindLocalize(tranTextL);

		AssetDatabase.Refresh();
	
	}

	public static void CreateHudItem(GameObject obj) {
	
		string name = obj.name;

		string templatePath = Application.dataPath + "/Scripts/CFramework/Hud/Editor/HudItemTemplate";
		string savePath = Application.dataPath + "/Scripts/Logic/Hud/" + name + ".cs";
		string templateStr = IOManager.GetTxt(templatePath);

		_property = "";
		_initevent = "";
		_function = "";

		Transform tran = obj.transform;

		List<Transform> tranTextF = tran.BlurFind("TextItem_");
		CreateText(tranTextF);

		List<Transform> tranButton = tran.BlurFind("ButtonItem_");
		CreateButton(tranButton);

		List<Transform> tranImage = tran.BlurFind("ImageItem_");
		CreateImage(tranImage);

		List<Transform> tranSlider = tran.BlurFind("SliderItem_");
		CreateSlider(tranSlider);

		templateStr = templateStr.Replace("#name#", name);
		templateStr = templateStr.Replace("#Property#", _property);
		templateStr = templateStr.Replace("#InitEvent#", _initevent);
		templateStr = templateStr.Replace("#Function#", _function);

		IOManager.SaveFile(savePath, templateStr);

		AssetDatabase.Refresh();
	
	}

	private static void CreateSlider(List<Transform> trans) {

		for(int i = 0; i < trans.Count; i++) {

			_property += "\tpublic Slider " + trans[i].name + "; \r\n";

		}

	}

	private static void CreateImage(List<Transform> trans) {
	
		for(int i = 0; i < trans.Count; i++) {

			_property += "\tpublic Image " + trans[i].name + "; \r\n";

		}
	
	}

	private static void CreateText(List<Transform> trans) {
	
		for(int i = 0; i < trans.Count; i++) {

			_property += "\tpublic Text " + trans[i].name + "; \r\n";

		}
	
	}

	public static void CreateButton(List<Transform> trans) {
	
		for(int i = 0; i < trans.Count; i++) {

			_property += "\tpublic GameObject " + trans[i].name + "; \r\n";

			string b_name = trans[i].name.Replace(Str_Button, "").Trim();

			_initevent += string.Format("\t\tHudEvent.Get({0}).onClick = OnClick_{1}; \r\n", trans[i].name, b_name);

			_function += string.Format("\tpublic void OnClick_{0}() {1} {2}\r\n", b_name, "{", "}");

		}
	
	}

	public static void CreateInputField(List<Transform> trans) {
	
		for(int i = 0; i < trans.Count; i++) {

			_property += "\tpublic InputField " + trans[i].name + "; \r\n";

		}
	
	}

//	private static void BindLocalize(List<Transform> trans) {
//	
//		for(int i = 0; i < trans.Count; i++) {
//		
//			Localize_UGUI local = trans[i].GetComponent<Localize_UGUI>();
//
//			if(local == null)
//				local = trans[i].gameObject.AddComponent<Localize_UGUI>();
//
//			local.key = trans[i].name.Replace(Str_Text_L, "").Trim();
//		
//		}
//	
//	}

}
