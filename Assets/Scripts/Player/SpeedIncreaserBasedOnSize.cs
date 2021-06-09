using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpeedIncreaserBasedOnSize : MonoBehaviour
{
    [SerializeField]
    private float _minObjSize, _maxObjSize, _currentObjSize;

    [SerializeField]
    private float _snowballSize, _maxSnowBallSizeToAffectObjScale, _snowballSizeIncreaseRate , _maxSnowballSize;

    private float _maxSpeed, _speedLerp;

    [SerializeField]
    private float _percentageSizeToReduceOnHit;

    [SerializeField]
    private GameObject _lastHitObject;

    [SerializeField]
    private float _minDistanceFromLastHitObjectRequiredToIgnoreIt;

    private event UnityAction<float> OnObjectSizeChanged;
    private event UnityAction<float> OnLerpValueOfSizeChanged;
    private event UnityAction<float> OnSnowBallSizeChanged;

    private void Start()
    {
        PlayerMovementBasedOnEnergy playerMovement = GetComponent<PlayerMovementBasedOnEnergy>();
        playerMovement.SubscribeToOnSpeedChanged(PlayerMovement_OnSpeedChanged);
        _maxSpeed = playerMovement.GetMaxSpeed();
    }

    private void PlayerMovement_OnSpeedChanged(float speed)
    {
        speed = Mathf.Max(speed, 0);
        _speedLerp = Mathf.Lerp(0, 1, speed / _maxSpeed);
    }

    private void Update()
    {
        float affectedFromSize = Mathf.Min(_snowballSize / _maxSnowBallSizeToAffectObjScale, 1);
        OnLerpValueOfSizeChanged?.Invoke(affectedFromSize);
        _currentObjSize = Mathf.Lerp(_minObjSize, _maxObjSize,  affectedFromSize);

        _snowballSize = Mathf.Clamp( _snowballSize + Time.deltaTime * _snowballSizeIncreaseRate * _speedLerp, 0 , _maxSnowballSize);

        OnObjectSizeChanged?.Invoke(_currentObjSize);
        OnSnowBallSizeChanged?.Invoke(_snowballSize);
        gameObject.transform.localScale = new Vector3(_currentObjSize, _currentObjSize, 0);

        ResetLastHitObjectWhenOutOfRange();
    }

    public float GetSnowBallSize() => _snowballSize;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Obstacle")
        {
            if(_lastHitObject != collision.gameObject)
            {
                _lastHitObject = collision.gameObject;
                _snowballSize = Mathf.Max(_snowballSize - _snowballSize * _percentageSizeToReduceOnHit, 0);
            }
        }
    }

    private void ResetLastHitObjectWhenOutOfRange()
    {
        if (_lastHitObject)
        {
            Vector2 direction = (_lastHitObject.transform.position - gameObject.transform.position).normalized;
            Debug.DrawRay(gameObject.transform.position, direction, Color.red, 0.1f);
            int layerMask = LayerMask.GetMask("Obstacle");

            if(!Physics2D.Raycast(gameObject.transform.position, direction, _minDistanceFromLastHitObjectRequiredToIgnoreIt, layerMask))
            {
                _lastHitObject = null;
            }
        }
    }
    public void SubscribeToOnObjectSizeChanged(UnityAction<float> callback) => HelperUtility.SubscribeTo(ref OnObjectSizeChanged, ref callback);
    public void UnsubscribeFromOnObjectSizeChanged(UnityAction<float> callback) => HelperUtility.UnsubscribeFrom(ref OnObjectSizeChanged, ref callback);
    public void SubscribeToOnLerpValueOfSizeChanged(UnityAction<float> callback) => HelperUtility.SubscribeTo(ref OnLerpValueOfSizeChanged, ref callback);
    public void UnsubscribeFromOnLerpValueOfSizeChanged(UnityAction<float> callback) => HelperUtility.UnsubscribeFrom(ref OnLerpValueOfSizeChanged, ref callback);
    public void SubscribeToOnSnowBallSizeChanged(UnityAction<float> callback) => HelperUtility.SubscribeTo(ref OnSnowBallSizeChanged, ref callback);
    public void UnsubscribeFromOnSnowBallSizeChanged(UnityAction<float> callback) => HelperUtility.UnsubscribeFrom(ref OnSnowBallSizeChanged, ref callback);


}
