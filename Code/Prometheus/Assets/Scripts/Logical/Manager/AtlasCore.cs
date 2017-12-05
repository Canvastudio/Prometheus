using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class AtlasCore : SingleGameObject<AtlasCore> {

    [SerializeField]
    List<SpriteAtlas> preloadAtlas;

    Dictionary<string, SpriteAtlas> atlasData = new Dictionary<string, SpriteAtlas>();

    protected override void Init()
    {
        base.Init();

        foreach(var at in preloadAtlas)
        {
            atlasData.Add(at.tag, at);
        }
    }

    public SpriteAtlas Load(string name)
    {
        SpriteAtlas atlas = null;

        if (atlasData.TryGetValue(name, out atlas))
        {
            return atlas;
        }
        else
        {
            var newAtlas = Resources.Load<SpriteAtlas>("Atlas/" + name);
            atlasData.Add(name, newAtlas);
            return newAtlas;
        }
    }
}
