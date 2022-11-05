using System;
using UnityEngine;
using Helpers;

[RequireComponent(typeof(Collider2D))]
public class Hazard : MonoBehaviour
{
    [SerializeField]
    private PlayerHealth _playerHealth;
    private Timer _currentDamageCooldownTimer;

    [SerializeField] [Range(0.5f, 2f)]
    private float _damageCooldownLength = 1f;

    void Start()
    {
        _currentDamageCooldownTimer = new Timer(_damageCooldownLength, startTimerImmediately: false);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && _currentDamageCooldownTimer.IsTimerDone())
        {
            _currentDamageCooldownTimer.StartTimer();
            Debug.Log(other.gameObject.name + " collided with " + this.gameObject.name);
            GameManager.OnPlayerTakeDamage?.Invoke();
        }
    }

}
