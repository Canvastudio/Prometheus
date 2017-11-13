using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IManagedUI  {

    IEnumerator Init(object param);
    IEnumerator Open(object param);
    IEnumerator Close(object param);
    IEnumerator Hide(object param);
}
