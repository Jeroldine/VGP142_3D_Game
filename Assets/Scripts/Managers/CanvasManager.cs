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
    [SerializeField] GameObject notesScreen;

    [Header("Buttons")]
    [SerializeField] Button startButton;
    [SerializeField] Button loadButton;
    [SerializeField] Button resumeButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button quitButton;
    [SerializeField] Button notesButton;

    [Header("Counters")]
    [SerializeField] GameObject HPCounter;

    [Header("Faders")]
    [SerializeField] Fade fader;

    Animator[] HPCounterArr;

    // Coroutines
    private Coroutine StartTransitionCR;
    private Coroutine LoadTransitionCR;
    private Coroutine GameOverTransitionCR;

    void Start()
    {
        if (startButton)
            startButton.onClick.AddListener(StartGame);
        if (loadButton)
            loadButton.onClick.AddListener(LoadGame);
        if (resumeButton)
            resumeButton.onClick.AddListener(Resume);
        if (quitButton)
            quitButton.onClick.AddListener(Quit);
        if (notesButton)
            notesButton.onClick.AddListener(ShowNotes);

        GameManager.Instance.TestGameManager();

        // end of level event
        GameManager.Instance.OnLevelEndArrival.AddListener((value) => ShowGameOverScreen(value));

        // player death event
        GameManager.Instance.OnPlayerDeath.AddListener((value) => GoToGameOverScene(value));

        // HUD
        if (HPCounter)
        {
            HPCounterArr = GetAnimators(HPCounter);
            UpdateHP(GameManager.Instance.currentHP);
            GameManager.Instance.OnHPValueChanged.AddListener((value) => UpdateHP(value));
        }


        StartCoroutine(ShowGameScreenTransition());

    }

    private void StartGame()
    {
        Debug.Log("Start button pressed");
        if (StartTransitionCR == null)
            StartTransitionCR = StartCoroutine(StartGameTransition());
    }

    private void LoadGame()
    {
        if (LoadTransitionCR == null)
            LoadTransitionCR = StartCoroutine(LoadGameTransition());
    }

    private void GoToGameOverScene(bool val)
    {
        if (GameOverTransitionCR == null)
            GameOverTransitionCR = StartCoroutine(GameOverTransition());
    }

    // Update is called once per frame
    void Update()
    {
        if (!pauseMenu) return;

        if (Input.GetKeyDown(KeyCode.P) && !GameManager.Instance.isPaused)
        {
            notesScreen.SetActive(false);
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

    private void ShowNotes()
    {
        if (notesScreen.activeInHierarchy)
            notesScreen.SetActive(false);
        else
            notesScreen.SetActive(true);
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

    IEnumerator StartGameTransition()
    {
        fader.fadeIn = true;
        fader.SetInteractable(true);
        yield return new WaitForSeconds(1);
        StartTransitionCR = null;
        GameManager.Instance.LoadLevel(1);
    }

    IEnumerator LoadGameTransition()
    {
        //if (GameManager.Instance.isDead)
        //    yield return new WaitForSeconds(2.5f);
            
        fader.fadeIn = true;
        fader.SetInteractable(true);
        yield return new WaitForSeconds(1);
        LoadTransitionCR = null;
        GameManager.Instance.Respawn();
        GameManager.Instance.LoadLevel(1);
    }

    IEnumerator GameOverTransition()
    {
        yield return new WaitForSeconds(2.5f);
        fader.fadeIn = true;
        yield return new WaitForSeconds(1.5f);
        GameOverTransitionCR = null;
        GameManager.Instance.LoadLevel(2);
    }

    IEnumerator ShowGameScreenTransition()
    {
        //fader.SetInteractable(true);
        yield return new WaitForSeconds(2.5f);

        fader.fadeOut = true;

        //fader.SetInteractable(false);
    }
}
