using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Enemy") && gameObject.CompareTag("Player"))
        {
            Debug.Log("Something Entered the hitbox");
            Debug.Log("Player hit enemy");
            other.GetComponent<EnemyManager>().TakeDamage(1);
        }

        if (other.CompareTag("Player") && gameObject.CompareTag("Enemy"))
        {
            GameManager.Instance.currentHP -= 1;
        }
    }

}
