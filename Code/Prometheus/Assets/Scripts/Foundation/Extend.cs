using UnityEngine;
using System.Collections.Generic;

public class Map<T, K> {

	private Dictionary<T, K> maps = new Dictionary<T, K>();

	public void Bind(T t, K k) {
	
		maps.Add (t, k);
	
	}

	public void UnBind(T t) {
	
		maps.Remove (t);
	
	}

	public void Clear() {
	
		maps.Clear ();
	
	}

	public K Get(T t) {

		K k;

		if (maps.TryGetValue (t, out k)) {
		
			return k;
		
		}

		return default(K);
	
	}

}