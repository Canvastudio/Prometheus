using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SuperUI
{
    /// <summary>
    /// 可以向下伸缩的布局模式，注意子部件的Ancher Presets要设置为center bottom
    /// </summary>
    public class SuperLayoutAutoGrow : MonoBehaviour
    {

        public Vector2 offset;

        private RectTransform rectTransform;
        private List<RectTransform> objectList;
        private List<Vector2> sizeList;
        private int objNum;

        void Awake()
        {
            objectList = new List<RectTransform>();
            sizeList=new List<Vector2>();
            rectTransform = GetComponent<RectTransform>();
            Vector2 pov = new Vector2();
            pov.x = 0.5f;
            pov.y = 1;
            rectTransform.pivot = pov;
        }

        // Update is called once per frame
        void Update()
        {
            objNum = transform.childCount;
            List<RectTransform> tempRectList = new List<RectTransform>();
            List<Vector2> tempSizeList=new List<Vector2>();
            for (int i = 0; i < objNum; i++)
            {
                RectTransform rtf = transform.GetChild(i).GetComponent<RectTransform>();
                tempRectList.Add(rtf);
                tempSizeList.Add(rtf.sizeDelta);
            }

            bool needSort = false;
            if (tempRectList.Count == objectList.Count)
            {
                for (int i = 0; i < objectList.Count; i++)
                {
                    if (!tempRectList.Contains(objectList[i]))
                    {
                        needSort = true;
                        break;
                    }
                }
            }
            else
            {
                needSort = true;
            }

            bool needSetPos = false;
            if (needSort) needSetPos = true;
            else
            {
                if (tempSizeList.Count != sizeList.Count)
                {
                    needSetPos = true;
                }
                else
                {
                    for (int i = 0; i < sizeList.Count; i++)
                    {
                        if (tempSizeList[i] != sizeList[i])
                        {
                            needSetPos = true;
                            break;
                        }
                    }
                }
            }

            if (needSort) Sort(tempRectList);
            if (needSetPos) SetPos(tempRectList);
            objectList = tempRectList;
            sizeList = tempSizeList;
        }

        private void Sort(List<RectTransform> objs)
        {
            //print("排序被执行");
            bool hasSawp = true;
            int begin = 0;

            for (int i = 0; i < objs.Count && hasSawp; i++)
            {
                hasSawp = false;
                for (int j = begin; j < objs.Count - 1 - i; j++)
                {
                    ISupperLayoutComparer comp = objs[j].GetComponent<ISupperLayoutComparer>();
                    if (comp == null)
                    {
                        hasSawp = true;
                        begin++;
                        break;
                    }
                    if (comp.SortCompear(objs[j + 1].gameObject))
                    {
                        global::SuperTool.Swap(objs, j, j + 1);
                        hasSawp = true;
                    }
                }
            }
        }

        private void SetPos(List<RectTransform> objs)
        {
            Vector2 temp = rectTransform.sizeDelta;
            temp.y = 0;
            rectTransform.sizeDelta = temp;
            for (int i = objs.Count - 1; i > -1; i--)
            {
                temp = rectTransform.sizeDelta;
                temp.y += objs[i].sizeDelta.y;
                rectTransform.sizeDelta = temp;

                objs[i].pivot = new Vector2(0.5f, 1);
                Vector2 pos = objs[i].anchoredPosition;
                pos.x = offset.x;
                pos.y = temp.y + offset.y;
                objs[i].anchoredPosition = pos;
            }

        }
    }
}
