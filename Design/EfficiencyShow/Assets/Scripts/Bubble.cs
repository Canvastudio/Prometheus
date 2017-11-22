using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : TempObj
{

    public SpriteRenderer renderer;
    public Sprite[] states;

    private List<Sprite> _states=new List<Sprite>();

    // Use this for initialization
    void Start()
    {
        ChangeState();
    }

    public void ChangeState()
    {
        if(_states.Count==0) _states=new List<Sprite>(states);
        renderer.sprite = SuperTool.Pop(_states);
    }
}
