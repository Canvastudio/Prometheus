using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cover : GameItemBase {

    [SerializeField]
    Image image;

    Color color = new Color32(95, 95, 95, 255);

    public override IEnumerator OnDiscoverd()
    {
        //Debug.Log("去除Cover: " + gameObject.name);
        Recycle();
        base.OnDiscoverd();

        return null;
    }

    public override void Recycle()
    {
        base.Recycle();
        standBrick.cover = null;
        standBrick = null;
        image.color = ArtColor.Instance.coverNormal;
        color = ArtColor.Instance.coverNormal;

        ObjPool<Cover>.Instance.RecycleObj(GameItemFactory.Instance.cover_pool, itemId);
    }

    public void SetLight()
    {
        color = ArtColor.Instance.coverLight;
    }

    public void SetDark()
    {
        color = ArtColor.Instance.coverNormal;
    }

    void Update()
    {

        if (Mathf.Abs(image.color.r - color.r) < 0.05)
        {
            image.color = color;
        }
        else
        {
            image.color = Color.Lerp(image.color, color, Time.deltaTime * 5);
        }

        if (GameTestData.Instance.alwaysShow)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.3f);
        }

    }
}
