using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StuffItem : MonoBehaviour {

    [SerializeField]
    ulong stuffId;
    [SerializeField]
    Text countText;

    private int _count = 0;
	// Use this for initialization
	void Start () {
        Messenger.AddListener(SA.StuffCountChange, OnStuffCountChange);
	}
	
	// Update is called once per frame
    void OnStuffCountChange()
    {
        var count = StageCore.Instance.Player.inventory.GetStuffCount(stuffId);
        if (_count != count)
        {
            countText.text = count.ToString();
            _count = count;
        }
    }
}
