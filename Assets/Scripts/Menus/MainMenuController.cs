using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button SettingsButton;
    [SerializeField] private Button QuitGameButton;

    [SerializeField] private GameObject mainMenuObject;
    [SerializeField] private GameObject settingsMenuObject;

    private void Awake()
    {
        startGameButton.onClick.AddListener(() =>
        {
            Loader.Load(GameScenes.SampleScene);
        });
        SettingsButton.onClick.AddListener(() =>
        {
            //open settings menu
            mainMenuObject.SetActive(false);
            settingsMenuObject.SetActive(true);
        });
        QuitGameButton.onClick.AddListener(() =>
        {
            Debug.Log("Goodbye!");
            Application.Quit();
        });

    }
}
