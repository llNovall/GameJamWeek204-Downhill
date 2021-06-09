using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovementBasedOnEnergy : MonoBehaviour
{
        [SerializeField]
    private SpeedData _speedData;

    [SerializeField]
    private float _minSlideSpeed, _maxSlideSpeed, _maxSpeedFromEnergy, _currentSlideSpeed;

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
    private float _energyLerp;

    [SerializeField]
    private float _minEnergyUsage, _maxEnergyUsage;

    [SerializeField]
    private PlayerEnergy _playerEnergy;

    [SerializeField]
    private Vector3 _startPosition;

    private PlayerEnergyState _energyState;

    [SerializeField]
    private float _eatSpeed, _eatSpeedAcceleration;

    [SerializeField]
    private bool _isEnabled;

    private event UnityAction<float> OnSpeedChanged;
    private event UnityAction<float> OnSpeedLerpChanged;
    private void Start()
    {
        _startPosition = transform.position;
        _isEnabled = true;
        _layerMask = LayerMask.GetMask("Walls", "Obstacle");

        PlayerInputController inputController = gameObject.GetComponent<PlayerInputController>();
        inputController.SubscribeToOnPlayerSideWaysMovementInputUpdated(PlayerInputController_OnPlayerSideWaysMovementInputUpdated);
        inputController.SubscribeToOnPlayerBackwardsMovementInputUpdated(PlayerInputController_OnPlayerBackwardsMovementInputUpdated);

        _playerEnergy = gameObject.GetComponent<PlayerEnergy>();
        _playerEnergy.SubscribeToOnEnergyLerpUpdated(PlayerEnergy_OnEnergyLerpUpdated);
        _playerEnergy.SubscribeToOnEnergyStateChanged(PlayerEnergy_OnEnergyStateChanged);

    }

    private void PlayerEnergy_OnEnergyStateChanged(PlayerEnergyState energyState)
    {
        _energyState = energyState;
    }

    private void FixedUpdate()
    {
        if (_isEnabled)
        {
            Vector2 move = gameObject.transform.rotation * Vector2.down * _currentSpeed * Time.fixedDeltaTime;

            if (!Physics2D.Raycast(gameObject.transform.position, move.normalized, _currentDistanceFromWall, _layerMask))
            {
                gameObject.transform.position += (Vector3)move;
                if(_energyState != PlayerEnergyState.Eaten)
                {
                    _currentSpeed = Mathf.Min(_currentSpeed + _accelerationSpeed * Time.fixedDeltaTime, _maxSpeedFromEnergy);


                    float lerpValue = _currentSpeed.MapValue(_speedData.ReverseSpeed, _speedData.MaxSpeed, 0.1f, 1);
                    _playerEnergy.RemoveEnergy(Mathf.Lerp(_minEnergyUsage, _maxEnergyUsage, lerpValue) * Time.fixedDeltaTime);
                }
                else
                {
                    if(_currentSpeed > _eatSpeed)
                    {
                        _currentSpeed = Mathf.Max(_currentSpeed - _eatSpeedAcceleration * Time.fixedDeltaTime, _eatSpeed);
                    }
                }
                    

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

                if(_energyState != PlayerEnergyState.Eaten)
                {
                    _currentSpeed = Mathf.Min(_currentSpeed + _accelerationSpeed * Time.fixedDeltaTime, _maxSpeedFromEnergy);

                    float lerpValue = _currentSpeed.MapValue(_speedData.ReverseSpeed, _speedData.MaxSpeed, 0.1f, 1);
                    _playerEnergy.RemoveEnergy(Mathf.Lerp(_minEnergyUsage, _maxEnergyUsage, lerpValue) * Time.fixedDeltaTime);
                }
                else
                {
                    if (_currentSpeed > _eatSpeed)
                    {
                        _currentSpeed = Mathf.Max(_currentSpeed - _eatSpeedAcceleration * Time.fixedDeltaTime, _eatSpeed);
                    }
                }
                

                OnSpeedChanged?.Invoke(_currentSpeed);
                float speedLerp = _currentSpeed >= 0 ? _currentSpeed / _speedData.MaxSpeed : Mathf.Abs(_currentSpeed) / Mathf.Abs(_speedData.ReverseSpeed);
                OnSpeedLerpChanged?.Invoke(speedLerp);
            }
        }
    }

    public void ResetPlayerMovement()
    {
        gameObject.transform.position = _startPosition;
        _isEnabled = true;
    }
    public float GetMaxSpeed() => _speedData.MaxSpeed;

    private void PlayerEnergy_OnEnergyLerpUpdated(float lerpValue)
    {
        _energyLerp = lerpValue;
        _currentSlideSpeed = _maxSlideSpeed;
        //_currentSlideSpeed = Mathf.Lerp(_maxSlideSpeed, _minSlideSpeed, lerpValue);
        _maxSpeedFromEnergy = Mathf.Lerp(_speedData.MinSpeed, _speedData.MaxSpeed, lerpValue);
        _currentDistanceFromWall = Mathf.Lerp(_minDistanceFromWall, _maxDistanceFromWall, lerpValue);
        _accelerationSpeed = Mathf.Lerp(_speedData.MinAccelerationSpeed, _speedData.MaxAccelerationSpeed, lerpValue);
        _reverseAccelerationSpeed = Mathf.Lerp(_speedData.MinReverseAcceleration, _speedData.MaxReverseAcceleration, lerpValue);
    }
    private void PlayerInputController_OnPlayerBackwardsMovementInputUpdated(float input)
    {
        if(input < 0)
        {
            _currentSpeed = Mathf.Clamp(_currentSpeed + Time.fixedDeltaTime * input * _reverseAccelerationSpeed, _speedData.ReverseSpeed, _speedData.MaxSpeed);

            _playerEnergy.RemoveEnergy(Mathf.Lerp(_minEnergyUsage, _maxEnergyUsage, _energyLerp) * Time.fixedDeltaTime);
        }
        else
        {
            _currentSpeed = Mathf.Min(_currentSpeed + _accelerationSpeed * Time.fixedDeltaTime, _maxSpeedFromEnergy);

            _playerEnergy.RemoveEnergy(Mathf.Lerp(_maxEnergyUsage, _minEnergyUsage, _energyLerp) * Time.fixedDeltaTime);
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
    public void SubscribeToOnSpeedLerpChanged(UnityAction<float> callback) => HelperUtility.SubscribeTo(ref OnSpeedLerpChanged, ref callback);
    public void UnsubscribeFromOnSpeedLerpChanged(UnityAction<float> callback) => HelperUtility.UnsubscribeFrom(ref OnSpeedLerpChanged, ref callback);

}
