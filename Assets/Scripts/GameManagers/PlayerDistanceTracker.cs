using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDistanceTracker : MonoBehaviour
{
    public static PlayerDistanceTracker Current;

    [SerializeField]
    private Transform _player;

    [SerializeField]
    private float _maxDistance;

    [SerializeField]
    private float _currentDistance;

    [SerializeField]
    private bool _isEnabled;

    private event UnityAction<float> OnDistanceUpdated;
    private event UnityAction OnDestinationReached;

    private void Awake()
    {
        Current = this;
    }
    private void Start()
    {
        _player = PlayerIdentifier.Current.transform;
    }
    private void Update()
    {
        if (_isEnabled)
        {
            _currentDistance = Mathf.Max(_maxDistance - Mathf.Abs(_player.position.y), 0);
            OnDistanceUpdated?.Invoke(_currentDistance);

            if (_currentDistance == 0)
            {
                OnDestinationReached?.Invoke();
                _isEnabled = false;
            }
        }
    }

    public void ResetTracker()
    {
        _isEnabled = true;
    }

    public void SubscribeToOnDistanceUpdated(UnityAction<float> callback) => HelperUtility.SubscribeTo(ref OnDistanceUpdated, ref callback);
    public void UnsubscribeFromOnDistanceUpdated(UnityAction<float> callback) => HelperUtility.UnsubscribeFrom(ref OnDistanceUpdated, ref callback);
    public void SubscribeToOnDestinationReached(UnityAction callback) => HelperUtility.SubscribeTo(ref OnDestinationReached, ref callback);
    public void UnsubscribeFromOnDestinationReached(UnityAction callback) => HelperUtility.UnsubscribeFrom(ref OnDestinationReached, ref callback);
}
