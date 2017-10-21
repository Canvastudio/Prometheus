using UnityEngine.UI;
using UnityEngine;

public class BoardSupplyInstance : BoardInstanceBase
{
    [SerializeField]
    public Text remainingPower;
    
    public void Init(float _powerSupply)
    {
        powerSupply = _powerSupply;
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


