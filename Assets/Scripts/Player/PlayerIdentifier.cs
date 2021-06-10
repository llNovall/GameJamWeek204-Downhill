using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdentifier : MonoBehaviour
{
    public static PlayerIdentifier Current;
    public static PlayerEnergy PlayerEnergy;
    public static PlayerAnimation PlayerAnimation;
    public static PlayerSound PlayerSound;

    private void Awake()
    {
        Current = this;
    }

    private void Start()
    {
        PlayerEnergy = GetComponent<PlayerEnergy>();
        PlayerAnimation = GetComponent<PlayerAnimation>();
        PlayerSound = GetComponent<PlayerSound>();
    }
}
