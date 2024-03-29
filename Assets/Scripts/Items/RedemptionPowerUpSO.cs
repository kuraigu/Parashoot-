using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RedemptionPowerUp", menuName = "Items/Power Ups/Redemption")]
public class RedemptionPowerUpSO : PowerUpItemSO
{
    public override bool UsePowerUp()
    {
        base.UsePowerUp();

        if(RecognizerManager.instance != null)
        {
            RecognizerManager.instance.EnableRecognition();
            RecognizerManager.instance.ClearWrongGesturesContainer();

            return true;
        }

        return false;
    }
}
