using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rpn {

    public static float CalculageRPN(long[] damage_values, GameItemBase rpn_source, GameItemBase rpn_target, out GameProperty valueType, 
        ActiveSkillsConfig skillsConfig = null, float skillDamage = 0)
    {
        Stack<float> stack = new Stack<float>();

        float[] fv = new float[2];

        SuperTool.GetValue(damage_values[damage_values.Length - 1], ref fv);

        valueType = (GameProperty)fv[0];

        for (int i = 0; i < damage_values.Length - 1; ++i)
        {
            SuperTool.GetValue(damage_values[i], ref fv);


            var property = (GameProperty)fv[0];

            if (fv[1] == 0)
            {
                stack.Push(fv[0]);
            }
            else
            {
                if (property == GameProperty.Eql)
                {
                    if (stack.Count != 0)
                    {
                        return stack.Pop();
                    }
                }
                else if (property == GameProperty.Plus)
                {
                    if (stack.Count < 2)
                    {
                        Debug.Log("逆波兰遇到操作符号的时候stack长度小于2");
                    }
                    float v1 = stack.Pop();
                    float v2 = stack.Pop();

                    stack.Push(v1 + v2);

                }
                else if (property == GameProperty.Sub)
                {
                    if (stack.Count < 2)
                    {
                        Debug.Log("逆波兰遇到操作符号的时候stack长度小于2");
                    }
                    float v1 = stack.Pop();
                    float v2 = stack.Pop();

                    stack.Push(v2 - v1);

                }
                else if (property == GameProperty.Mul)
                {
                    if (stack.Count < 2)
                    {
                        Debug.Log("逆波兰遇到操作符号的时候stack长度小于2");
                    }
                    float v1 = stack.Pop();
                    float v2 = stack.Pop();

                    stack.Push(v1 * v2);

                }
                else if (property == GameProperty.Div)
                {
                    if (stack.Count < 2)
                    {
                        Debug.Log("逆波兰遇到操作符号的时候stack长度小于2");
                    }
                    float v1 = stack.Pop();
                    float v2 = stack.Pop();

                    stack.Push(v2 / v1);

                }
                else if (property == GameProperty.skillTime)
                {
                    stack.Push(skillsConfig.costTime);
                }
                else if (property == GameProperty.skillDamage)
                {
                    stack.Push(skillDamage);
                }
                else if (property == GameProperty.monsterNum)
                {
                    stack.Push(GContext.Instance.discover_monster - GContext.Instance.enslave_count);
                }
                else if (property == GameProperty.openGridNum)
                {
                    stack.Push(GContext.Instance.discover_brick);
                }
                else if (property == GameProperty.darkGridNum)
                {
                    stack.Push(GContext.Instance.dark_brick);
                }
                else
                {
                    LiveItem target = null;

                    if (fv[1] == 1)
                    {
                        target = rpn_source as LiveItem;
                    }
                    else
                    {
                        target = rpn_target as LiveItem;
                    }

                    stack.Push(target.Property.GetFloatProperty(property));
                }
            }
        }

        Debug.Log("逆波兰没有出口.");

        return 0;
    }
}
