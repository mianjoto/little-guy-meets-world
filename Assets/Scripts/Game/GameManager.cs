using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singletons
        [SerializeField]
        public static GameManager Instance { get; private set; }
        [SerializeField]
        public static GameObject Player;
    #endregion

    #region Scripts
        [SerializeField]
        private PlayerHealth _playerHealth;
    #endregion

    [HideInInspector]
    public static GameData GameData;

    # region Events
        [SerializeField]
        public static Action OnPlayerTakeDamage;
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

        Player = SingletonHandler.ReturnGameObjectIfNotInitialized(Player, "Player");
    }

    void Start()
    {
        _playerHealth.OnPlayerDied += HandlePlayerDeath;
    }

    void HandlePlayerDeath()
    {
        // Player died :(
        Debug.Log("Player died :(((");
    }
}
