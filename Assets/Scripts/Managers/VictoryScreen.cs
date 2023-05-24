using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] private float waitForAnyKey = 2f;
    [SerializeField] private TextMeshProUGUI anyKeyText;
    [SerializeField] private string mainMenuScene;

    private void Start()
    {
        Time.timeScale = 1f;
        anyKeyText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (waitForAnyKey > 0)
        {
            waitForAnyKey -= Time.deltaTime;
            if (waitForAnyKey <= 0)
            {
                anyKeyText.gameObject.SetActive(true);
            }
            else
            {
                if (Input.anyKeyDown)
                {
                    SceneManager.LoadScene(mainMenuScene);
                }
            }
        }
    }
}