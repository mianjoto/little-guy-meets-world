using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Fan : MonoBehaviour
{
    [SerializeField] [Tooltip("The strength of the fan drift")]
    private float _fanPower = 2f;
    private Rigidbody2D _playerRb;
    [SerializeField]
    private Vector2 direction;

    void Start()
    {
        _playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        direction = GetDirection();
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PushPlayer();
        }
    }

    private void PushPlayer()
    {
        _playerRb.AddForce(direction * _fanPower, ForceMode2D.Force);
    }

    private Vector2 GetDirection()
    {
        Vector2 direction = Vector2.zero;
        float orientation = transform.eulerAngles.z / 360f;
        switch (orientation)
        {
            case 0:
                direction = Vector2.left;
                break;
            case 0.25f:
                direction = Vector2.down;
                break;
            case 0.5f:
                direction = Vector2.right;
                break;
            case 0.75f:
                direction = Vector2.up;
                break;
        }
        return direction;
    }
}