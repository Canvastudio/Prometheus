using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyOrgan : OrganBase {

    public OperateConfig config;

    public override void Reactive()
    {
        int count = config.arg4.Count();
        for (int i = 0; i < count; ++i)
        {
            float[] f;
            var value = Rpn.CalculageRPN(config.arg4.ToArray(i), StageCore.Instance.Player,
                null, out f);
            GameProperty property = (GameProperty)(f[0]);
            StageCore.Instance.Player.Property.SetFloatProperty(property, value);
        }
        Clean();
    }               
}
