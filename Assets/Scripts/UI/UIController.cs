using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance { get; private set; }

    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private TextMeshProUGUI playerCoinText;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private Image fadeScreen;
    [SerializeField] private float levelFadeTime;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mapDisplay;
    [SerializeField] private TextMeshProUGUI mapExitText;

    private bool fadeToBlack, fadeOutBlack;
    private int playerMaxHealth = 0, playerCurrentHealth = 0;
    private int currentCoins = 0;

    private void Awake()
    {
        instance = this;
        deathScreen.gameObject.SetActive(false);
        pauseMenu.SetActive(false);
    }

    private void Start()
    {
        fadeOutBlack = true;
        fadeToBlack = false;

        mapExitText.gameObject.SetActive(false);

        playerCurrentHealth = PlayerHealth.instance.GetCurrentHealth();
        playerMaxHealth = PlayerHealth.instance.GetMaxHealth();
        currentCoins = LevelManager.instance.GetCurrentCoins();
        UpdatePlayerHealthBarAndText();
        UpdatePlayerCoinsText();

        PlayerHealth.instance.OnPlayerDamageTaken += PlayerHealth_OnPlayerDamageTaken;
        PlayerHealth.instance.OnPlayerHeal += PlayerHealth_OnPlayerHeal;
        PlayerHealth.instance.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
        PlayerHealth.instance.OnPlayerHealthUpgrade += PlayerHealth_OnPlayerHealthUpgrade;
        CoinPickup.OnCoinPickup += CoinPickup_OnCoinPickup;
        LevelManager.instance.OnCoinsChanged += LevelManager_OnCoinsChanged;
    }

    private void PlayerHealth_OnPlayerHealthUpgrade(object sender, EventArgs e)
    {
        UpdatePlayerHealthBarAndText();
    }

    private void LevelManager_OnCoinsChanged(object sender, EventArgs e)
    {
        UpdatePlayerCoinsText();
    }

    private void CoinPickup_OnCoinPickup(object sender, EventArgs e)
    {
        UpdatePlayerCoinsText();
    }

    private void Update()
    {
        Fade();
    }

    private void PlayerHealth_OnPlayerHeal(object sender, EventArgs e)
    {
        UpdatePlayerHealthBarAndText();
    }

    private void PlayerHealth_OnPlayerDeath(object sender, EventArgs e)
    {
        deathScreen.gameObject.SetActive(true);
    }

    private void PlayerHealth_OnPlayerDamageTaken(object sender, EventArgs e)
    {
        UpdatePlayerHealthBarAndText();
    }

    public void UpdatePlayerHealthBarAndText()
    {
        playerHealthSlider.maxValue = PlayerHealth.instance.GetMaxHealth();
        playerHealthSlider.value = PlayerHealth.instance.GetCurrentHealth();
        playerHealthText.text = (PlayerHealth.instance.GetCurrentHealth() + "/" + PlayerHealth.instance.GetMaxHealth());
    }

    public void UpdatePlayerCoinsText()
    {
        currentCoins = LevelManager.instance.GetCurrentCoins();

        playerCoinText.text = currentCoins.ToString();
    }

    private void OnDestroy()
    {
        PlayerHealth.instance.OnPlayerDamageTaken -= PlayerHealth_OnPlayerDamageTaken;
        PlayerHealth.instance.OnPlayerHeal -= PlayerHealth_OnPlayerHeal;
        PlayerHealth.instance.OnPlayerDeath -= PlayerHealth_OnPlayerDeath;
        CoinPickup.OnCoinPickup -= CoinPickup_OnCoinPickup;
    }

    private void Fade()
    {
        if (fadeOutBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, levelFadeTime * Time.deltaTime));
            if (fadeScreen.color.a == 0f)
            {
                fadeOutBlack = false;
            }
        }

        if (fadeToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, levelFadeTime * Time.deltaTime));
            if (fadeScreen.color.a == 1f)
            {
                fadeToBlack = false;
            }
        }
    }

    public void StartFadeToBlack()
    {
        fadeToBlack = true;
        fadeOutBlack = false;
    }

    public GameObject GetPauseMenuObject()
    {
        return pauseMenu;
    }

    public void Resume()
    {
        LevelManager.instance.PauseUnpause();
    }

    public GameObject GetMapDisplay()
    {
        return mapDisplay;
    }

    public GameObject GetBigMapTextGameObject()
    {
        return mapExitText.gameObject;
    }
}