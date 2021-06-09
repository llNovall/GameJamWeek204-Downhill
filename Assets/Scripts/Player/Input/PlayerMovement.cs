using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private SpeedData _speedData;

    [SerializeField]
    private float _minSlideSpeed, _maxSlideSpeed, _maxSpeedFromSize, _currentSlideSpeed;

    [SerializeField]
    private float _currentSpeed, _accelerationSpeed;
    
    [SerializeField]
    private float _reverseAccelerationSpeed;

    [SerializeField]
    private float _minDistanceFromWall, _maxDistanceFromWall, _currentDistanceFromWall;

    [SerializeField]
    private float _maxRotation;

    [SerializeField]
    private int _layerMask;

    [SerializeField]
    private float _sizeLerp;

    [SerializeField]
    private float _minEnergyUsage, _maxEnergyUsage;

    [SerializeField]
    private PlayerEnergy _playerEnergy;

    [SerializeField]
    private Vector3 _startPosition;

    [SerializeField]
    private bool _isEnabled;

    private event UnityAction<float> OnSpeedChanged;
    private void Start()
    {
        _startPosition = transform.position;
        _isEnabled = true;
        _layerMask = LayerMask.GetMask("Walls", "Obstacle");

        PlayerInputController inputController = gameObject.GetComponent<PlayerInputController>();
        inputController.SubscribeToOnPlayerSideWaysMovementInputUpdated(PlayerInputController_OnPlayerSideWaysMovementInputUpdated);
        inputController.SubscribeToOnPlayerBackwardsMovementInputUpdated(PlayerInputController_OnPlayerBackwardsMovementInputUpdated);
       

        SpeedIncreaserBasedOnSize speedIncrease = gameObject.GetComponent<SpeedIncreaserBasedOnSize>();
        speedIncrease.SubscribeToOnLerpValueOfSizeChanged(SpeedIncreaserBasedOnSize_OnLerpValueOfSizeChanged);

        _playerEnergy = gameObject.GetComponent<PlayerEnergy>();
        

    }

    private void FixedUpdate()
    {
        if (_isEnabled)
        {
            Vector2 move = gameObject.transform.rotation * Vector2.down * _currentSpeed * Time.fixedDeltaTime;

            if (!Physics2D.Raycast(gameObject.transform.position, move.normalized, _currentDistanceFromWall, _layerMask))
            {
                gameObject.transform.position += (Vector3)move;
                _currentSpeed = Mathf.Min(_currentSpeed + _accelerationSpeed * Time.fixedDeltaTime, _maxSpeedFromSize);

                float lerpValue = _currentSpeed.MapValue(_speedData.ReverseSpeed, _speedData.MaxSpeed, 0.1f, 1);
                _playerEnergy.RemoveEnergy(Mathf.Lerp(_minEnergyUsage, _maxEnergyUsage, lerpValue) * Time.fixedDeltaTime);

                OnSpeedChanged?.Invoke(_currentSpeed);

            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, move.normalized, _currentDistanceFromWall, _layerMask);

                move = Vector2.down * _currentSpeed;

                if (hit)
                {
                    move += (Vector2)gameObject.transform.position - hit.point;
                }
                gameObject.transform.position += (Vector3)move * Time.fixedDeltaTime;
                _currentSpeed = Mathf.Min(_currentSpeed + _accelerationSpeed * Time.fixedDeltaTime, _maxSpeedFromSize);

                float lerpValue = _currentSpeed.MapValue(_speedData.ReverseSpeed, _speedData.MaxSpeed, 0.1f, 1);
                _playerEnergy.RemoveEnergy(Mathf.Lerp(_minEnergyUsage, _maxEnergyUsage, lerpValue) * Time.fixedDeltaTime);

                OnSpeedChanged?.Invoke(_currentSpeed);
            }
        }
    }

    public void ResetPlayerMovement()
    {
        gameObject.transform.position = _startPosition;
        _isEnabled = true;
    }
    public float GetMaxSpeed() => _speedData.MaxSpeed;

    private void SpeedIncreaserBasedOnSize_OnLerpValueOfSizeChanged(float lerpValue)
    {
        _sizeLerp = lerpValue;
        _currentSlideSpeed = Mathf.Lerp(_maxSlideSpeed, _minSlideSpeed, lerpValue);
        _maxSpeedFromSize = Mathf.Lerp(_speedData.MinSpeed, _speedData.MaxSpeed, lerpValue);
        _currentDistanceFromWall = Mathf.Lerp(_minDistanceFromWall, _maxDistanceFromWall, lerpValue);
        _accelerationSpeed = Mathf.Lerp(_speedData.MinAccelerationSpeed, _speedData.MaxAccelerationSpeed, lerpValue);
        _reverseAccelerationSpeed = Mathf.Lerp(_speedData.MinReverseAcceleration, _speedData.MaxReverseAcceleration, lerpValue);
    }
    private void PlayerInputController_OnPlayerBackwardsMovementInputUpdated(float input)
    {
        if(input < 0)
        {
            _currentSpeed = Mathf.Clamp(_currentSpeed + Time.fixedDeltaTime * input * _reverseAccelerationSpeed, _speedData.ReverseSpeed, _speedData.MaxSpeed);

            _playerEnergy.RemoveEnergy(Mathf.Lerp(_minEnergyUsage, _maxEnergyUsage, _sizeLerp) * Time.fixedDeltaTime);
        }
        else
        {
            _currentSpeed = Mathf.Min(_currentSpeed + _accelerationSpeed * Time.fixedDeltaTime, _maxSpeedFromSize);

            _playerEnergy.RemoveEnergy(Mathf.Lerp(_maxEnergyUsage, _minEnergyUsage, _sizeLerp) * Time.fixedDeltaTime);
        }
    }

    private void PlayerInputController_OnPlayerSideWaysMovementInputUpdated(float input)
    {
        Vector2 direction = input * Vector2.right * _currentSlideSpeed * Time.fixedDeltaTime;

        if (input < 0)
        {
            if (gameObject.transform.rotation.eulerAngles.z < 360 - _maxRotation && gameObject.transform.rotation.eulerAngles.z > 180)
                return;
        }
        else if(input > 0)
        {
            if (gameObject.transform.rotation.eulerAngles.z > _maxRotation && gameObject.transform.rotation.eulerAngles.z <= 180)
                return;
        }

        if (Physics2D.Raycast(gameObject.transform.position, direction.normalized, _minDistanceFromWall, _layerMask))
            return;

        Vector3 eulerRotation = gameObject.transform.rotation.eulerAngles + new Vector3(0,0, direction.x);

        gameObject.transform.rotation = Quaternion.Euler(eulerRotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Obstacle")
        {
            _currentSpeed = 0;
            OnSpeedChanged?.Invoke(_currentSpeed);
        }
    }

    public void SubscribeToOnSpeedChanged(UnityAction<float> callback) => HelperUtility.SubscribeTo(ref OnSpeedChanged, ref callback);
    public void UnsubscribeFromOnSpeedChanged(UnityAction<float> callback) => HelperUtility.UnsubscribeFrom(ref OnSpeedChanged, ref callback);

}
