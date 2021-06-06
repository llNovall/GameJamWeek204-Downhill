using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;

    private event UnityAction<float> OnPlayerSideWaysMovementInputUpdated;

    private void Start()
    {
        _playerInput = gameObject.AddComponent<PlayerInput>();
        _playerInput.actions = Resources.Load<InputActionAsset>("Controls");
        _playerInput.notificationBehavior = PlayerNotifications.InvokeUnityEvents;
        _playerInput.defaultControlScheme = "Keyboard & Mouse";

        foreach (InputAction item in _playerInput.actions)
            item.Enable();
    }

    private void FixedUpdate()
    {
        float input = _playerInput.actions["SideWays"].ReadValue<float>();
        //Debug.LogError(inputVector);
        if (input != 0)
            OnPlayerSideWaysMovementInputUpdated?.Invoke(input);

    }

    #region

    public void SubscribeToOnPlayerSideWaysMovementInputUpdated(UnityAction<float> callback) => HelperUtility.SubscribeTo(ref OnPlayerSideWaysMovementInputUpdated, ref callback);
    public void UnsubscribeFromOnPlayerSideWaysMovementInputUpdated(UnityAction<float> callback) => HelperUtility.UnsubscribeFrom(ref OnPlayerSideWaysMovementInputUpdated, ref callback);

    #endregion
}
