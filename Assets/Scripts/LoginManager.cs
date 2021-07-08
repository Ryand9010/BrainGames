using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class LoginManager : MonoBehaviour
{

    public TMP_InputField inputField;
    public TMP_Text displayNameText;


    private void Start()
    {
        displayNameText.text = ("Hello " + PlayerPrefs.GetString("DisplayName") + ", Select a Game Below");
        Debug.Log(PlayerPrefs.GetString("MatchingLeaderboard"));
    }

    public void Login()
    {
        string displayName = inputField.text;
        Debug.Log(displayName);
        if (displayName != "")
        {
            PlayerPrefs.SetString("DisplayName", displayName);
            SceneManager.LoadScene("MainMenuScene");
        }
        else
        {

        }

    }
}
