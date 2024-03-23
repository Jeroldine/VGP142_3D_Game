using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineMotion : MonoBehaviour
{
    [SerializeField] private float amplitude = 1;
    [SerializeField] private float period = 2 * Mathf.PI;
    [SerializeField] private float shift = 0;

    private float b;
    private float t = 0;

    void Start()
    {
        b = (2 * Mathf.PI) / period;
    }

    // Update is called once per frame
    void Update()
    {
        t = t >= period ? 0 : (t += Time.deltaTime);

        float v = amplitude * Mathf.Cos(b * (t - shift));

        transform.Translate(0, v * Time.deltaTime, 0);
    }
}
