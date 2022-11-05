using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectable
{
    public override void Collect()
    {
        // TODO Implement score pickup code
        Debug.Log("Collected " + gameObject.name);
        GameManager.GameData.CurrentScore += 1;
    }

}
