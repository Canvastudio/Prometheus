using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GM_UI : MonoBehaviour {

    [SerializeField]
    Button OpenChipBoard_Btn;
    
	// Use this for initialization
	void Start () {
        HudEvent.Get(OpenChipBoard_Btn.gameObject).onClick = () =>
        {
            ChipBoard.Instance.OpenChipBoard();
        };
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
