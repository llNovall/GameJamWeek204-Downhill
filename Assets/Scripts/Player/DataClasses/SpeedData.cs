using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SpeedData
{
    public float MinSpeed, MaxSpeed, ReverseSpeed;

    public float MinAccelerationSpeed, MaxAccelerationSpeed;

    public float MinReverseAcceleration, MaxReverseAcceleration;
}
