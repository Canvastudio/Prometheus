using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AtkRange : SingleGameObject<AtkRange> {

    [SerializeField]
    Image rangeMask;
    [SerializeField]
    AtkRangeEffect effectPrefab;

    public string strRangeMask = "RangeMask";
    public string strRangeEffect = "RangeEffect";
    /// <summary>
    /// 每一个mask从透明到完成的时间
    /// </summary>
    public float gradualTime = 2f;
    public float maskAlpha = 0.5f;

    protected override void Init()
    {
        base.Init();

        ObjPool<Image>.Instance.InitOrRecyclePool(strRangeMask, rangeMask, 20);
        ObjPool<AtkRangeEffect>.Instance.InitOrRecyclePool(strRangeEffect, effectPrefab, 20);
    }

    public AtkRangeEffect ShowAtkRangeEffect(int range, Brick brick)
    {
        ulong id;
        var effect = ObjPool<AtkRangeEffect>.Instance.GetObjFromPoolWithID(out id, strRangeEffect);
        effect.SetParentAndNormalize(StageView.Instance.range);
        effect.gameObject.SetActive(true);
        effect.Show(range, brick);
        effect.id = id;
        return effect;
    }

    public void RecycEffect(ulong id)
    {
        ObjPool<AtkRangeEffect>.Instance.RecycleObj(strRangeEffect, id);
    }
}
