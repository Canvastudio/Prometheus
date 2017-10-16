using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipCore : SingleObject<ChipCore> {

    public ChipInventory AssembleChip(ChipInventory a, ChipInventory b)
    {
        return new ChipInventory(1);
    }
}
