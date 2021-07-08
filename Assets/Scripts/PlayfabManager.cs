using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;

public class PlayfabManager : MonoBehaviour
{

    public GameObject rowPrefab;
    public Transform rowsParent;

    public GameObject loginPanel;
    public GameObject mainMenuPanel;
    public TMP_InputField usernameInput;

    // Start is called before the first frame update
    void Start()
    {
        //loginPanel.SetActive(true);
        //mainMenuPanel.SetActive(false);
        Login();

    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = "BOB",
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Successfull login/account created!");
        string username = null;

        if(result.InfoResultPayload.PlayerProfile != null)
        {
            username = result.InfoResultPayload.PlayerProfile.DisplayName;
        }

        //if(username == null)
        //{
        //    loginPanel.SetActive(true);

        //}
        //else
        //{
        //    mainMenuPanel.SetActive(true);

        //}
    }



    public void SubmitUsername()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = usernameInput.text
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);

        //Login();
        loginPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated display name");
        mainMenuPanel.SetActive(true);
        loginPanel.SetActive(false);
    }


    void OnError(PlayFabError  error)
    {
        Debug.Log(error.GenerateErrorReport());
    }



    //Matching Leaderboard Send and Get
    public void SendMatchingLeaderBoard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate
                {
                    StatisticName = "MatchingScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnMatchingLeaderboardUpdate, OnError);
    }


    void OnMatchingLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Success leaderboard was sent " + result);
    }



    //Math Leaderboard Send and Get
    public void SendMathLeaderBoard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate
                {
                    StatisticName = "MathScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnMathLeaderboardUpdate, OnError);
    }

    
    void OnMathLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Success leaderboard was sent " + result);
    }



    //Reaction Leaderboard Send and Get
    public void SendReactionLeaderBoard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate
                {
                    StatisticName = "ReactionScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnReactionLeaderboardUpdate, OnError);
    }



    void OnReactionLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Success leaderboard was sent " + result);
    }

    public void GetReactionLeaderBoard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "ReactionScore",
            StartPosition = 0,
            MaxResultsCount = 5
        };
        PlayFabClientAPI.GetLeaderboard(request, OnReactionLeaderboardGet, OnError);
    }


    void OnReactionLeaderboardGet(GetLeaderboardResult result)
    {

        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in result.Leaderboard)
        {

            GameObject myGO = Instantiate(rowPrefab, rowsParent);
            TMPro.TMP_Text[] texts = myGO.GetComponentsInChildren<TMPro.TMP_Text>();
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();

            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
        }
    }
}
