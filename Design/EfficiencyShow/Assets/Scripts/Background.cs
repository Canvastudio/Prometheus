using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : SingleGameObject<Background>
{
    private List<GameObject> grids;
    private List<GameObject> _grids;

    protected override void Init()
    {
        var spriteRenderers = SuperTool.GetComponentsInChildren<SpriteRenderer>(this);
        grids=new List<GameObject>();
        foreach (var var in spriteRenderers)
        {
            grids.Add(var.gameObject);
        }
        _grids=new List<GameObject>();
        _grids.AddRange(grids);
    }

    public GameObject GetGrids()
    {
        if (_grids.Count == 0) return null;
        return SuperTool.RandomPopElement(_grids);
    }

    public void Reset()
    {
        _grids = new List<GameObject>();
        _grids.AddRange(grids);
    }
}
