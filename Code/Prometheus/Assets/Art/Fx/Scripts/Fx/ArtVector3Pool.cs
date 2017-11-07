using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtVector3Pool {

	public static List<List<Vector3>> pool = null;

	private static int count = 1000;
	private static int each = 10;

	public static int index = -1;

	public static void Init() {
	
		if (pool == null) {
		
			pool = new List<List<Vector3>>(count);

			for (int i = 0; i < count; i++) {
			
				pool.Add(new List<Vector3>(each));
			
			}
		
		}
	
	}

	public static List<Vector3> Get() {
	
		if(pool == null) {
			Init();
		}

		index++;

		if (index >= count)
			index = 0;

		return pool[index];
	
	}

}
