using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtPop : MonoBehaviour {

	public List<Sprite> spriteList;
	public Image Image_state;
	public int index = -1;
	public Animator anim;

	// Use this for initialization
	void Start () {
		anim.SetBool("isloop", spriteList.Count > 1);
	}

	public void OnNext() {
	
		//check end
		if (spriteList.Count <= 0) {
		
			OnEnd();
			return;
		
		}

		index++;
		index = index >= spriteList.Count ? 0 : index;
		Image_state.sprite = spriteList[index];
	
	}

	public void Add(List<Sprite> slist) {
	
		for (int i = 0; i < slist.Count; i++)
			Add(slist[i]);
	
	}

	public void Add(Sprite sprite) {
	
		spriteList.Add(sprite);

		anim.SetBool("isloop", spriteList.Count > 1);
	
	}

	public void Remove(Sprite sprite) {
	
		if (spriteList.Contains(sprite)) {
		
			spriteList.Remove(sprite);
			anim.SetBool("isloop", spriteList.Count > 1);
		
		}
	
	}

	public void OnEnd() {

		index = -1;

	}

}
