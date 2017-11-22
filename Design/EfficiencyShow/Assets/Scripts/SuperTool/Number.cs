using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

#region 数字
/// <summary>
/// 美术数字
/// </summary>
public class Number : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("SuperTool/NumberComponent &%l")]
    static void AddNumber(MenuCommand cmd)
    {
        foreach (var obj in Selection.gameObjects)
        {
            if (obj.GetComponent<Number>() == null)
                Undo.AddComponent<Number>(obj);
        }
    }
#endif

    public Sprite[] numbers = new Sprite[10];
    public NumberJustify numberJustify;
    public int sortingOrder;
    private List<GameObject> numbersGameObject;
    private Color color = Color.white;

    public Color Color
    {
        set
        {
            color = value;
            if (numbersGameObject == null || numbersGameObject.Count == 0) return;
            foreach (GameObject nums in numbersGameObject)
                nums.GetComponent<SpriteRenderer>().color = color;
        }
        get { return color; }
    }

    public float offset = 0;
    private int oldValue = int.MinValue;

    // Use this for initialization
    void Awake()
    {
        if (numbers.Length != 10)
        {
            Debug.Log("必须传入10个数字");
            throw new Exception();
        }
        numbersGameObject = new List<GameObject>();

    }


    public void SetValue(int value)
    {
        if (value < 0) value = 0;
        if (oldValue == value) return;
        oldValue = value;
        numbersGameObject.ForEach(GameObject.Destroy);
        numbersGameObject.Clear();
        string numStr = value.ToString();

        for (int i = 0; i < numStr.Length; i++)
        {
            string tempStr;
            if (numberJustify == NumberJustify.Left || numberJustify == NumberJustify.Middle)
                tempStr = numStr[i].ToString();
            else
                tempStr = numStr[numStr.Length - i - 1].ToString();
            GameObject temp = new GameObject(tempStr);
            temp.transform.SetParent(transform);
            temp.transform.localScale = Vector3.one;
            SpriteRenderer spriteRenderer = temp.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = sortingOrder;
            spriteRenderer.sprite = numbers[int.Parse(tempStr)];
            spriteRenderer.color = color;
            Vector3 pos = new Vector3();
            if (i == 0)
            {
                pos = Vector3.zero;
                if (numberJustify == NumberJustify.Middle)
                {
                    float len = 0;
                    for (int j = 1; j < numStr.Length; j++)
                    {
                        int index = int.Parse(numStr[j].ToString());
                        len += numbers[index].bounds.size.x;
                    }
                    len += offset * (numStr.Length - 1);
                    pos.x = -len / 2;
                }
            }
            else
            {
                pos = numbersGameObject[i - 1].transform.localPosition;
                float offsetX = (numbers[i].bounds.size.x + numbers[i - 1].bounds.size.x) / 2 + offset;
                if (numberJustify == NumberJustify.Left || numberJustify == NumberJustify.Middle)
                    pos.x += offsetX;
                else
                    pos.x -= offsetX;
            }
            temp.transform.localPosition = pos;
            numbersGameObject.Add(temp);
        }
    }

}

public enum NumberJustify
{
    Left,
    Right,
    Middle
}


#endregion