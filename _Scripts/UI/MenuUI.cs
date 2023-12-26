using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MoreMountains.Feedbacks;
public class MenuUI : MonoBehaviour
{
    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
  
    public void StartGame()
    {
        SceneMgr.instance.StartGame();
    }

    public void Continue()
    {
        SceneMgr.instance.ContinueGmae();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void NextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(index);
    }

    public void PlayClockPressAudio()
    {
        SoundManager.PlayClockPressAudio();
    }
    public void PlayClockLooseAudio()
    {
        SoundManager.PlayClockLooseAudio();
    }
    public void PlayUIClickAudio()
    {
        SoundManager.PlayUIClickAudio();
    }
}
