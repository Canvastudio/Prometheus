using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtTestYeild : MonoBehaviour {

	public bool isture = false;

	// Use this for initialization
	void Start () {

		StartCoroutine(Done());
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public IEnumerator Done() {
	
		while(true) {
			
			if(isture) {

				Debug.Log("while  ..." + Time.deltaTime);
				yield break;

			}else{
				Debug.Log("whild done!" + Time.deltaTime);
				yield return null;
			}

		}

		Debug.Log("Whild out:" + Time.deltaTime);
		yield return null;

	}

}
