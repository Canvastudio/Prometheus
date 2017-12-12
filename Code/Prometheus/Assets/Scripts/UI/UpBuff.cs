using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpBuff : MonoBehaviour {

    [SerializeField]
    Image image;
    public StateIns ins;
    public ulong id;

    public void Init(StateIns _ins, ulong _id)
    {
        ins = _ins;
        image.sprite = StageView.Instance.stateAtlas.GetSprite(ins.stateConfig.icon);
        id = _id;
    }
}
