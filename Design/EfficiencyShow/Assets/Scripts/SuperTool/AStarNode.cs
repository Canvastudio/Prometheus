using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

#region A*算法
/// <summary>
/// A*管理器管理的节点对象，需要注意它自己并不会主动构建节点之间的连接关系
/// </summary>
public class AStarNode : MonoBehaviour
{

    void Awake()
    {
        linkNode = new List<AStarNode>();
        MessageCenter.Instance.AddListener("MSG_AStar_Clear", ClearHG);
    }

    void OnDestroy()
    {
        MessageCenter.Instance.RemoveListener("MSG_AStar_Clear", ClearHG);
    }

    private void ClearHG(object o)
    {
        G = H = 0;
        fatherAStarNode = null;
    }

    /// <summary>
    /// 该节点连接的其他节点
    /// </summary>
    public List<AStarNode> linkNode;

    /// <summary>
    /// 该节点的父节点
    /// </summary>
    [HideInInspector]
    public AStarNode fatherAStarNode;

    /// <summary>
    /// 阻碍等级，通常为0，阻碍等级越高，表明越难通过，如果为负，表示不可通过
    /// </summary>
    public int blockLevel = 0;

    /// <summary>
    /// 到终点的预估值
    /// </summary>
    [HideInInspector]
    public int H;

    /// <summary>
    /// 到相邻点的预估值
    /// </summary>
    [HideInInspector]
    public int G;

    /// <summary>
    /// 综合预估值
    /// </summary>
    public int F
    {
        get { return H + G + blockLevel; }
    }

}

public class AStarManager
{
#if UNITY_EDITOR
    [MenuItem("SuperTool/AddAStarNode &%o")]
    static void AddNode(MenuCommand cmd)
    {
        foreach (var obj in Selection.gameObjects)
        {
            if (obj.GetComponent<AStarNode>() == null)
                Undo.AddComponent<AStarNode>(obj);
        }
    }
#endif


    private static AStarManager _instance;
    public static AStarManager Instance
    {
        get { return _instance ?? (_instance = new AStarManager()); }
    }

    private AStarManager()
    {
        closeList = new List<AStarNode>();
        openList = new List<AStarNode>();
    }

    private readonly List<AStarNode> closeList;//已经走过的节点，如果一个格子全都是走过的格子，又没有走到终点，那表示不存在路径
    private readonly List<AStarNode> openList;

    public delegate int EvaluateHandle(AStarNode a, AStarNode b);

    /// <summary>
    /// 终点距离评估函数，H是一个忽略阻碍的预估值，决定了优先探索方向
    /// </summary>
    public EvaluateHandle EvaluateH { set; private get; }

    /// <summary>
    /// 邻接点距离评估函数，如果返回负数，表示不可达
    /// 比如邻接点如果是直线评估返回10，那么斜向就可以返回14
    /// </summary>
    public EvaluateHandle EvaluateG { set; private get; }

    /// <summary>
    /// 计算并获取路径
    /// </summary>
    public List<AStarNode> GetPath(AStarNode start, AStarNode end)
    {
        if (CreatePath(start, end))
        {
            List<AStarNode> path = new List<AStarNode>();
            AStarNode temp = end;
            int check = 0;
            while (temp.fatherAStarNode != null)
            {
                if (check++ > 10000) throw new Exception("寻路发生死循环");
                path.Add(temp);
                temp = temp.fatherAStarNode;
            }
            path.Add(start);
            path.Reverse();
            return path;
        }
        return null;
    }

    /// <summary>
    /// 如果返回false表示不存在路径
    /// </summary>
    private bool CreatePath(AStarNode startAStarNode, AStarNode endAStarNode)
    {
        if (EvaluateH == null || EvaluateG == null) throw new ArgumentException("必须先设定终点评估与邻接评估两个函数");
        openList.Clear();
        closeList.Clear();
        MessageCenter.Instance.PostMsg("MSG_AStar_Clear");
        openList.Add(startAStarNode);
        while (openList.Count > 0)
        {
            AStarNode center = GetNode();
            if (center == null) continue;
            if (center == endAStarNode) return true;
            foreach (var near in center.linkNode.Where(near => !closeList.Contains(near)))
            {
                if (near == null) continue;
                if (openList.Contains(near))
                {
                    int i_temp = EvaluateG_Use(center, near);
                    if (i_temp < near.G)
                    {
                        near.G = i_temp;
                        near.fatherAStarNode = center;
                        Sort();
                    }
                }
                else
                {
                    near.H = EvaluateH(near, endAStarNode);
                    near.G = EvaluateG_Use(center, near);
                    if (near.G > 0)
                    {
                        near.fatherAStarNode = center;
                        AddNode(near);
                    }
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 往openList添加节点，按F升序排列
    /// </summary>
    private void AddNode(AStarNode aStarNode)
    {
        for (int i = 0; i < openList.Count; i++)
        {
            if (aStarNode.F < openList[i].F)
            {
                openList.Insert(i, aStarNode);
                return;
            }
        }
        openList.Add(aStarNode);
    }

    /// <summary>
    /// 获取最小F值Node
    /// </summary>
    private AStarNode GetNode()
    {
        AStarNode t = openList[0];
        openList.RemoveAt(0);
        if (t != null) closeList.Add(t);
        return t;
    }

    /// <summary>
    /// 对openList按照F升序排序
    /// </summary>
    private void Sort()
    {
        openList.Sort((node1, node2) =>
        {
            if (node1.F > node2.F) return 1;
            if (node1.F < node2.F) return -1;
            return 0;
        });
    }

    private int EvaluateG_Use(AStarNode center, AStarNode near)
    {
        if (near.blockLevel < 0) return -1;
        return center.G + near.blockLevel + EvaluateG(center, near);
    }
}


#endregion
