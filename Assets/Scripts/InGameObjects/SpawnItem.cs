using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    [SerializeField] Pickups salvePrefab;
    [SerializeField] Pickups silkBundlePrefab;
    [SerializeField] Pickups currencyPrefab;
    int randomPickup;

    void Start()
    {
        randomPickup = Random.Range(0, 4);
        //Debug.Log(randomPickup);
        switch (randomPickup)
        {
            case 0:
                Instantiate(salvePrefab, transform.position, transform.rotation);
                break;
            case 1:
                Instantiate(silkBundlePrefab, transform.position, transform.rotation);
                break;
            case 2:
                Instantiate(currencyPrefab, transform.position, transform.rotation);
                break;
            case 3:
                break;

        }
    }
}
