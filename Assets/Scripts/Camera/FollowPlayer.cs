using System;
using DG.Tweening;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private bool _useSmoothCamera;
    [SerializeField]
    private float _cameraTransitionSpeed;
    [SerializeField] [Tooltip("The position offset added to the camera's position")]
    private Vector3 _cameraOffset;

    private Transform _playerTransform;
    private Vector3 _lastPlayerPosition;
    private Transform _mainCameraTransform;
    private float _cameraZPosition;    
    private float _dontMoveYThreshold;    
    
    void Start()
    {
        // _playerTransform = GameManager
        _playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _mainCameraTransform = Camera.main.transform;
        _cameraZPosition = _mainCameraTransform.transform.position.z;
        _useSmoothCamera = true;
        _cameraTransitionSpeed = 2f;
        _dontMoveYThreshold = 0.1f;
    }

    void Update()
    {
        if (_useSmoothCamera)
        {
            SmoothCameraFollowPlayer();
        }
        else
        {
            CameraFollowPlayer();
        }
    }

    void LateUpdate()
    {
        _lastPlayerPosition = _playerTransform.position;
    }

    private void CameraFollowPlayer()
    {
        _mainCameraTransform.position = transformCameraPosition(_playerTransform.position);
    }

    private void SmoothCameraFollowPlayer()
    {
        _mainCameraTransform.DOMove(transformCameraPosition(_playerTransform.position), _cameraTransitionSpeed);
    }

    Vector3 transformCameraPosition(Vector3 startingPosition)
    {
        return resetZ(addOffset(clampYTransition(startingPosition)));
    }

    private Vector3 addOffset(Vector3 startingPosition)
    {
        return startingPosition + _cameraOffset;
    }

    Vector3 resetZ(Vector3 startingPosition)
    {
        return new Vector3(startingPosition.x, startingPosition.y, _cameraZPosition);
    }

    Vector3 clampYTransition(Vector3 startingPosition)
    {
        float yDifference = MathF.Abs(_playerTransform.position.y - _lastPlayerPosition.y);
        // Don't move y position if under threshold
        if (yDifference > _dontMoveYThreshold)
        {
            return new Vector2(_playerTransform.position.x, _lastPlayerPosition.y);
        }
        else
        {
            return _playerTransform.position;
        }
    }

    void OnDisable()
    {
        this.GetComponent<Camera>().enabled = false;
    }

    void OnEnable()
    {
        this.GetComponent<Camera>().enabled = true;
    }

}