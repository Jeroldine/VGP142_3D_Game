using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    public enum PickupType
    {
        health = 0,
        silkBundle = 1,
        currency = 2,

    }

    [SerializeField] PickupType currentPickup;
    [SerializeField] int hpAmount = 2;
    [SerializeField] int currencyAmount = 5;
    [SerializeField] AudioClip pickUpSound;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            //pc.PlayPickupSound(pickUpSound);
            switch (currentPickup)
            {
                case PickupType.health:
                    GameManager.Instance.currentHP += hpAmount;
                    break;
                case PickupType.silkBundle:
                    GameManager.Instance.currentSilkBundles++;
                    break;
                case PickupType.currency:
                    GameManager.Instance.currentCurrency += currencyAmount;
                    break;
            }
            Destroy(gameObject);
        }
    }
}
