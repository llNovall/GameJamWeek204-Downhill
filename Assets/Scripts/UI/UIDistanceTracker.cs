using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDistanceTracker : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _txtDistance;

    private void Start()
    {
        PlayerDistanceTracker.Current.SubscribeToOnDistanceUpdated(PlayerDistanceTracker_OnDistanceUpdated);
    }

    private void PlayerDistanceTracker_OnDistanceUpdated(float distance)
    {
        _txtDistance.text = Mathf.RoundToInt(distance).ToString();
    }
}
