using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState {

    string name
    {
        get;
    }

    IEnumerator DoState();
    IState GetNextState();
    IEnumerator StopState();
}
