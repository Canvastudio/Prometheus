using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageRecording {

    public class DeadMonsterRecord
    {
        public int pwr;
        public ulong uid;
        public int lv;
        public Brick brick;
    }


    /// <summary>
    /// 记录最后死亡的怪物
    /// </summary>
    public DeadMonsterRecord lastDeadMonster;

}
