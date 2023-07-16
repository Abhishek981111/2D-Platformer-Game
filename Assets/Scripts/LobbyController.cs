using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{
    public Button buttonPlay;
    public Button buttonQuit;
    public GameObject levelSelection;

    private void Awake()
    {
        buttonPlay.onClick.AddListener(PlayGame);
        buttonQuit.onClick.AddListener(QuitGame);
    }

    private void PlayGame()
    {
        //SceneManager.LoadScene(1);
        levelSelection.SetActive(true);
    }

    private void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }
}
