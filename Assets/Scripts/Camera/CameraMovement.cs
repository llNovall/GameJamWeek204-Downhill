using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Transform _player;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private float _offset;

    private void Start()
    {
        _player = PlayerIdentifier.Current.transform;
    }

    private void LateUpdate()
    {
        Vector3 newPosition = _camera.transform.position;
        newPosition.y = _player.position.y + _offset;
        _camera.transform.position = newPosition;
    }
}
