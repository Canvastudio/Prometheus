using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtPop : MonoBehaviour {

	public List<StateIns> StateList;
	public Image Image_state;
	public int index = -1;
	public Animator anim;
    private StateIns cur_ins;
	// Use this for initialization
	void Awake () {
		anim.SetBool("isloop", StateList.Count > 1);
        gameObject.SetActive(false);
	}

	public void OnNext() {
	
		//check end
		if (StateList.Count <= 0) {
		
			OnEnd();
			return;
		}

        gameObject.SetActive(true);
        index++;
        index = index >= StateList.Count ? 0 : index;
        Image_state.SetStateIcon(StateList[index].stateConfig.icon);
        cur_ins = StateList[index];
        
	}

	//public void Add(List<Sprite> slist) {
	
	//	for (int i = 0; i < slist.Count; i++)
	//		Add(slist[i]);
	
	//}

	public void Add(StateIns ins) {

        StateList.Add(ins);

        anim.SetBool("isloop", StateList.Count > 1);

        gameObject.SetActive(true);
    }

	public void Remove(StateIns ins) {
	
        for (int i = StateList.Count - 1; i >= 0; ++i)
        {
            if (ins.id == StateList[i].id)
            {
                StateList.RemoveAt(i);

                anim.SetBool("isloop", StateList.Count > 1);

                if (cur_ins != null && cur_ins.id == ins.id)
                {
                    OnNext();
                }

                break;
            }
        }


	
	}

	public void OnEnd() {

        gameObject.SetActive(false);
        index = -1;

	}

}
