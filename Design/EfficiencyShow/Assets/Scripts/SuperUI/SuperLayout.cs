using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SuperUI
{
    /// <summary>
    /// 根据自己的子物体数量，按照指定方向自动扩展宽高，注意它会改变容器的尺寸与锚点（povit）
    /// 实现ISuperLayoutSort接口以排序
    /// </summary>
    public class SuperLayout : MonoBehaviour
    {

        /// <summary>
        /// 往哪个方向扩展
        /// 当增长方向向左时，物体的Pivot最好设置为右上角（1,1），为右时设置为左上角（0,1），为上时设置为左下角（0,0），为下时设置为左上角（0,1）
        /// </summary>
        public ExpansionMode expansionMode;

        /// <summary>
        /// 是否实时处理，false的话，只会在容器内容有改变时才处理，如果游戏中不改变容器尺寸，使用false
        /// </summary>
        public bool realTime=false;

        /// <summary>
        /// 每个物体的尺寸
        /// </summary>
        public Vector2 perSize;

        /// <summary>
        /// 物体间的间隔
        /// </summary>
        public Vector2 interval;

        /// <summary>
        /// 物体的坐标偏移
        /// </summary>
        public Vector2 offset;

        private RectTransform rectTransform;
        private int numPerWidth;//一行能放多少个
        private int numPerHeight;//一列能放多少个
        private Vector2 size;//框大小
        private List<Transform> objectList;
        private int objNum;

        void Awake()
        {
            objectList = new List<Transform>();
            rectTransform = GetComponent<RectTransform>();
            size = rectTransform.sizeDelta;
        }


        // Update is called once per frame
        void Update()
        {
            objNum = transform.childCount;
            List<Transform> tempList = new List<Transform>();
            for (int i = 0; i < objNum; i++)
                tempList.Add(transform.GetChild(i));

            bool isSame = true;
            if (tempList.Count == objectList.Count)
            {
                for (int i = 0; i < objectList.Count; i++)
                {
                    if (!tempList.Contains(objectList[i]))
                    {
                        isSame = false;
                        break;
                    }
                }
            }
            else
            {
                isSame = false;
            }
            objectList = tempList;
            //print(isSame);
            if (realTime)
            {
                SetPivot();
                CountNewSize(objNum);
                if (!isSame) Sort(objectList);
                SetPosition(objectList);
            }
            else if(!isSame)
            {
                SetPivot();
                CountNewSize(objNum);
                Sort(objectList);
                SetPosition(objectList);
            }


        }

        private void CountNewSize(int objNum)
        {
            size = rectTransform.sizeDelta;
            Vector2 sizeWithInterval = new Vector2()
            {
                x = perSize.x + interval.x,
                y = perSize.y + interval.y
            };
            if (sizeWithInterval.x <= 0)
                numPerWidth = 1;
            else
            {
                float tempNum = (size.x - offset.x)/sizeWithInterval.x;
                numPerWidth = (int)tempNum;//一行能放几个
                if (tempNum % 1 * sizeWithInterval.x >= perSize.x) numPerWidth++;

                if (numPerWidth <= 0) numPerWidth = 1;
            }

            if (sizeWithInterval.y <= 0)
                numPerHeight = 1;
            else
            {
                float tempNum = (size.y - offset.y) / sizeWithInterval.y;
                numPerHeight = (int) tempNum;//一列能放几个
                if (tempNum % 1 * sizeWithInterval.y >= perSize.y) numPerHeight++;
                if (numPerHeight <= 0) numPerHeight = 1;
            }

            int needColOrRow;
            if (expansionMode == ExpansionMode.Down)
            {
                needColOrRow = (int)Math.Ceiling((float)objNum / numPerWidth);
                size.y = offset.y + (perSize.y + interval.y) * needColOrRow- interval.y;
            }
            else if (expansionMode == ExpansionMode.Right)
            {
                needColOrRow = (int)Math.Ceiling((float)objNum / numPerHeight);
                size.x = offset.x + (perSize.x + interval.x) * needColOrRow- interval.x;
            }
            else if (expansionMode == ExpansionMode.Up)
            {
                needColOrRow = (int)Math.Ceiling((float)objNum / numPerWidth);
                size.y = offset.y + (perSize.y + interval.y) * needColOrRow- interval.y;
            }
            else if (expansionMode == ExpansionMode.Left)
            {
                needColOrRow = (int)Math.Ceiling((float)objNum / numPerHeight);
                size.x = offset.x + (perSize.x + interval.x) * needColOrRow- interval.x;
            }
            rectTransform.sizeDelta = size;
        }

        private void Sort(List<Transform> objs)
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



        private void SetPosition(List<Transform> objs)
        {
            for (int i = 0; i < objs.Count; i++)
            {
                float x = 0, y = 0;
                if (expansionMode == ExpansionMode.Down)
                {
                    x = i % numPerWidth * (perSize.x + interval.x) + offset.x;
                    y = -(int)(i / numPerWidth) * (perSize.y + interval.y) - offset.y;
                }
                else if (expansionMode == ExpansionMode.Right)
                {
                    x = (int)(i / numPerHeight) * (perSize.x + interval.x) + offset.x;
                    y = -i % numPerHeight * (perSize.y + interval.y) - offset.y;
                }
                else if (expansionMode == ExpansionMode.Up)
                {
                    x = i % numPerWidth * (perSize.x + interval.x) + offset.x;
                    y = (int)(i / numPerWidth) * (perSize.y + interval.y) + offset.y;
                }
                else if (expansionMode == ExpansionMode.Left)
                {
                    x = -(int)(i / numPerHeight) * (perSize.x + interval.x) - offset.x;
                    y = -i % numPerHeight * (perSize.y + interval.y) - offset.y;
                }
                objs[i].localPosition = new Vector3(x, y, objs[i].transform.localPosition.z);
            }
        }


        private void SetPivot()
        {
            Vector2 pov = new Vector2();
            if (expansionMode == ExpansionMode.Left)
            {
                pov.x = 1;
                pov.y = 1;
            }
            else if (expansionMode == ExpansionMode.Right)
            {
                pov.x = 0;
                pov.y = 1;
            }
            else if (expansionMode == ExpansionMode.Up)
            {
                pov.x = 0;
                pov.y = 0;
            }
            else if (expansionMode == ExpansionMode.Down)
            {
                pov.x = 0;
                pov.y = 1;
            }
            rectTransform.pivot = pov;
        }
    }



    public enum ExpansionMode
    {
        Left,
        Right,
        Up,
        Down
    }

    /// <summary>
    /// 实现SortCompear方法，返回true的排在后面
    /// </summary>
    public interface ISupperLayoutComparer
    {
        /// <summary>
        /// 布局排序，返回true的排在后面
        /// </summary>
        bool SortCompear(GameObject compearObj);
    }
}