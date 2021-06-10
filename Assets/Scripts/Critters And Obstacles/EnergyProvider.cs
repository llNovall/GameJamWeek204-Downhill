using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnergyProvider : MonoBehaviour
{
    //[SerializeField]
    //private bool _isHit;

    [SerializeField]
    private float _energyProvidedWhenHit;

    private event UnityAction OnHit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerIdentifier.PlayerEnergy.AddEnergy(_energyProvidedWhenHit);
            gameObject.SetActive(false);
            //_isHit = true;
        }
        //if (!_isHit)
        //{
            
        //}
    }

    private void OnBecameInvisible()
    {
        //_isHit = false;
    }

    public void SubscribeToOnHit(UnityAction callback) => HelperUtility.SubscribeTo(ref OnHit, ref callback);
    public void UnsubscribeFromOnHit(UnityAction callback) => HelperUtility.UnsubscribeFrom(ref OnHit, ref callback);

}
