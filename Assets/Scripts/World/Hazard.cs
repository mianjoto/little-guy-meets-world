using System;
using UnityEngine;
using Helpers;

[RequireComponent(typeof(Collider2D))]
public class Hazard : MonoBehaviour
{
    [SerializeField]
    private PlayerHealth _playerHealth;
    [HideInInspector]
    public bool _hazardIsEnabled;

    public Helpers.Timer _currentDamageCooldownTimer;

    [SerializeField] [Range(0.5f, 2f)]
    private float _damageCooldownLength = 1f;

    public virtual void Awake()
    {
        _playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();    
        _hazardIsEnabled = false;
        _currentDamageCooldownTimer = new Timer(_damageCooldownLength, startTimerImmediately: false);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (_hazardIsEnabled &&
            other.gameObject.CompareTag("Player") &&
            _currentDamageCooldownTimer.IsTimerDone())
        {
            _currentDamageCooldownTimer.StartTimer();
            GameManager.OnPlayerTakeDamage?.Invoke();
        }
    }

}
