using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    //Image Picker container
    public Transform ImageEntryContainer;
    public Transform ImageEntryTemplate;
    //Math containr
    public Transform MathEntryContainer;
    public Transform MathEntryTemplate;
    //matching container
    public Transform MatchingEntryContainer;
    public Transform MatchingEntryTemplate;
    //list of transforms for  image entries
    private List<Transform> imageEntryTransformList;
    private List<Transform> mathEntryTransformList;
    private List<Transform> matchingEntryTransformList;
    //lists of entries
    private List<ScoreEntry> imageEntryList;
    private List<ScoreEntry> mathEntries;
    private List<ScoreEntry> matchingEntries;
    //Panel variables for showing and hiding
    public GameObject imagePickPanel;
    public GameObject mathPanel;
    public GameObject matchingPickPanel;
    public GameObject chooserPanel;


    private void Awake()
    {

        //setting placeholder entry to disappear
        ImageEntryTemplate.gameObject.SetActive(false);
        MathEntryTemplate.gameObject.SetActive(false);
        MatchingEntryTemplate.gameObject.SetActive(false);

        if (!PlayerPrefs.HasKey("ImagePickLeaderboard"))
        {
            imageEntryList = new List<ScoreEntry>()
            {
                new ScoreEntry{score = 90, name = "Peter"}
            };

            ScoreEntries scoreEntry = new ScoreEntries { scoreEntryList = imageEntryList };
            string json = JsonUtility.ToJson(scoreEntry);
            PlayerPrefs.SetString("ImagePickLeaderboard", json);
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("MathLeaderboard"))
        {
            mathEntries = new List<ScoreEntry>()
            {
                new ScoreEntry{score = 200, name = "Joe"}
            };

            ScoreEntries scoreEntry = new ScoreEntries { scoreEntryList = mathEntries };
            string json = JsonUtility.ToJson(scoreEntry);
            PlayerPrefs.SetString("MathLeaderboard", json);
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("MatchingLeaderboard"))
        {

            matchingEntries = new List<ScoreEntry>()
            {
                new ScoreEntry{score = 300, name = "Brian"}
            };

            ScoreEntries scoreEntry = new ScoreEntries { scoreEntryList = matchingEntries };
            string json = JsonUtility.ToJson(scoreEntry);
            PlayerPrefs.SetString("MatchingLeaderboard", json);
            PlayerPrefs.Save();
        }
        else
        {
            ReadyLeaderboard();
        }
    }


    //class for a single Entry
    [System.Serializable]
    public class ScoreEntry
    {
        public int score;
        public string name;
    }

    //class holding list of score entries
    public class ScoreEntries
    {
        public List<ScoreEntry> scoreEntryList;
    }

    public void ReadyLeaderboard()
    {
        //getting playerPrefs
        string imageJson = PlayerPrefs.GetString("ImagePickLeaderboard");
        string mathJson = PlayerPrefs.GetString("MathLeaderboard");
        string matchingJson = PlayerPrefs.GetString("MatchingLeaderboard");
        ScoreEntries imageEntries = JsonUtility.FromJson<ScoreEntries>(imageJson);
        ScoreEntries mathEntries = JsonUtility.FromJson<ScoreEntries>(mathJson);
        ScoreEntries matchingEntries = JsonUtility.FromJson<ScoreEntries>(matchingJson);

        //Sort the leaderboards
        SortLeaderboard(imageEntries);
        SortLeaderboard(mathEntries);
        SortLeaderboard(matchingEntries);

        //Creating Transform Lists
        imageEntryTransformList = new List<Transform>();
        mathEntryTransformList = new List<Transform>();
        matchingEntryTransformList = new List<Transform>();

        //Displaying
        for (int i = 0; i < 3; i++)
        {
            CreateEntryTransform(imageEntries.scoreEntryList[i], ImageEntryContainer, imageEntryTransformList, ImageEntryTemplate);
            CreateEntryTransform(mathEntries.scoreEntryList[i], MathEntryContainer, mathEntryTransformList, MathEntryTemplate);
            CreateEntryTransform(matchingEntries.scoreEntryList[i], MatchingEntryContainer, matchingEntryTransformList, MatchingEntryTemplate);
        }
    }


    public void SortLeaderboard(ScoreEntries entries)
    {

        //sort the leaderboard low to high
        for (int i = 0; i < entries.scoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < entries.scoreEntryList.Count; j++)
            {
                if (entries.scoreEntryList[j].score < entries.scoreEntryList[i].score)
                {
                    ScoreEntry temp = entries.scoreEntryList[i];
                    entries.scoreEntryList[i] = entries.scoreEntryList[j];
                    entries.scoreEntryList[j] = temp;
                }
            }
        }
    }


    public void AddImageScoreEntry(int score, string name)
    {
        //Create Entry
        ScoreEntry scoreEntry = new ScoreEntry { score = score, name = name };
        //Get the saved Scores
        string json = PlayerPrefs.GetString("ImagePickLeaderboard");
        ScoreEntries scoreEntries = JsonUtility.FromJson<ScoreEntries>(json);
        //Add the new Entry
        scoreEntries.scoreEntryList.Add(scoreEntry);
        //Save
        string jsonSave = JsonUtility.ToJson(scoreEntries);
        PlayerPrefs.SetString("ImagePickLeaderboard", jsonSave);
        PlayerPrefs.Save();
        SortLeaderboard(scoreEntries);
    }

    public void AddMathScoreEntry(int score, string name)
    {
        //Create Entry
        ScoreEntry scoreEntry = new ScoreEntry { score = score, name = name };
        //Get the saved Scores
        string json = PlayerPrefs.GetString("MathLeaderboard");
        ScoreEntries scoreEntries = JsonUtility.FromJson<ScoreEntries>(json);
        //Add the new Entry
        scoreEntries.scoreEntryList.Add(scoreEntry);
        //Save
        string jsonSave = JsonUtility.ToJson(scoreEntries);
        PlayerPrefs.SetString("MathLeaderboard", jsonSave);
        PlayerPrefs.Save();
        SortLeaderboard(scoreEntries);
    }

    public void AddMatchingScoreEntry(int score, string name)
    {
        //Create Entry
        ScoreEntry scoreEntry = new ScoreEntry { score = score, name = name };
        //Get the saved Scores
        string json = PlayerPrefs.GetString("MatchingLeaderboard");
        ScoreEntries scoreEntries = JsonUtility.FromJson<ScoreEntries>(json);
        //Add the new Entry
        scoreEntries.scoreEntryList.Add(scoreEntry);
        //Save
        string jsonSave = JsonUtility.ToJson(scoreEntries);
        PlayerPrefs.SetString("MatchingLeaderboard", jsonSave);
        PlayerPrefs.Save();
        SortLeaderboard(scoreEntries);
    }


    //Create a transform for an entry
    public void CreateEntryTransform(ScoreEntry scoreEntry, Transform container, List<Transform> transformList, Transform template)
    {
        float entryTemplateHeight = 90f;
        Transform entryTransfrom = Instantiate(template, container);
        RectTransform entryRectTransform = entryTransfrom.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -entryTemplateHeight * transformList.Count);
        entryTransfrom.gameObject.SetActive(true);
        //adding 1 to the rank so first place is not zero
        int rank = transformList.Count + 1;
        string rankString;
        //switch statement for the placement naming scheme
        switch (rank)
        {
            default:
                rankString = rank + "Th";
                break;
            case 1:
                rankString = "1st";
                break;
            case 2:
                rankString = "2nd";
                break;
            case 3:
                rankString = "3rd";
                break;
        }

        entryTransfrom.Find("PlaceText").GetComponent<TMPro.TMP_Text>().text = rankString;

        //name
        string name = scoreEntry.name;
        entryTransfrom.Find("NameText").GetComponent<TMPro.TMP_Text>().text = name;
        //score
        int score = scoreEntry.score;
        entryTransfrom.Find("TimeText").GetComponent<TMPro.TMP_Text>().text = score.ToString();

        transformList.Add(entryTransfrom);
    }




    //methods for hiding  and showing the leaderboard panels
    //and displaying the leaderboards
    public void ShowImagePickPanel()
    {
        chooserPanel.SetActive(false);
        mathPanel.SetActive(false);
        matchingPickPanel.SetActive(false);
        imagePickPanel.SetActive(true);
    }
    public void ShowMathPanel()
    {
        chooserPanel.SetActive(false);
        mathPanel.SetActive(true);
        matchingPickPanel.SetActive(false);
        imagePickPanel.SetActive(false);
    }
    public void ShowMatchingPanel()
    {
        chooserPanel.SetActive(false);
        mathPanel.SetActive(false);
        matchingPickPanel.SetActive(true);
        imagePickPanel.SetActive(false);

    }
    public void ShowChooserPanel()
    {
        chooserPanel.SetActive(true);
        mathPanel.SetActive(false);
        matchingPickPanel.SetActive(false);
        imagePickPanel.SetActive(false);
    }
}
