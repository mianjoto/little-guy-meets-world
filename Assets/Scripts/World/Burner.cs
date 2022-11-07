using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Burner : Hazard
{
    #region Sprites
        [Header("Active/Inactive Sprites")]
        [SerializeField]
        private Sprite _inactivatedBurnerSprite;
        [SerializeField]
        private Sprite _activatedBurnerSprite;
        private SpriteRenderer _sr;
    #endregion

    #region Properties
        [Header("Burner properties")]
        [SerializeField] [Tooltip("Whether the burner is active")]
        private bool _active = false;
        [SerializeField] [Tooltip("Whether the burner is cycling between on and off")]
        private bool _burnerCycleOn = true;
        [SerializeField] [Tooltip("How long the burner stay on and off for")]
        private float _activityInterval = 3f;
        [SerializeField] [Tooltip("How much time to wait before starting the cycle")]
        private float _cycleTimeOffset;
        [SerializeField] [Tooltip("Whether the burner stays on forever")]
        private bool _isActiveForever = false;
    #endregion

    public override void Awake()
    {
        base.Awake();
        _sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (_isActiveForever)
        {
            ActivateBurner();
            return;
        }
        StartCoroutine(TurnOnBurnerCycle(interval: _activityInterval, offset: _cycleTimeOffset));
    }


    void ActivateBurner()
    {
        _active = true;
        _hazardIsEnabled = _active;
        _sr.sprite = _activatedBurnerSprite;
    }

    void DeactivateBurner()
    {
        _active = false;
        _hazardIsEnabled = _active;
        _sr.sprite = _inactivatedBurnerSprite;
    }

    IEnumerator TurnOnBurnerCycle(float interval, float offset = 0)
    {
        if (offset > 0)
        {
            yield return new WaitForSeconds(offset);
        }
        while (_burnerCycleOn)
        {
            ActivateBurner();
            yield return new WaitForSeconds(interval);
            DeactivateBurner();
            yield return new WaitForSeconds(interval);
        }
    }
}
