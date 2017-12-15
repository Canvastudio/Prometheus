using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rpn {

    public static float CalculageRPN(long[] damage_values, GameItemBase rpn_source, GameItemBase rpn_target, out float[] value,
        ActiveSkillsConfig skillsConfig = null, float skillDamage = 0, float passTime = 0, float moveBrick = 0)
    {
        Stack<float> stack = new Stack<float>();

        value = new float[2];
        float[] fv = new float[2];

        SuperTool.GetValue(damage_values[damage_values.Length - 1], ref fv);

        value[0] = fv[0];
        value[1] = fv[1];

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
                    stack.Push(GContext.Instance.discover_monster);

                    //if (rpn_source is Player)
                    //{
                    //    stack.Push(GContext.Instance.discover_monster - GContext.Instance.enslave_count);
                    //}
                    //else
                    //{
                    //    stack.Push(1 + GContext.Instance.enslave_count);
                    //}
                }
                else if (property == GameProperty.openGridNum)
                {
                    stack.Push(GContext.Instance.discover_brick);
                }
                else if (property == GameProperty.darkGridNum)
                {
                    stack.Push(GContext.Instance.dark_brick);
                }
                else if (property == GameProperty.passTime)
                {
                    stack.Push(passTime);
                }
                else if (property == GameProperty.dis)
                {
                    stack.Push(rpn_source.standBrick.pathNode.Distance(rpn_target.standBrick.pathNode));
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

                    try
                    {
                        stack.Push(target.Property.GetFloatProperty(property));
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError("RPn exceptiong: " + e);
                    }
                }
            }
        }

        Debug.Log("逆波兰没有出口.");

        return 0;
    }

    public static float GetMoveTime()
    {
        var rpn = GlobalParameterConfig.GetConfigDataById<GlobalParameterConfig>(1).motorizedFormula.ToArray();

        float[] f;

        float time = CalculageRPN(rpn, StageCore.Instance.Player, null, out f);

        //float rate = GlobalParameterConfig.GetConfigDataById<GlobalParameterConfig>(1).timeRate;

        return time;// * rate;

    }
}
