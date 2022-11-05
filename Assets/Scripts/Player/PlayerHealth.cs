using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int Lives
    {
        get { return GameManager.GameData.Lives; }
        set { GameManager.GameData.Lives = value; }
    }
    
    private Hazard _hazard;
    public Action OnPlayerDied;
    
    void Start()
    {
        Lives = GameManager.GameData.Lives;
        GameManager.OnPlayerTakeDamage += TakeDamage;
    }

    void TakeDamage()
    {
        Lives = Lives - 1;
        if (Lives == 0)
        {
            OnPlayerDied?.Invoke();
        }
    }
}
