using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; private set; }

    public event EventHandler OnLevelExit;

    [SerializeField] private float waitToLoad = 4f;
    [SerializeField] private string nextLevel;
    [SerializeField] private string firstLevelScene;
    [SerializeField] private string mainMenuScene;
    [SerializeField] private bool isPaused;

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public IEnumerator LevelEnd()
    {
        OnLevelExit?.Invoke(this, EventArgs.Empty);
        PlayerController.instance.ToggleCanMove();
        UIController.instance.StartFadeToBlack();
        yield return new WaitForSeconds(waitToLoad);
        SceneManager.LoadScene(nextLevel);
    }

    public void StartNewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(firstLevelScene);
    }

    public void LoadMainMenuScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void PauseUnpause()
    {
        if (!isPaused)
        {
            UIController.instance.GetPauseMenuObject().SetActive(true);
            isPaused = true;
            Time.timeScale = 0f;
        }
        else
        {
            UIController.instance.GetPauseMenuObject().SetActive(false);
            isPaused = false;
            Time.timeScale = 1f;
        }
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}