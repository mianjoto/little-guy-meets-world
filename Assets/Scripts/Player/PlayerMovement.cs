using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private CapsuleCollider2D _collider;

    #region Player Movement Variables
        [Header("Player Movement Variables")]
        [SerializeField]
        private float _runSpeed;
        [SerializeField]
        private float _jumpSpeed;
        [SerializeField]
        private float _fastFallDownSpeed;
        [SerializeField]
        private float _fastFallDownThreshold;
        [SerializeField]
        private LayerMask _groundLayer;
    #endregion

    #region Input and Movement
        [HideInInspector]
        public float HorizontalInput;
        [HideInInspector]
        public float HorizontalMovement;
        [HideInInspector]
        public float VerticalMovement;
        private bool _isStillPressingJump = false;
        private bool _hasShortJumped = false;
        private bool _queuedShortJump = false;
        private float _minJumpTimeLength = .5f;
        private float _maxJumpTimeLength = 1f;
        private float _timeSinceLastJump;
    #endregion
    
    #region State checks
        [HideInInspector]
        public bool IsGrounded {
                get
                {
                    Vector2 center = _collider.bounds.center;
                    Vector2 bottomOfPlayer = new Vector2(center.x, center.y - _collider.bounds.extents.y);
                    RaycastHit2D hit = Physics2D.Raycast(bottomOfPlayer, Vector2.down, rayCastLength, _groundLayer);

                    if (hit.collider != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }            
            }
        public bool IsFacingRight;
    #endregion    

    [SerializeField]
    private float rayCastLength = 0.5f;
    private KeyCode _jumpKey = KeyCode.Space;


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
        _runSpeed = 2.5f;
        IsFacingRight = true;
    }

    void Update()
    {
        Run();
        Jump();
        FastFall();
        FlipIfNecessary();
        // print("timesincelastjump: " + _timeSinceLastJump + "s");
    }

    void FixedUpdate()
    {
    }

    void Run()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        HorizontalMovement = _rb.velocity.x;
        if (HorizontalInput == 0)
        {
            return;
        }

        if (HorizontalInput > 0)
        {
            this.transform.Translate(Vector2.right * _runSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            this.transform.Translate(Vector3.left * _runSpeed * Time.deltaTime, Space.World);
        }
    }
    
    void Jump()
    {
        if (Input.GetKeyDown(_jumpKey) && IsGrounded)
        {
            _rb.AddForce(Vector2.up, ForceMode2D.Impulse);
            _isStillPressingJump = true;
        }
        if (Input.GetKeyUp(_jumpKey))
        {
            _isStillPressingJump = false;
        }

        if (Input.GetKey(_jumpKey) && _isStillPressingJump)
        {
            _timeSinceLastJump += Time.deltaTime; 
        }
        
        if (IsGrounded)
        {
            _timeSinceLastJump = 0;
            _hasShortJumped = false;
            _queuedShortJump = false;
        }
    }

    void FlipIfNecessary()
    {
        if (HorizontalInput == 0)
        {
            return;
        }

        if (HorizontalInput < 0 && IsFacingRight)
        {
            FlipCharacter();
        }
        else if (HorizontalInput > 0 && !IsFacingRight)
        {
            FlipCharacter();
        }
    }

    private void FlipCharacter()
    {
        IsFacingRight = !IsFacingRight;

        Vector2 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void FastFall()
    {
        if (IsGrounded)
        {
            return;
        }
        VerticalMovement = _rb.velocity.y;

        if (!_isStillPressingJump && !_hasShortJumped)
        {
            bool canShortJump = _timeSinceLastJump < _maxJumpTimeLength && _timeSinceLastJump > _minJumpTimeLength;
            if (_queuedShortJump || canShortJump)
            {
                print("Short jumped");
                // Halve air velocity
                _rb.velocity /= 1.5f;
                _rb.AddForce(Vector2.down * VerticalMovement, ForceMode2D.Force);
                _hasShortJumped = true;    
            }
            else if (!canShortJump)
            {
                _queuedShortJump = true;
                print("Queued short jump!");
            }
        }
        
        // print("vmovement="+VerticalMovement);

        
        if (VerticalMovement < _fastFallDownThreshold)
        {
            _rb.AddForce(Vector2.down * _fastFallDownSpeed, ForceMode2D.Force);
        }
    }

    void OnDrawGizmos()
    {
        if (_collider == null)
        {
            return;
        }
        Vector2 center = _collider.bounds.center;
        Vector2 bottomOfPlayer = new Vector2(center.x, center.y - _collider.bounds.extents.y);
        Gizmos.DrawRay(bottomOfPlayer, Vector2.down * rayCastLength);
    }
}
