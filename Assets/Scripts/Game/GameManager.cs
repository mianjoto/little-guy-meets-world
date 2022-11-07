using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    #region Singletons
        public static GameManager Instance { get; private set; }
        public static SceneManager SceneManager;
        public static UIManager UIManager;
        public static GameObject Player;
    #endregion

    #region Gameplay Loop
        public static float TotalGameTime = 300f;
        public static float GameTimeRemaining = 300f;
    #endregion
    [HideInInspector]
    public static GameData GameData;

    # region Events
        [SerializeField]
        public static Action OnPlayerTakeDamage;
        public static Action OnPlayerPickUpCoin;
        // [SerializeField]
        // public static Action OnGameOver;
    # endregion

    #region Keybinds
        [HideInInspector]
        public static KeyCode JumpKey = KeyCode.Space;
        [HideInInspector]
        public static KeyCode InteractKey = KeyCode.E;
    #endregion
    
    void Awake()
    {
        GameData = new GameData();

        // GameManager Singleton
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        }

        // SceneManager Singleton
        if (SceneManager != null)
        {
            Destroy(SceneManager);
        }
        else
        {
            SceneManager = gameObject.AddComponent<SceneManager>();
        }

        // UIManager Singleton
        if (UIManager != null)
        {
            Destroy(UIManager);
        }
        else
        {
            UIManager = gameObject.AddComponent<UIManager>();
        }

        Player = SingletonHandler.ReturnGameObjectIfNotInitialized(Player, "Player");
    }

    void Start()
    {
        Player.GetComponent<PlayerHealth>().OnPlayerDied += HandlePlayerDeath;
        GetComponent<UIManager>().OnTimeRanOut += GameOver;
    }

    void GameOver()
    {
        RunGameOverSequence();
    }

    IEnumerator RunGameOverSequence()
    {
        float newOrthoSize = 5f;
        Camera.main.DOOrthoSize(newOrthoSize, 5f);
        yield return new WaitForSeconds(5f);
        UIManager.DisplayGameOver();
        yield return new WaitForSeconds(3f);
    }

    void HandlePlayerDeath()
    {
        // Player died :(
        Debug.Log("Player died :(((");
        GameOver();
    }
}
