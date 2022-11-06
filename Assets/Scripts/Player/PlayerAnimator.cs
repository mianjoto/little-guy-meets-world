using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement _playerMovement;
    [SerializeField]
    private Animator _animator;
    [SerializeField] [Range(0,2)]
    private float _animatorSpeed; 

    #region AnimatorParameters
        private string groundedParameter = "isGrounded";
        private string velocityMagnitudeParameter = "absVelocityXMagnitude";
    #endregion

    void Start()
    {
        _animator.speed = _animatorSpeed;
    }

    void Update()
    {   
        AnimateRun();
        AnimateJump();
    }

    private void AnimateRun()
    {
        float hInput = _playerMovement.HorizontalInput;
        _animator.SetFloat(velocityMagnitudeParameter, Mathf.Abs(hInput));
    }

    void AnimateJump()
    {
        if (_playerMovement.IsGrounded == true)
        {
            _animator.SetBool(groundedParameter, true);
        } else
        {
            _animator.SetBool(groundedParameter, false);
        }
    }

}

