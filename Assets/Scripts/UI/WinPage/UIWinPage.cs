using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIWinPage : MonoBehaviour
{
    [SerializeField]
    private GameObject _objUI, _objWinPage;

    [SerializeField]
    private TextMeshProUGUI _txtScore;

    [SerializeField]
    private Button _btnMainMenu;
    private void Start()
    {
        TimeManager.Current.SubscribeToOnTimeFinished(TimeManager_OnTimeFinished);
        _btnMainMenu.onClick.AddListener(BtnMainMenu_OnClick);
    }

    private void BtnMainMenu_OnClick()
    {
        Time.timeScale = 1;
        GameStateManager.Current.ChangeGameState(GameState.MainMenu);
        SceneManager.LoadScene(0);
    }

    private void TimeManager_OnTimeFinished()
    {
        _objUI.SetActive(false);
        _objWinPage.SetActive(true);

        Time.timeScale = 0;

        SpeedIncreaserBasedOnSize speedIncreaser = PlayerIdentifier.Current.GetComponent<SpeedIncreaserBasedOnSize>();
        _txtScore.text = $"You got a score of {Math.Round(speedIncreaser.GetSnowBallSize(), 2)}.";
    }
}
