using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    [SerializeField]
    private Button buttonPlay;
    [SerializeField]
    private Button buttonQuit;

    private void Awake()
    {
        buttonPlay.onClick.AddListener(PlayGame);
        buttonPlay.onClick.AddListener(QuitGame);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
}
