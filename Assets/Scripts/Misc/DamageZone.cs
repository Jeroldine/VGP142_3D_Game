using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] float dmgCycle = 2.0f;
    private float dmgTimer = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            GameManager.Instance.currentHP -= damage;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dmgTimer += Time.deltaTime;
            if (dmgTimer >= dmgCycle)
            {
                GameManager.Instance.currentHP -= damage;
                dmgTimer = 0;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            dmgTimer = 0;
    }
}
