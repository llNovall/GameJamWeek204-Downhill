using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISnowBallSize : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _txtSnowBallSize;

    private void Start()
    {
        SpeedIncreaserBasedOnSize speedIncreaser = PlayerIdentifier.Current.GetComponent<SpeedIncreaserBasedOnSize>();
        speedIncreaser.SubscribeToOnSnowBallSizeChanged(SpeedIncreaserBasedOnSize_OnSnowBallSizeChanged);
    }

    private void SpeedIncreaserBasedOnSize_OnSnowBallSizeChanged(float size)
    {
        _txtSnowBallSize.text = Math.Round(size, 2).ToString();
    }
}
