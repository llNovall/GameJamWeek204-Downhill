using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPanner : MonoBehaviour
{
    [SerializeField]
    private Material _backgroundMat;

    [SerializeField]
    private float _offset;

    [SerializeField]
    private float _maxSpeed;

    [SerializeField]
    private bool _isPanningEnabled;

    [SerializeField]
    private Transform _player;

    [SerializeField]
    private Camera _mainCamera;

    private void Awake()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        _backgroundMat = meshRenderer.material;

    }
    private void Start()
    {
        _player = PlayerIdentifier.Current.transform;
        if (!_player)
            Debug.LogError($"{GetType().FullName} : Failed to find Player.");

        _mainCamera = Camera.main;

        TimeManager.Current.SubscribeToOnTimeFinished(PlayerProgressTracker_OnDestinationReached);
    }



    private void Update()
    {
        if (_isPanningEnabled)
        {
            _offset -= Time.deltaTime * _maxSpeed * GetSpeedScalerBasedOnPlayerYAxisLocation();

            _backgroundMat.SetTextureOffset("_MainTex", new Vector2(0, _offset));
        }
    }
    public void EnablePanning(bool isEnabled) => _isPanningEnabled = isEnabled;

    private float GetSpeedScalerBasedOnPlayerYAxisLocation()
    {
        if (_player)
        {
            Vector2 bottomLeftCorner = _mainCamera.ViewportToWorldPoint(new Vector2(0, 1));
            Vector2 topRightCorner = _mainCamera.ViewportToWorldPoint(new Vector2(1, 0));

            float maxY = bottomLeftCorner.y;
            float minY = topRightCorner.y;

            float remappedValue = _player.transform.position.y.MapValue(minY, maxY, 0, 1);
            return 1 - remappedValue;
        }
        else
        {
            Debug.LogError($"{GetType().FullName} : Player is missing.");
            return 0;
        }
    }
    private void PlayerProgressTracker_OnDestinationReached() => EnablePanning(false);
}
