using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinishAreaManager : MonoBehaviour
{
    public static FinishAreaManager Current;

    [SerializeField]
    private FinishArea _finishArea;

    private event UnityAction OnFinishingLevel;

    private void Awake()
    {
        Current = this;

        if (_finishArea)
        {
            _finishArea.SubscribeToOnLanded(WinLandArea_OnLanded);
        }
        else
            Debug.LogError($"{GetType().FullName} : Failed to find WinLandArea");
    }
    private void WinLandArea_OnLanded() => OnFinishingLevel?.Invoke();

    #region Event Subscription
    public void SubscribeToOnLandingInWinArea(UnityAction callback) => HelperUtility.SubscribeTo(ref OnFinishingLevel, ref callback);
    public void UnsubscribeFromOnLandingInWinArea(UnityAction callback) => HelperUtility.UnsubscribeFrom(ref OnFinishingLevel, ref callback);
    #endregion
}
