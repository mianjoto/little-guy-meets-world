using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed;

    void Awake()
    {
        _rotateSpeed = 1f;
    }

    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        float yRotationOffset = 90f * Mathf.Sin(_rotateSpeed * Time.time) + 90; 
        transform.eulerAngles = new Vector2(transform.rotation.x, transform.rotation.y + yRotationOffset);
    }
}
