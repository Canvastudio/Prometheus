using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ECTran {

	#region Set Transform postion

	public static void SetX(Transform tran, float x) {
	
		Vector3 newPostion = new Vector3 (x, tran.position.y, tran.position.z);
		tran.position = newPostion;
	
	}

	public static void SetY(Transform tran, float y) {
	
		Vector3 newPosition = new Vector3 (tran.position.x, y, tran.position.z);
		tran.position = newPosition;
	
	}

	public static void SetZ(Transform tran, float z) {
	
		Vector3 newPosition = new Vector3 (tran.position.x, tran.position.y, z);
		tran.position = newPosition;
	
	}

	#endregion

	#region Set Transform local position 

	public static void SetLX(Transform tran, float x) {
	
		Vector3 newPosition = new Vector3 (x, tran.localPosition.y, tran.localPosition.z);
		tran.localPosition = newPosition;
	
	}

	public static void SetLY(Transform tran, float y) {
	
		Vector3 newPosition = new Vector3 (tran.localPosition.x, y, tran.localPosition.z);
		tran.localPosition = newPosition;

	}

	public static void SetLZ(Transform tran, float z) {
	
		Vector3 newPosition = new Vector3 (tran.localPosition.x, tran.localPosition.y, z);
		tran.localPosition = newPosition;
	
	}

	#endregion

	#region Set tansform eulerAngle

	public static void SetAngleX(Transform tran, float angleX) {
	
		Vector3 newAngle = tran.eulerAngles;
		newAngle.x = angleX;
		tran.eulerAngles = newAngle;
	
	}

	public static void SetAngleY(Transform tran, float angleY) {
	
		Vector3 newAngle = tran.eulerAngles;
		newAngle.y = angleY;
		tran.eulerAngles = newAngle;
	
	}

	public static void SetAngleZ(Transform tran, float angleZ) {
	
		Vector3 newAngle = tran.eulerAngles;
		newAngle.z = angleZ;
		tran.eulerAngles = newAngle;
	
	}

	#endregion

	#region Set tranform local eulerAngle

	public static void SetLAngleX(Transform tran, float angleX) {

		Vector3 newAngle = tran.localEulerAngles;
		newAngle.x = angleX;
		tran.localEulerAngles = newAngle;
	
	}

	public static void SetLAngleY(Transform tran, float angleY) {

		Vector3 newAngle = tran.localEulerAngles;
		newAngle.y = angleY;
		tran.localEulerAngles = newAngle;
	
	}

	public static void SetLAngleZ(Transform tran, float angleZ) {

		Vector3 newAngle = tran.localEulerAngles;
		newAngle.z = angleZ;
		tran.localEulerAngles = newAngle;
	
	}

	#endregion

	#region Set tranform local Scale

	public static void SetScaleX(Transform tran, float scaleX) {
		
		Vector3 newScale = tran.localScale;
		newScale.x = scaleX;
		tran.localScale = newScale;
		
	}

	public static void SetScaleY(Transform tran, float scaleY) {

		Vector3 newScale = tran.localScale;
		newScale.y = scaleY;
		tran.localScale = newScale;
	
	}

	public static void SetScaleZ(Transform tran, float scaleZ) {
	
		Vector3 newScale = tran.localScale;
		newScale.z = scaleZ;
		tran.localScale = newScale;
	
	}

	#endregion

	#region Tools

	public static void RemoveChilds<T> (T t_obj) where T : Component {
	
		T[] trans = t_obj.GetComponentsInChildren<T>(true);

		for(int i = 0; i < trans.Length; i++) {
		
			if(trans[i] != t_obj)
				GameObject.Destroy(trans[i].gameObject);
		
		}
	
	}

	public static List<Transform> BlurFind(this Transform tranRoot, string blurStr) {
	
		List<Transform> tranlist = new List<Transform>();

		Transform[] trans = tranRoot.GetComponentsInChildren<Transform>(true);

		for(int i = 0; i < trans.Length; i++) {
		
			if(trans[i].name.Contains(blurStr))
				tranlist.Add(trans[i]);
		
		}

		return tranlist;
	
	}

	#endregion

}
