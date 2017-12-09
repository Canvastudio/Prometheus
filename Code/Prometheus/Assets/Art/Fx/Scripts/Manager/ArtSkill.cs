using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtSkill {

	public static void DoSkill(string name, Vector3 _start_pos, Vector3 _end_pos, Callback _callback = null) {
	
		GameObject objskill = FxPool.Get(FxEnum.Skill, name);
		objskill.GetComponent<ArtEventFlow>().Init(_start_pos, _end_pos, _callback);
	
	}

	public static void DoSkill(string name, Vector3 _start_pos, Callback _callback = null) {
	
		GameObject objskill = FxPool.Get(FxEnum.Skill, name);
		objskill.GetComponent<ArtEventFlow>().Init(_start_pos, _start_pos, _callback);
	
	}

	public static IEnumerator DoSkillIE(string name, Vector3 _start_pos, Vector3 _end_pos, Callback _callback = null) {

		GameObject objskill = FxPool.Get(FxEnum.Skill, name);
		ArtEventFlow eventFlow = objskill.GetComponent<ArtEventFlow>();
		eventFlow.Init(_start_pos, _end_pos, null);

		while(true) {

			if (eventFlow.ishit) {
				yield return true;
				Debug.Log(objskill.name + "____finished");
				break;
			} else
				yield return null;
		}

	}

	public static IEnumerator DoSkillIE(string name, Transform tranStart, Transform tranEnd, Callback _callback = null) {

		GameObject objskill = FxPool.Get(FxEnum.Skill, name);
		ArtEventFlow eventFlow = objskill.GetComponent<ArtEventFlow>();
		eventFlow.Init(tranStart, tranEnd, _callback);

		while(true) {

			if (eventFlow.ishit) {
				yield return true;
				//Debug.Log(objskill.name + "____finished");
				break;
			} else
				yield return null;
		}

	}

	public static ArtFxBase Show(string name, Vector3 pos) {

		GameObject objskill = FxPool.Get(FxEnum.Fx, name);

		objskill.transform.position = pos;

		return objskill.GetComponent<ArtFxBase>();

	}

	public static ArtFxBase Show(string name, Vector3 pos, float size) {

		ArtFxBase fx =  Show(name, pos);

		if (fx != null)
			fx.SetSize(size);

		return fx;
	
	}



}
