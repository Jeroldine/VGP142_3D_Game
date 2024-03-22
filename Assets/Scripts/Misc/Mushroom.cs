using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    private bool emittedCloud = false;
    private float timer = 0;

    [SerializeField] private float emitCycle = 3;
    [SerializeField] private Cloud cloudPrefab;
    [SerializeField] private Transform cloudSpawn;

    // Update is called once per frame
    void Update()
    {
        if (emittedCloud)
        {
            timer += Time.deltaTime;
            if (timer >= emitCycle)
                emittedCloud = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enviro"))
        {
            if (!emittedCloud)
            {
                emittedCloud = true;
                Instantiate(cloudPrefab, cloudSpawn.position, cloudSpawn.rotation);
            }
        }
    }
}
