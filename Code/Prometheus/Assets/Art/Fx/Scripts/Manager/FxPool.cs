using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxPool {

	public static List<GameObject> objpool = new List<GameObject>(100);

	public static void ClearAll() {
		objpool.Clear();
	}

	public static GameObject Get(FxEnum fxenum, string name) {
	
		if (name.Trim().Length <= 0)
			return default(GameObject);

		for (int i = 0; i < objpool.Count; i++) {
		
			GameObject obj = objpool[i];
			if (obj.name.Equals(name)) {
				obj.SetActive(true);
				objpool.Remove(obj);
				return obj;
			}
		
		}

		GameObject newObj = GameObject.Instantiate(Resources.Load(fxenum.ToString() + "/" + name)) as GameObject;
		newObj.name = name;
		newObj.SetActive(true);
		return newObj;
	
	}

	public static void Recover(GameObject obj) {
	
		obj.SetActive(false);

		if(!objpool.Contains(obj))
			objpool.Add(obj);
	
	}

}

public enum FxEnum {

	Fx,
	Skill,

}