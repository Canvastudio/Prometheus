using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtMath {

	public static Quaternion LookAtZ(Vector3 dir) {

		return Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, Vector3.forward);
	
	}

	public static float angle_360(Vector3 _from, Vector3 _to){
	
		_to = _from - _to;
		_from = Vector3.right;

		Vector3 v3 = Vector3.Cross(_from, _to);

		float angle = Vector3.Angle(_from, _to);

		return v3.z > 0 ? angle : 360 - angle;
	
	}

	public static float PowerOfN(float base_numer, int exponent) {
	
		float final_num = 1;
		for (int i = 0; i < exponent; i++)
			final_num *= base_numer;

		return base_numer == 0 ? 0 : final_num;
	
	}

	private static List<float> factorials = new List<float>{
		1, 1, 2, 6, 24, 120, 720, 5040
	};

	private static List<float> bezier3RatioList = null;

	public static List<float> Bezier3RatioList {
	
		get {
		
			if (bezier3RatioList == null) {

				int n = 3;
				bezier3RatioList = new List<float>(n);

				for (int k = 0; k <= n; k++) {
					bezier3RatioList.Add(factorials[n] / (factorials[k] * factorials[n - k]));
					Debug.Log(bezier3RatioList[k]);
				}
			
			}

			return bezier3RatioList;
		
		}
	
	}

	private static Vector3 RndBezierPos(Vector3 orign) {

		return orign + new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0);

	}

	private static List<Vector3> vecList = new List<Vector3>{Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero};

	public static List<Vector3> Bezier3Pos(Vector3 start, Vector3 end) {
	
		vecList[0] = start;
		vecList[1] = RndBezierPos(start);
		vecList[2] = RndBezierPos(end);
		vecList[3] = end;

		return vecList;
	
	}

	public static float DistanceList(List<Vector3> plist) {
	
		float len = 0;

		for (int i = 0; i < plist.Count - 1; i++)
			len += Vector3.Distance(plist[i], plist[i + 1]);

		return len;
	
	}

}
