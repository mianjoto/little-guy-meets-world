using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static GameData GameData;

    void Start()
    {
        GameData = new GameData();
    }

    void Update()
    {
        print("Score: " + GameData.CurrentScore);
    }

}
