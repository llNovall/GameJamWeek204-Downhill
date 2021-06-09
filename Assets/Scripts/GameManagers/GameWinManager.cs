using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinManager : MonoBehaviour
{
    private void Start()
    {
        TimeManager.Current.SubscribeToOnTimeFinished(TimeManager_OnTimeFinished);
    }

    private void TimeManager_OnTimeFinished()
    {
        throw new NotImplementedException();
    }
}
