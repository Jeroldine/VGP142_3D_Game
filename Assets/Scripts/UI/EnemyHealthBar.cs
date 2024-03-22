using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    private Camera cam;
    private Slider slider;

    private void Start()
    {
        cam = Camera.main;
        slider = GetComponent<Slider>();
        if (!slider) Debug.Log("No HealthBar slider");
    }

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.rotation = cam.transform.rotation;
        transform.position = transform.parent.position + offset;
    }
}
