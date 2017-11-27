﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtPop : MonoBehaviour {

	public List<Sprite> spriteList;
	public Image Image_state;
	public int index = -1;
	public Animator anim;
    private Sprite cur_sprite;
	// Use this for initialization
	void Awake () {
		anim.SetBool("isloop", spriteList.Count > 1);
        gameObject.SetActive(false);
	}

	public void OnNext() {
	
		//check end
		if (spriteList.Count <= 0) {
		
			OnEnd();
			return;
		}

        if (spriteList.Count == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            index++;
            index = index >= spriteList.Count ? 0 : index;
            Image_state.sprite = spriteList[index];
            cur_sprite = spriteList[index];
        }
	}

	public void Add(List<Sprite> slist) {
	
		for (int i = 0; i < slist.Count; i++)
			Add(slist[i]);
	
	}

	public void Add(Sprite sprite) {

        if (!spriteList.Contains(sprite))
        {
            spriteList.Add(sprite);

            anim.SetBool("isloop", spriteList.Count > 1);
        }

        gameObject.SetActive(true);
    }

	public void Remove(Sprite sprite) {
	
		if (spriteList.Contains(sprite)) {
		
			spriteList.Remove(sprite);
			anim.SetBool("isloop", spriteList.Count > 1);

            if (cur_sprite == sprite)
            {
                OnNext();
            }

        }
	
	}

	public void OnEnd() {

		index = -1;

	}

}
