using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [SerializeField] private AudioSource levelMusic, gameOverMusic, gameWinMusic;
    [SerializeField] private AudioSource[] sfxClips;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PlayerHealth.instance.OnPlayerDamageTaken += PlayerHealth_OnPlayerDamageTaken;
        PlayerHealth.instance.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
        PlayerController.instance.OnPlayerDash += PlayerController_OnPlayerDash;
        PlayerController.instance.OnPlayerShooting += PlayerController_OnPlayerShooting;
        Breakable.OnObjectBreak += Breakable_OnObjectBreak;
        EnemyController.OnEnemyHealthChange += EnemyController_OnEnemyHealthChange;
        EnemyController.OnEnemyDeath += EnemyController_OnEnemyDeath;
        EnemyController.OnEnemyShooting += EnemyController_OnEnemyShooting;
        PlayerBulletController.OnBulletImpact += PlayerBulletController_OnBulletImpact;
        EnemyBulletController.OnBulletImpact += EnemyBulletController_OnBulletImpact;
        HealthPickup.OnHealthPickup += HealthPickup_OnHealthPickup;
        LevelManager.instance.OnLevelExit += LevelManager_OnLevelExit;
        CoinPickup.OnCoinPickup += CoinPickup_OnCoinPickup;
        ShopItem.OnBuyItem += ShopItem_OnBuyItem;
        ShopItem.OnCanNotBuy += ShopItem_OnCanNotBuy;
    }

    private void ShopItem_OnCanNotBuy(object sender, EventArgs e)
    {
        PlaySFX("Shop Not Enough");
    }

    private void ShopItem_OnBuyItem(object sender, EventArgs e)
    {
        PlaySFX("Shop Buy");
    }

    private void CoinPickup_OnCoinPickup(object sender, EventArgs e)
    {
        PlaySFX("Pickup Coin");
    }

    private void LevelManager_OnLevelExit(object sender, EventArgs e)
    {
        PlayLevelWinMusic();
    }

    private void EnemyController_OnEnemyShooting(object sender, EventArgs e)
    {
        int randomNum = UnityEngine.Random.Range(12, 18);
        PlaySFXNum(randomNum);
    }

    private void PlayerController_OnPlayerShooting(object sender, EventArgs e)
    {
        int randomNum = UnityEngine.Random.Range(12, 18);
        PlaySFXNum(randomNum);
    }

    private void HealthPickup_OnHealthPickup(object sender, EventArgs e)
    {
        PlaySFX("Pickup Health");
    }

    private void EnemyBulletController_OnBulletImpact(object sender, EventArgs e)
    {
        PlaySFX("Impact");
    }

    private void PlayerBulletController_OnBulletImpact(object sender, EventArgs e)
    {
        PlaySFX("Impact");
    }

    private void PlayerController_OnPlayerDash(object sender, EventArgs e)
    {
        PlaySFX("Player Dash");
    }

    private void PlayerHealth_OnPlayerDamageTaken(object sender, EventArgs e)
    {
        PlaySFX("Player Hurt");
    }

    private void EnemyController_OnEnemyDeath(object sender, EventArgs e)
    {
        PlaySFX("Enemy Death");
    }

    private void EnemyController_OnEnemyHealthChange(object sender, EventArgs e)
    {
        PlaySFX("Enemy Hurt");
    }

    private void Breakable_OnObjectBreak(object sender, EventArgs e)
    {
        PlaySFX("Box Breaking");
    }

    private void PlayerHealth_OnPlayerDeath(object sender, EventArgs e)
    {
        PlaySFX("Player Death");

        PlayGameOverMusic();
    }

    public void PlayGameOverMusic()
    {
        levelMusic.Stop();
        gameOverMusic.Play();
    }

    public void PlayLevelWinMusic()
    {
        levelMusic.Stop();
        gameWinMusic.Play();
    }

    public void PlaySFX(string name)
    {
        foreach (AudioSource audioSource in sfxClips)
        {
            if (audioSource.name == name)
            {
                audioSource.Stop();
                audioSource.Play();
                break;
            }
        }
    }

    public void PlaySFXNum(int sfxNum)
    {
        sfxClips[sfxNum].Stop();
        sfxClips[sfxNum].Play();
    }

    private void OnDestroy()
    {
        PlayerHealth.instance.OnPlayerDamageTaken -= PlayerHealth_OnPlayerDamageTaken;
        PlayerHealth.instance.OnPlayerDeath -= PlayerHealth_OnPlayerDeath;
        PlayerController.instance.OnPlayerDash -= PlayerController_OnPlayerDash;
        PlayerController.instance.OnPlayerShooting -= PlayerController_OnPlayerShooting;
        Breakable.OnObjectBreak -= Breakable_OnObjectBreak;
        EnemyController.OnEnemyHealthChange -= EnemyController_OnEnemyHealthChange;
        EnemyController.OnEnemyDeath -= EnemyController_OnEnemyDeath;
        EnemyController.OnEnemyShooting -= EnemyController_OnEnemyShooting;
        PlayerBulletController.OnBulletImpact -= PlayerBulletController_OnBulletImpact;
        EnemyBulletController.OnBulletImpact -= EnemyBulletController_OnBulletImpact;
        HealthPickup.OnHealthPickup -= HealthPickup_OnHealthPickup;
    }
}