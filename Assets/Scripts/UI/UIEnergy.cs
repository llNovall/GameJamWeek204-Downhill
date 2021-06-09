using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnergy : MonoBehaviour
{
    [SerializeField]
    private Slider _sldEnergy;
    private void Start()
    {
        PlayerEnergy playerEnergy = PlayerIdentifier.Current.GetComponent<PlayerEnergy>();
        playerEnergy.SubscribeToOnEnergyUpdated(PlayerEnergy_OnEnergyUpdated);

        _sldEnergy.maxValue = playerEnergy.GetMaxEnergy();
    }

    private void PlayerEnergy_OnEnergyUpdated(float energy)
    {
        _sldEnergy.value = energy;
    }
}
