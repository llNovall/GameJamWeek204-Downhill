using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField]
    private Button _btnStart, _btnPlay, _btnCredits, _btnQuit, _btnBackCredits, _btnOptions,_btnInstructions;


    [SerializeField]
    private GameObject _objStart, _objMainMenu, _objCredits, _objOptions, _objInstructions;

    public void Start()
    {
        GameMusicPlayer.Current.StopMusic();

        _btnStart.onClick.AddListener(BtnStart_OnClick);
        _btnPlay.onClick.AddListener(BtnPlay_OnClick);
        _btnCredits.onClick.AddListener(BtnCredits_OnClick);
        _btnInstructions.onClick.AddListener(BtnInstructions_OnClick);
        _btnQuit.onClick.AddListener(BtnQuit_OnClick);
        //_btnBackCredits.onClick.AddListener(BtnBackCredits_OnClick);
        _btnOptions.onClick.AddListener(BtnOptions_OnClick);
    }

    private void BtnInstructions_OnClick()
    {
        _objInstructions.SetActive(true);
        _objOptions.SetActive(false);
        _objCredits.SetActive(false);
    }

    private void BtnStart_OnClick()
    {
        GameMusicPlayer.Current.PlayMainMenuMusic();
        _objStart.SetActive(false);
        _objMainMenu.SetActive(true);
    }

    private void BtnBackCredits_OnClick()
    {
        _objCredits.SetActive(false);
        _objMainMenu.SetActive(true);
    }

    private void BtnQuit_OnClick()
    {
        Application.Quit();
    }

    private void BtnOptions_OnClick()
    {
        _objOptions.SetActive(true);
        _objInstructions.SetActive(false);
        _objCredits.SetActive(false);
    }
    private void BtnCredits_OnClick()
    {
        _objOptions.SetActive(false);
        _objCredits.SetActive(true);
        _objInstructions.SetActive(false);
        //_objMainMenu.SetActive(false);
    }

    private void BtnPlay_OnClick()
    {
        GameStateManager.Current.ChangeGameState(GameState.Play);
        SceneManager.LoadScene(1);
    }
}
