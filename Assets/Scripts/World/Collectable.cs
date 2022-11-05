using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Collectable : MonoBehaviour
{
    public abstract void Collect();
    
    private string _playerTag = "Player";

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(_playerTag))
        {
            Collect();
            Destroy(this.gameObject);
        }
    }
}
