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

    public static IEnumerator DoSkill(string name, Vector3 _start_pos, Vector3 _end_pos)
    {
        GameObject objskill = FxPool.Get(FxEnum.Skill, name);
        yield return objskill.GetComponent<ArtEventFlow>().Show(_start_pos, _end_pos);
    }

}
