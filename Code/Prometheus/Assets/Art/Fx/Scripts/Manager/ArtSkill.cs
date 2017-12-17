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

			if (eventFlow.isend) {
				yield return true;
				Debug.Log(objskill.name + "____finished");
				break;
			} else
				yield return null;
		}

	}

	public static IEnumerator DoSkillIE(string name, Transform tranStart, Transform tranEnd, Callback _callback = null) {

		if (string.IsNullOrEmpty(name)) {
		
			if(_callback != null) _callback();
			yield break;

		}

		if(name.Trim().Length <= 0) {
			
			if(_callback != null) _callback();
			yield break;

		}

		GameObject objskill = FxPool.Get(FxEnum.Skill, name);
		ArtEventFlow eventFlow = objskill.GetComponent<ArtEventFlow>();

		if(eventFlow == null) {
		
			if(_callback != null) _callback();
			yield break;

		}

		eventFlow.Init(tranStart, tranEnd, _callback);

		while(true) {

			if (eventFlow.isend) {
				//Debug.Log("art logic end." + Time.time);
				yield break;
			} else
				yield return true;
		}

	}

	public static ArtFxBase ShowDrop(string name, Vector3 pos, int count) {
		
		ArtFxBase fx =  Show(name, pos);

		if (fx != null)
			fx.Show(pos, count);

		return fx;	

	}

	public static ArtFxBase Show(string name, Vector3 pos) {

		GameObject objskill = FxPool.Get(FxEnum.Fx, name);

//		objskill.transform.position = pos;

		ArtFxBase artFxBase = objskill.GetComponent<ArtFxBase>();

		artFxBase.SetPos(pos);

		return artFxBase;

	}

	public static ArtFxBase Show(string name, Vector3 pos, float size) {

		ArtFxBase fx =  Show(name, pos);

		if (fx != null)
			fx.SetSize(size);

		return fx;
	
	}

	public static void DoNormalAtk(string name, Transform tranStart, Transform tranEnd) {

		GameObject objskill = FxPool.Get(FxEnum.Skill, name);
		ArtEventFlow eventFlow = objskill.GetComponent<ArtEventFlow>();
		eventFlow.Init(tranStart, tranEnd, null);	

	}


}
