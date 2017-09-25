using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStage : IState
{
    public string name
    {
        get
        {
            return Predefine.GAME_STAGE;
        }
    }

    public IEnumerator DoState()
    {
        BrickCore.Instance.CreatePrimitiveStage();
        BrickCore.Instance.CreatePlayer();

        return null;
    }

    public IState GetNextState()
    {
        return null;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
