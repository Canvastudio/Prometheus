using UnityEngine.UI;
using UnityEngine;

public class BoardSupplyInstance : BoardInstanceBase
{
    [SerializeField]
    public Text remainingPower;
    
    public void Init(float _powerSupply)
    {
        powerSupply = _powerSupply * (1f + StageCore.Instance.Player.Property.GetFloatProperty(GameProperty.capacity) / 100f);
        remainingPower.text = powerSupply.ToString();
    }

    public void Update()
    {
        if (powerGrid != null && powerGrid.remainingDirty)
        {
            remainingPower.text = (powerGrid.powerGridTotalPower - powerGrid.powerGridCastPower).ToString();
        }
    }
}


