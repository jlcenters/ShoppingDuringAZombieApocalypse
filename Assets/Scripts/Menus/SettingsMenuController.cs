using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class SettingsMenuController : MonoBehaviour
{
    //audio settings
    //fullscreen or windowed
    //resolution
    //screen aspect ratio
    //FOV
    //key rebinding

    [SerializeField] private Button returnToMainMenuButton;

    [SerializeField] private GameObject mainMenuObject;


    private void Awake()
    {
        returnToMainMenuButton.onClick.AddListener(() =>
        {
            //invoke toggle settings menu event
            this.gameObject.SetActive(false);
            mainMenuObject.SetActive(true);

        });
    }
}
