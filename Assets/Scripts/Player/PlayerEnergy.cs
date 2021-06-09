using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEnergy : MonoBehaviour
{
    [SerializeField]
    private float _energy, _maxEnergy;

    [SerializeField]
    private float _timeSinceEaten, _timeRequiredToChangeStateToNotEaten;

    [SerializeField]
    private PlayerEnergyState _energyState;

    [SerializeField]
    private bool _isEnabled;

    private event UnityAction<float> OnEnergyLerpUpdated;
    private event UnityAction<float> OnEnergyUpdated;
    private event UnityAction OnEnergyDepleted;
    private event UnityAction<PlayerEnergyState> OnEnergyStateChanged;

    private void Awake()
    {
        _energy = _maxEnergy;
    }

    private void Start()
    {
        OnEnergyUpdated?.Invoke(_energy);
    }
    private void Update()
    {
        if (_isEnabled)
        {
            switch (_energyState)
            {
                case PlayerEnergyState.Idle:
                case PlayerEnergyState.Eaten:
                    if(_timeSinceEaten >= _timeRequiredToChangeStateToNotEaten)
                    {
                        ChangeState(PlayerEnergyState.Hungry);
                        _timeSinceEaten = 0;
                    }
                    else
                    {
                        _timeSinceEaten += Time.deltaTime;
                    }
                    break;
                case PlayerEnergyState.Hungry:
                    break;
            }
        }
    }
    public void ResetEnergy()
    {
        _energy = _maxEnergy;
        _isEnabled = true;
        ChangeState(PlayerEnergyState.Eaten);
        OnEnergyUpdated?.Invoke(_energy);
    }
    public float GetMaxEnergy() => _maxEnergy;
    public void AddEnergy(float energyToAdd)
    {
        if (_isEnabled)
        {
            ChangeState(PlayerEnergyState.Eaten);
            _energy = Mathf.Min(_energy + energyToAdd, _maxEnergy);
            OnEnergyUpdated?.Invoke(_energy);
            OnEnergyLerpUpdated?.Invoke(_energy / _maxEnergy);
        }
    }

    public void RemoveEnergy(float energyToRemove)
    {
        if (_isEnabled)
        {
            if(_energyState == PlayerEnergyState.Hungry)
            {
                //Debug.LogError(energyToRemove);
                _energy = Mathf.Max(_energy - energyToRemove, 0);
                OnEnergyUpdated?.Invoke(_energy);
                OnEnergyLerpUpdated?.Invoke(_energy / _maxEnergy);

                if (_energy == 0)
                {
                    _isEnabled = true;
                    OnEnergyDepleted?.Invoke();
                }
            }
        }
    }

    private void ChangeState(PlayerEnergyState state)
    {
        if(_energyState != state)
        {
            _energyState = state;
            OnEnergyStateChanged?.Invoke(_energyState);
        }
    }
    public void SubscribeToOnEnergyUpdated(UnityAction<float> callback) => HelperUtility.SubscribeTo(ref OnEnergyUpdated, ref callback);
    public void UnsubscribeFromOnEnergyUpdated(UnityAction<float> callback) => HelperUtility.UnsubscribeFrom(ref OnEnergyUpdated, ref callback);

    public void SubscribeToOnEnergyDepleted(UnityAction callback) => HelperUtility.SubscribeTo(ref OnEnergyDepleted, ref callback);
    public void UnsubscribeFromOnEnergyDepleted(UnityAction callback) => HelperUtility.UnsubscribeFrom(ref OnEnergyDepleted, ref callback);

    public void SubscribeToOnEnergyLerpUpdated(UnityAction<float> callback) => HelperUtility.SubscribeTo(ref OnEnergyLerpUpdated, ref callback);
    public void UnsubscribeFromOnEnergyLerpUpdated(UnityAction<float> callback) => HelperUtility.UnsubscribeFrom(ref OnEnergyLerpUpdated, ref callback);

    public void SubscribeToOnEnergyStateChanged(UnityAction<PlayerEnergyState> callback) => HelperUtility.SubscribeTo(ref OnEnergyStateChanged, ref callback);
    public void UnsubscribeFromOnEnergyStateChanged(UnityAction<PlayerEnergyState> callback) => HelperUtility.UnsubscribeFrom(ref OnEnergyStateChanged, ref callback);

}
