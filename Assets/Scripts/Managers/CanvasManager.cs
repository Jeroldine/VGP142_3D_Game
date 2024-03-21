using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] GameObject winMenu;

    [Header("Buttons")]
    [SerializeField] Button startButton;
    [SerializeField] Button resumeButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button quitButton;

    [Header("Counters")]
    [SerializeField] GameObject HPCounter;

    Animator[] HPCounterArr;

    void Start()
    {
        if (resumeButton)
            resumeButton.onClick.AddListener(Resume);
        if (quitButton)
            quitButton.onClick.AddListener(Quit);

        // end of level event
        GameManager.Instance.OnLevelEndArrival.AddListener((value) => ShowGameOverScreen(value));

        // HUD
        if (HPCounter)
        {
            HPCounterArr = GetAnimators(HPCounter);
            GameManager.Instance.OnHPValueChanged.AddListener((value) => UpdateHP(value));
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !GameManager.Instance.isPaused)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            GameManager.Instance.isPaused = true;
        }
    }

    private void UpdateHP(int value)
    {
        for (int i = 0; i < HPCounterArr.Length; i++)
        {
            if (i < value)
                HPCounterArr[i].SetTrigger("hasHealth");
            else
                HPCounterArr[i].SetTrigger("lostHealth");
        }
    }

    private void Resume()
    {
        Time.timeScale = 1;
        GameManager.Instance.isPaused = false;
        pauseMenu.SetActive(false);
    }

    private void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    private void ShowGameOverScreen(bool value)
    {
        gameOverMenu.SetActive(value);
        Debug.Log("Canvas Manager has shown the game over screen");
    }

    private void ShowWinScreen(bool value)
    {
        winMenu.SetActive(value);
    }

    private Animator[] GetAnimators(GameObject go)
    {
        return go.GetComponentsInChildren<Animator>();
    }
}
