using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class BrickNameData : ScriptableObject
{
    //public int scene_id = 0;
    //public string unexplore_brick = "box_d_0";
    //public string[] explore_bricks = new string[] { "box_l_0", "box_l_1", "box_l_2", "box_l_3", "box_l_5", "box_l_6", };
    //public string side_brick = "box_s_4";
    //public string obstacle = "box_d_0";
    //public string block = "box_f_0";
    public int scene_id = 0;
    public string unexplore_brick;
    public string[] explore_bricks;
    public string side_brick;
    public string obstacle;
    public string block;
}

public class BrickNameCore : SingleGameObject<BrickNameCore> {

    [SerializeField]
    List<BrickNameData> list;

    BrickNameData _curData = null;
    BrickNameData curData
    {
        get {
            if (_curData == null)
            {
                for (int i = 0; i < list.Count; ++i)
                {
                    if (list[i].scene_id == scene_id)
                    {
                        _curData = list[i];
                    }
                }
            }

            return _curData;
        }
        set
        {
            _curData = value;
        }
    }
    int scene_id = -1;

    public void SetCurrentScene(int id)
    {
        scene_id = 0;
    }

    public string GetUnExploredBrickName()
    {
        return curData.unexplore_brick;
    }

    public string GetExploredBrickName()
    {
        int i = Random.Range(0, curData.explore_bricks.Length);
        return curData.explore_bricks[i];
    }

    public string GetSideBrickName()
    {
        return curData.side_brick;
    }

    public string GetObstacleName()
    {
        return curData.obstacle;
    }

    public string GetBlockName()
    {
        return curData.block;
    }
}
