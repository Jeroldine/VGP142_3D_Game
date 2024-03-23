using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinMotion : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 120;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
