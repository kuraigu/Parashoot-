using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpItemSO : ItemSO
{
    public virtual bool UsePowerUp()
    {
        return false;
    }

    public override bool UseItem()
    {
        return false;
    }
}
