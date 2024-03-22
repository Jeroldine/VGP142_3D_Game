using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private float initSpeed = 3;
    [SerializeField] private float halfLife = 0.2f;
    [SerializeField] private float delay = 3;

    private float t = 0;
    private Transform cloudTransform;

    void Start()
    {
        cloudTransform = GetComponent<Transform>();
        Destroy(gameObject, delay);
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        float v = initSpeed * Mathf.Exp(-halfLife * t);
        Vector3 cloudV = new Vector3(0, v, 0);
        transform.Translate(cloudV * Time.deltaTime);
    }
}
