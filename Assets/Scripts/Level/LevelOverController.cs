using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelOverController : MonoBehaviour
{
    public GameObject completeLevelUI;
    public Button buttonMainMenu;

    private void Awake()
    {
        buttonMainMenu.onClick.AddListener(MainMenu);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>() != null)
        {
            Debug.Log("Level Finished by the player");
            SoundManager.Instance.Play(Sounds.PlayerLevelWinVoice);
            SoundManager.Instance.PlayMusic(Sounds.DoorOpening);
            LevelComplete();
            LevelManager.Instance.MarkCurrentLevelComplete();
        }
    }

    public void LevelComplete()
    {
        completeLevelUI.SetActive(true);
    }
    public void MainMenu()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        Debug.Log("Loading scene 0");
        SceneManager.LoadScene(0);  
    }
}
