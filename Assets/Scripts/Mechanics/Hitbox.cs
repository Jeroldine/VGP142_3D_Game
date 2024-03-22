using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something Entered the hitbox");
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyManager>().TakeDamage(1);
        }
    }
}
