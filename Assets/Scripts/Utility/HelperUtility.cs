using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public static class HelperUtility
{
    public static void SubscribeTo(ref UnityAction eventToSubscribe, ref UnityAction callback)
    {
        if (eventToSubscribe == null)
            eventToSubscribe += callback;
        else
        {
            Delegate[] invocationList = eventToSubscribe.GetInvocationList();

            if (!invocationList.Contains(callback))
                eventToSubscribe += callback;
        }
    }

    public static void UnsubscribeFrom(ref UnityAction eventToUnsubscribe, ref UnityAction callback)
    {
        if (eventToUnsubscribe != null)
        {
            Delegate[] invocationList = eventToUnsubscribe.GetInvocationList();

            if (invocationList.Contains(callback))
                eventToUnsubscribe -= callback;
        }
    }
    public static void SubscribeTo<T>(ref UnityAction<T> eventToSubscribe, ref UnityAction<T> callback)
    {
        if (eventToSubscribe == null)
            eventToSubscribe += callback;
        else
        {
            Delegate[] invocationList = eventToSubscribe.GetInvocationList();

            if (!invocationList.Contains(callback))
                eventToSubscribe += callback;
        }
    }

    public static void UnsubscribeFrom<T>(ref UnityAction<T> eventToUnsubscribe, ref UnityAction<T> callback)
    {
        if (eventToUnsubscribe != null)
        {
            Delegate[] invocationList = eventToUnsubscribe.GetInvocationList();

            if (invocationList.Contains(callback))
                eventToUnsubscribe -= callback;
        }
    }
    public static void SubscribeTo<T, V>(ref UnityAction<T, V> eventToSubscribe, ref UnityAction<T, V> callback)
    {
        if (eventToSubscribe == null)
            eventToSubscribe += callback;
        else
        {
            Delegate[] invocationList = eventToSubscribe.GetInvocationList();

            if (!invocationList.Contains(callback))
                eventToSubscribe += callback;
        }
    }

    public static void UnsubscribeFrom<T, V>(ref UnityAction<T, V> eventToUnsubscribe, ref UnityAction<T, V> callback)
    {
        if (eventToUnsubscribe != null)
        {
            Delegate[] invocationList = eventToUnsubscribe.GetInvocationList();

            if (invocationList.Contains(callback))
                eventToUnsubscribe -= callback;
        }
    }

    public static float MapValue(this float value, float oldMin, float oldMax, float newMin, float newMax)
    {
        return newMin + (value - oldMin) * (newMax - newMin) / (oldMax - oldMin);
    }
}
