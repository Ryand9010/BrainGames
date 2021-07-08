using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour
{
    
  public void LoadScene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }

    public void ResetGameSettings()
    {
        MatchingSettings.Instance.ResetMemoryGameSettings();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
