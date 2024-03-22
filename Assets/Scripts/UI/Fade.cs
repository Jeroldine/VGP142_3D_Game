using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Fade : MonoBehaviour
{
    [SerializeField] private float mod = 1;

    private bool _fadeIn = false;
    private bool _fadeOut = false;
    private CanvasGroup cg;

    public bool fadeIn
    {
        get => _fadeIn;
        set => _fadeIn = value;
    }

    public bool fadeOut
    {
        get => _fadeOut;
        set => _fadeOut = value;
    }

    void Start()
    {
        cg = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_fadeIn)
        {
            cg.alpha += mod * Time.deltaTime;
            if (cg.alpha >= 1)
            {
                cg.alpha = 1;
                _fadeIn = false;
            }
        }

        if (_fadeOut)
        {
            cg.alpha -= mod * Time.deltaTime;
            if (cg.alpha <= 0)
            {
                cg.alpha = 0;
                _fadeOut = false;
            }
        }
    }

    public void SetInteractable(bool val)
    {
        cg.interactable = val;
    }
}
