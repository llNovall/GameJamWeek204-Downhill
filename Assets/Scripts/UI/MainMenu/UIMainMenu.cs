using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField]
    private Button _btnPlay, _btnCredits, _btnQuit, _btnBackCredits, _btnOptions;


    [SerializeField]
    private GameObject _objMainMenu, _objCredits, _objOptions;

    public void Start()
    {
        _btnPlay.onClick.AddListener(BtnPlay_OnClick);
        _btnCredits.onClick.AddListener(BtnCredits_OnClick);
        _btnQuit.onClick.AddListener(BtnQuit_OnClick);
        _btnBackCredits.onClick.AddListener(BtnBackCredits_OnClick);
        _btnOptions.onClick.AddListener(BtnOptions_OnClick);
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
        _objMainMenu.SetActive(false);
    }
    private void BtnCredits_OnClick()
    {
        _objCredits.SetActive(true);
        _objMainMenu.SetActive(false);
    }

    private void BtnPlay_OnClick()
    {
        GameStateManager.Current.ChangeGameState(GameState.Play);
        SceneManager.LoadScene(1);
    }
}
