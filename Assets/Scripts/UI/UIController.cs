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
    [SerializeField] private GameObject deathScreen;

    private void Awake()
    {
        instance = this;
        deathScreen.gameObject.SetActive(false);
    }

    private void Start()
    {
        PlayerHealth.instance.OnPlayerHealthChange += PlayerHealth_OnPlayerHealthChange;
        PlayerHealth.instance.OnPlayerDeath += PlayerHealth_OnPlayerDeath;

        UpdatePlayerHealthSlider();
        UpdatePlayerHealthText();
    }

    private void PlayerHealth_OnPlayerDeath(object sender, EventArgs e)
    {
        deathScreen.gameObject.SetActive(true);
    }

    private void PlayerHealth_OnPlayerHealthChange(object sender, EventArgs e)
    {
        UpdatePlayerHealthSlider();
        UpdatePlayerHealthText();
    }

    public void UpdatePlayerHealthSlider()
    {
        playerHealthSlider.maxValue = PlayerHealth.instance.GetMaxhealth();
        playerHealthSlider.value = PlayerHealth.instance.GetCurrenthealth();
    }

    public void UpdatePlayerHealthText()
    {
        playerHealthText.text = (PlayerHealth.instance.GetCurrenthealth() + "/" + PlayerHealth.instance.GetMaxhealth());
    }

    private void OnDestroy()
    {
        PlayerHealth.instance.OnPlayerHealthChange -= PlayerHealth_OnPlayerHealthChange;
        PlayerHealth.instance.OnPlayerDeath -= PlayerHealth_OnPlayerDeath;
    }
}