using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;  

public class UpUIView : MonoBehaviour {

    [SerializeField]
    Image icon;
    [SerializeField]
    Text hpText;
    [SerializeField]
    Slider hpSlider;
    [SerializeField]
    UpBuff buff;
    [SerializeField]
    Button roleButton;
    StringBuilder sb = new StringBuilder(10);

    List<UpBuff> list = new List<UpBuff>();

    string pname = "upbuff";
    public void Init()
    {
        ObjPool<UpBuff>.Instance.InitOrRecyclePool(pname, buff);
        HudEvent.Get(roleButton).onClick = OnRole;
        Messenger.AddListener(SA.PlayHpChange, OnHpChange);

        OnHpChange();
    }

    private void OnRole()
    {
        StartCoroutine(MuiCore.Instance.AddOpen(UiName.strRoleInfoView));
    }

    public void OnStateAdd(StateIns state)
    {
        int id;
        var ub = ObjPool<UpBuff>.Instance.GetObjFromPoolWithID(out id, pname);
        ub.Init(state, id);
        ub.SetParentAndNormalize(this.transform);
        list.Add(ub);
        ub.transform.localPosition = new Vector3(buff.transform.localPosition.x + 60 * (list.Count - 1), buff.transform.localPosition.y, buff.transform.localPosition.z);
    }

    public void OnStateRemove(StateIns state)
    {

    }

    private void OnHpChange()
    {
        sb.Remove(0, sb.Length);
        int curHp = Mathf.RoundToInt(StageCore.Instance.Player.cur_hp);
        int maxHp = Mathf.RoundToInt(StageCore.Instance.Player.fmax_hp);

        sb.Append(curHp.ToString());
        sb.Append("/");
        sb.Append(maxHp.ToString());

        hpText.text = sb.ToString();

        hpSlider.value = StageCore.Instance.Player.cur_hp / StageCore.Instance.Player.fmax_hp;
    }
}
