using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapObject : MonoBehaviour
{
    [SerializeField]
    private int _index;

    [SerializeField]
    private bool _isEntered;

    public event UnityAction<int> OnEntered;

    public void SetIndex(int index) => _index = index;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isEntered)
        {
            _isEntered = true;
            OnEntered?.Invoke(_index);
        }
    }

    private void OnBecameInvisible()
    {
        _isEntered = false;
    }
}
