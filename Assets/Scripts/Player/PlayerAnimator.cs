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
    }

    void Update()
    {   
        _animator.speed = _animatorSpeed;
        AnimateRun();
        AnimateJump();
    }

    private void AnimateRun()
    {
        _animator.SetFloat(velocityMagnitudeParameter, Mathf.Abs(_playerMovement.HorizontalInput));
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

