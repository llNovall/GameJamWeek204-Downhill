using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager Current;

    [SerializeField]
    private float musicVolume;
    [SerializeField]
    private float SFXVolume;

    private event UnityAction<float> ChangeSFX;

    private event UnityAction<float> ChangeMusic;

    

    private void Awake()
    {
        if (!Current)
        {
            Current = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void SetSFX(float volume)
    {
        SFXVolume = volume;
        ChangeSFX?.Invoke(SFXVolume);
    }
    public void SetMusic(float volume)
    {
        musicVolume = volume;
        ChangeMusic?.Invoke(musicVolume);
    }

    public float SFX
    {
        get => SFXVolume;
        set => SFXVolume = value;
    }
    public float Music
    {
        get => musicVolume;
        set => musicVolume = value;
    }

    public void SubscribeToChangeSFX(UnityAction<float> callback) => HelperUtility.SubscribeTo(ref ChangeSFX, ref callback);
    public void UnsubscribeFromChangeSFX(UnityAction<float> callback) => HelperUtility.UnsubscribeFrom(ref ChangeSFX, ref callback);

    public void SubscribeToChangeMusic(UnityAction<float> callback) => HelperUtility.SubscribeTo(ref ChangeMusic, ref callback);
    public void UnsubscribeFromChangeMusic(UnityAction<float> callback) => HelperUtility.UnsubscribeFrom(ref ChangeMusic, ref callback);
}
