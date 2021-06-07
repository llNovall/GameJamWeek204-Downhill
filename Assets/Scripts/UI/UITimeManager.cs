using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITimeManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _txtTimeRemaining;

    private void Start()
    {
        TimeManager.Current.SubscribeToOnProgressUpdated(TimeManager_OnProgressUpdated);
        TimeManager.Current.SubscribeToOnTimeFinished(TimeManager_OnTimeFinished);
    }

    private void TimeManager_OnTimeFinished()
    {
        _txtTimeRemaining.text = "0";
    }

    private void TimeManager_OnProgressUpdated(float progress)
    {
        _txtTimeRemaining.text = Mathf.RoundToInt(progress).ToString();
    }
}
