using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInputModZone : MonoBehaviour
{
    [SerializeField] float moveInputMod = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().ChangeMoveInputModifier(moveInputMod);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().ChangeMoveInputModifier(1);
        }
    }
}
