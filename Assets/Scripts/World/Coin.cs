using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectable
{
    public override void Collect()
    {
        // gm.OnPlayerPickUpCoin?.Invoke();
        GameManager.GameData.CurrentScore += 1;

    }

}
