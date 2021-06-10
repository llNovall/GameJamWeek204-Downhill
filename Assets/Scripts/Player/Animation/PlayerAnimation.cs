using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private float _animationSpeedOffset;

    private void Start()
    {
        if (!_animator)
            _animator = GetComponent<Animator>();

        PlayerMovementBasedOnEnergy playerMovement = GetComponent<PlayerMovementBasedOnEnergy>();
        playerMovement.SubscribeToOnSpeedLerpChanged(PlayerMovementBasedOnEnergy_OnSpeedLerpChanged);

        SpeedIncreaserBasedOnSize sizeController = GetComponent<SpeedIncreaserBasedOnSize>();
        sizeController.SubscribeToOnLerpValueOfSizeChanged(SpeedIncreaserBasedOnSize_OnLerpValueOfSizeChanged);
    }

    private void PlayerMovementBasedOnEnergy_OnSpeedLerpChanged(float speedLerp)
    {
        _animator.speed = Mathf.Min(1, speedLerp + _animationSpeedOffset);
    }

    private void SpeedIncreaserBasedOnSize_OnLerpValueOfSizeChanged(float sizeLerp)
    {
        _animator.SetFloat("size", sizeLerp);
    }

    public void IsHitObstacle(bool isHit)
    {
        _animator.SetBool("isHit", isHit);
    }
}
