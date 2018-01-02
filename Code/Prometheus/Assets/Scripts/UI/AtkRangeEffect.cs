using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AtkRangeEffect : MonoBehaviour {

    [SerializeField]
    List<Image> maskList = new List<Image>();
    [SerializeField]
    int currenDistance;
    [SerializeField]
    int radius;
    [SerializeField]
    Brick brick;
    [SerializeField]
    LineRenderer line;

    public ulong id;
    public float br = 0f;

    List<ulong> imageIds = new List<ulong>(12);

    static Vector2[] points = new Vector2[] 
    {
        new Vector2(1, 1),
        new Vector2(1, -1),
        new Vector2(-1, -1),
        new Vector2(-1, 1)
    };

    /// <summary>
    /// 以一个方块为中心显示攻击范围
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="brick"></param>
    public void Show(int radius, Brick brick)
    {
        this.radius = radius;
        this.brick = brick;
        transform.position = brick.transform.position;
        StartCoroutine(ShowEffect());
    }

    public IEnumerator ShowEffect()
    {
        float maxAlpha = AtkRange.Instance.maskAlpha;
        float gradualTime = AtkRange.Instance.gradualTime;

        currenDistance = 1;

        for (;currenDistance <= radius; ++currenDistance)
        {
            float alpha = 0;

            var targets = BrickCore.Instance.GetBrickInDistance(brick.row, brick.column, currenDistance);

            foreach(var t in targets)
            {
                ulong id;
                Image im = ObjPool<Image>.Instance.GetObjFromPoolWithID(out id, AtkRange.Instance.strRangeMask);
                im.SetParentAndNormalize(transform);
                im.gameObject.SetActive(true);
                im.transform.position = t.transform.position;
                im.color = new Color(1, 1, 1, alpha);
                maskList.Add(im);
                imageIds.Add(id);
            }

            while(alpha < maxAlpha)
            {
                alpha += maxAlpha * (Time.deltaTime / gradualTime);

                foreach (var im in maskList)
                {
                    im.color = new Color(1, 1, 1, alpha);
                }

                yield return 0;
            }

            maskList.Clear();
        }

        int vecNum = 4 + 8 * radius;
        line.positionCount = vecNum;

        StartCoroutine(Showline());
    }

    int x;
    int y;
    public float spread = 0.02f;

    public float wait_time = 1;
    private float temp_wait_time = 0;

    IEnumerator Showline()
    {
        while (true)
        {
            br += spread;

            Vector3 sp = brick.transform.position;

            Vector3 pp = sp + new Vector3(-(radius + 0.5f) * br, 0.5f * br, 0);

            int i = 0;
            int p = 1 + 2 * radius;
            while (i < 4 * p)
            {
                Vector2 d = points[i / p];
                if (i % 2 == 0)
                {
                    if (d.x > 0)
                    {
                        pp = GetRight(pp);
                    }
                    else
                    {
                        pp = GetLeft(pp);
                    }

                    line.SetPosition(i, pp);
                    ++i;
                }
                else
                {
                    if (d.y > 0)
                    {
                        pp = GetUp(pp);
                    }
                    else
                    {
                        pp = GetDown(pp);
                    }
                    line.SetPosition(i, pp);
                    ++i;
                }
            }

            yield return 0;

            if (br > 1.2f)
            {
                br = 1.21f;
                
                temp_wait_time += Time.deltaTime;

                if(temp_wait_time > wait_time) {

                    br = 0;
                    temp_wait_time = 0;
                }

            }
        }
    }

    public void Clean()
    {
        temp_wait_time = 0;
        currenDistance = 1;

        foreach (var i in imageIds)
        {
            ObjPool<Image>.Instance.RecycleObj(AtkRange.Instance.strRangeMask, id);
        }

        StopAllCoroutines();
    }

    private Vector3 GetLeft(Vector3 p)
    {
        return new Vector3(p.x - br, p.y, p.z);
    }
    private Vector3 GetUp(Vector3 p)
    {
        return new Vector3(p.x, p.y + br, p.z);
    }
    private Vector3 GetRight(Vector3 p)
    {
        return new Vector3(p.x + br, p.y, p.z);
    }
    private Vector3 GetDown(Vector3 p)
    {
        return new Vector3(p.x, p.y - br, p.z);
    }
}
