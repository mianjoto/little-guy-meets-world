using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    [SerializeField]
    private float _floatDistance;
    [SerializeField]
    private float _floatSpeed;

    private float baseXposition;
    private float baseYposition;
    private Vector2 minPosition;
    private Vector2 maxPosition;

    void Awake()    
    {
        baseXposition = transform.position.x;
        baseYposition = transform.position.y;
        minPosition = new Vector2(baseXposition, baseYposition - _floatDistance - GetComponent<SpriteRenderer>().bounds.extents.y);
        maxPosition = new Vector2(baseXposition, baseYposition + _floatDistance + GetComponent<SpriteRenderer>().bounds.extents.y);
    }

    void Update()
    {
        Float();        
    }

    private void Float()
    {
        float yOffset = _floatDistance * Mathf.Sin(_floatSpeed * Time.time);
        transform.position = new Vector2(baseXposition, baseYposition + yOffset);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(minPosition, maxPosition);
    }
}
