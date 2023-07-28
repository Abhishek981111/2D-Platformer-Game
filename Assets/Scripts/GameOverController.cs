using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    public Button buttonRestart;
    public Button buttonMainMenu;
    public ParticleController particleController;
    
    public void Awake()
    {
        buttonRestart.onClick.AddListener(ReloadLevel);
        buttonMainMenu.onClick.AddListener(MainMenu);
    }
    public void PlayerDied()
    {
        SoundManager.Instance.PlayMusic(Sounds.PlayerDeath);
        gameObject.SetActive(true);
        particleController.PlayOnLevelFail();
    }
    public void ReloadLevel()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        Debug.Log("Reloading Scene...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);      //Can also do with SceneManager.LoadScene(0);
    }
    public void MainMenu()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        Debug.Log("Loading scene 0");
        SceneManager.LoadScene(0);  
    }
}
