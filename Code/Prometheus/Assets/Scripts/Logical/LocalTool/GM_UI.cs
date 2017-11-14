using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GM_UI : MonoBehaviour {

    [SerializeField]
    Button OpenChipUpdate_Btn;
    
	// Use this for initialization
	void Start () {
        HudEvent.Get(OpenChipUpdate_Btn.gameObject).onClick = () =>
        {
            MuiCore.Instance.Open(UiName.strChipUpdateView);
        };
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
