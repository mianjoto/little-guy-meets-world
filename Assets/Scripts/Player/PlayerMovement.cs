using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private BoxCollider2D _collider;

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
        private float _clampXSpeed = 6f;
        [SerializeField]
        private float _clampYSpeed = 15f;
        [SerializeField]
        private List<LayerMask> _nonJumpableLayers;
        private LayerMask _groundLayer;
    #endregion

    #region Input and Movement
        [HideInInspector]
        public float HorizontalInput;
        [HideInInspector]
        public float HorizontalMovement;
        [HideInInspector]
        public float VerticalMovement;
        private bool _tryingToMoveHorizontally = false;
        private bool _tryingToJump = false;
        private bool _isStillPressingJump = false;
        private bool _hasShortJumped = false;
        private bool _queuedShortJump = false;
        private float _minJumpTimeLength = .5f;
        private float _maxJumpTimeLength = 1f;
        private float _timeSinceLastJump;
    #endregion
    [SerializeField]
    
    #region State checks
        [HideInInspector]
        public bool IsGrounded {
            get
            {
                Vector2 center = _collider.bounds.center;
                Vector2 bottomOfPlayer = new Vector2(center.x, center.y - _collider.bounds.extents.y);
                Vector2 boxDimensions = new Vector2(rayCastLength, rayCastDepth);

                int validLayers =~ LayerMask.GetMask("Ignore Raycast");
                RaycastHit2D hit = Physics2D.BoxCast(bottomOfPlayer, boxDimensions, 0f, Vector2.down, 0.05f, validLayers);
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
    private float rayCastLength = 0.7f;
    [SerializeField]
    private float rayCastDepth = 0.2f;
    private KeyCode _jumpKey = GameManager.JumpKey;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _runSpeed = 2.25f;
        IsFacingRight = true;
    }

    void Update()
    {
        CheckForInput();
        FlipIfNecessary();
    }

    void FixedUpdate()
    {
        if (_tryingToMoveHorizontally)
        {
            Run();
        }
        Jump();
        if (_tryingToMoveHorizontally || _tryingToJump)
        {
            ClampVelocity();    
        }
        FastFall();
    }

    private void CheckForInput()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        if (HorizontalInput != 0)
        {
            _tryingToMoveHorizontally = true;
        }
        else
        {
            _tryingToMoveHorizontally = false;
        }

        if (Input.GetKeyDown(_jumpKey))
        {
            _tryingToJump = true;
        }
    }

    private void ClampVelocity()
    {
        float clampedX = Mathf.Clamp(_rb.velocity.x, -_clampXSpeed, _clampXSpeed);
        float clampedY = Mathf.Clamp(_rb.velocity.y, -_clampYSpeed, _clampYSpeed);
        _rb.velocity = new Vector2(clampedX, clampedY);        
    }

    void Run()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        HorizontalMovement = _rb.velocity.x;
        if (HorizontalInput == 0)
        {
            // SlowHorizontalVelocity();
            return;
        }

        if (HorizontalInput > 0)
        {
            _rb.AddForce(transform.right.normalized * _runSpeed, ForceMode2D.Force);
        }
        else
        {
            _rb.AddForce(-transform.right.normalized * _runSpeed, ForceMode2D.Force);
        }
    }

    private void SlowHorizontalVelocity()
    {
        float penalty = 2f;
        _rb.velocity = new Vector2(_rb.velocity.x / penalty, _rb.velocity.y);
    }

    void Jump()
    {
        if (_tryingToJump && IsGrounded)
        {
            _rb.AddForce(Vector2.up, ForceMode2D.Impulse);
            _isStillPressingJump = true;
            _tryingToJump = false;
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
                // Halve air velocity
                float reducedVelocity = _rb.velocity.y / 1.5f;
                _rb.velocity = new Vector2(_rb.velocity.x, reducedVelocity);
                _rb.AddForce(Vector2.down * VerticalMovement, ForceMode2D.Force);
                _hasShortJumped = true;    
            }
            else if (!canShortJump)
            {
                _queuedShortJump = true;
            }
        }
        
        if (VerticalMovement < _fastFallDownThreshold)
        {
            _rb.AddForce(Vector2.down * _fastFallDownSpeed, ForceMode2D.Force);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (_collider == null)
        {
            return;
        }
        Vector2 center = _collider.bounds.center;
        Vector2 bottomOfPlayer = new Vector2(center.x, center.y - _collider.bounds.extents.y);
        Vector2 boxDimensions = new Vector2(rayCastLength, rayCastDepth);


        Gizmos.DrawWireCube(bottomOfPlayer, boxDimensions);
    }
}
