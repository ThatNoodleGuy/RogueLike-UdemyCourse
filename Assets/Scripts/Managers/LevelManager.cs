using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; private set; }

    public event EventHandler OnLevelExit;
    public event EventHandler OnCoinsChanged;

    [SerializeField] private float waitToLoad = 4f;
    [SerializeField] private string nextLevel;
    [SerializeField] private string firstLevelScene;
    [SerializeField] private string mainMenuScene;
    [SerializeField] private bool isPaused;
    [SerializeField] private int currentCoins;

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

        currentCoins = Mathf.Clamp(currentCoins, 0, int.MaxValue);
    }

    public IEnumerator LevelEnd()
    {
        OnLevelExit?.Invoke(this, EventArgs.Empty);
        PlayerController.instance.PlayerCantMove();
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

    public void GetCoins(int amount)
    {
        OnCoinsChanged?.Invoke(this, EventArgs.Empty);
        currentCoins += amount;
    }

    public void SpendCoins(int amount)
    {
        currentCoins -= amount;
        OnCoinsChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetCurrentCoins()
    {
        return currentCoins;
    }
}