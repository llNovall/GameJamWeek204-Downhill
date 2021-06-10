using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField]
    private float _volume;

    private EventInstance _eventInstance;

    private void Start()
    {
        if (VolumeManager.Current)
        {
            _volume = VolumeManager.Current.SFX;
            VolumeManager.Current.SubscribeToChangeSFX(VolumeManager_OnChangeSFX);
        }
        else
            Debug.LogError($"Failed to find VolumeManager");
    }
    public void PlaySound(string eventID)
    {
        if (_eventInstance.isValid())
        {
            _eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _eventInstance.release();
        }

        _eventInstance = RuntimeManager.CreateInstance(eventID);
        _eventInstance.start();
        _eventInstance.setVolume(_volume);

    }

    protected void PlayOneShotSound(string eventID)
    {
        RuntimeManager.PlayOneShot(eventID);
    }

    private void StopSound()
    {
        if (_eventInstance.isValid())
        {
            _eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _eventInstance.release();
        }
    }
    private void UpdateVolume(float newVolume)
    {
        _volume = newVolume;
        if (_eventInstance.isValid())
        {
            _eventInstance.setVolume(_volume);
        }
    }

    private void OnBecameInvisible()
    {
        StopSound();
    }
    private void OnDestroy()
    {
        if (VolumeManager.Current)
        {

            VolumeManager.Current.UnsubscribeFromChangeSFX(VolumeManager_OnChangeSFX);
        }
        else
            Debug.LogError($"Failed to find VolumeManager");

        StopSound();
    }

    private void VolumeManager_OnChangeSFX(float volume) => UpdateVolume(volume);
}
