using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCamera : MonoBehaviour {

    public void Awake()
    {
        Messenger.AddListener(SA.GameStart, OnGameStart);
        Messenger.AddListener(SA.GameEnd, OnGameEnd);
    }

    private void OnGameStart()
    {

    }

    private void OnGameEnd()
    {

    }

    private void LateUpdate()
    {
        if (StageCore.Instance.gaming)
        {

        }
    }
}
