using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArtTran {

	public static void SetPos(this Transform tran, ref Vector3 pos) {

		if (tran != null) {

			pos = tran.position;

		}

	}
}
