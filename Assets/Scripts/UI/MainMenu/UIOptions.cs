using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIOptions : MonoBehaviour
{
    [SerializeField]
    private Slider _sldMusicVolume;

    private void Start()
    {
        _sldMusicVolume.onValueChanged.AddListener(new UnityAction<float>(c => VolumeManager.Current.SetMusic(c)));
    }
}
