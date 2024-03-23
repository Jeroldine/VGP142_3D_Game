using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something Entered the hitbox");
        if (other.CompareTag("Enemy") && gameObject.CompareTag("Player"))
        {
             other.GetComponent<EnemyManager>().TakeDamage(1);
        }

        if (other.CompareTag("Player") && gameObject.CompareTag("Enemy"))
        {
            GameManager.Instance.currentHP -= 1;
        }
    }

}
