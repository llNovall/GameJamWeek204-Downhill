using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Current;

    [SerializeField]
    private float _currentTime, _timeRequired;

    [SerializeField]
    private Camera _mainCamera;

    [SerializeField, Tooltip("Use this only for debugging")]
    private bool _isTimeFinished;

    [SerializeField]
    private Transform _player;

    private EventInstance _eventInstance;

    private event UnityAction<float> OnProgressUpdated;
    private event UnityAction OnTimeFinished;

    private void Awake()
    {
        Current = this;
    }

    private void Start()
    {
        if (PlayerIdentifier.Current)
            _player = PlayerIdentifier.Current.transform;
        else
            Debug.LogError($"{GetType().FullName} : Failed to find Player.");

        _mainCamera = Camera.main;

        _currentTime = _timeRequired;
    }

    private void Update()
    {
        if (!_isTimeFinished)
        {
            _currentTime -= Time.deltaTime;
            _currentTime = Mathf.Max(_currentTime, 0);

            OnProgressUpdated?.Invoke(_currentTime);

            if (_currentTime == 0)
            {
                _isTimeFinished = true;
                PlaySound("event:/SFX/Cheer");
                OnTimeFinished?.Invoke();
            }
        }
    }

    public void ResetTime()
    {
        _currentTime = _timeRequired;
        _isTimeFinished = false;
        OnProgressUpdated?.Invoke(_currentTime);
    }
    public float GetMaxProgress() => _timeRequired;

    protected void PlaySound(string eventID)
    {
        if (_eventInstance.isValid())
        {
            _eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _eventInstance.release();
        }

        _eventInstance = RuntimeManager.CreateInstance(eventID);
        _eventInstance.start();
        _eventInstance.setVolume(VolumeManager.Current.SFX);

    }

    private void OnDestroy()
    {
        if (_eventInstance.isValid())
        {
            _eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _eventInstance.release();
        }
    }
    #region Event Subscription
    public void SubscribeToOnProgressUpdated(UnityAction<float> callback) => HelperUtility.SubscribeTo(ref OnProgressUpdated, ref callback);
    public void UnsubscribeFromOnProgressUpdated(UnityAction<float> callback) => HelperUtility.UnsubscribeFrom(ref OnProgressUpdated, ref callback);
    public void SubscribeToOnTimeFinished(UnityAction callback) => HelperUtility.SubscribeTo(ref OnTimeFinished, ref callback);
    public void UnsubscribeFromOnTimeFinished(UnityAction callback) => HelperUtility.UnsubscribeFrom(ref OnTimeFinished, ref callback);
    #endregion
}
