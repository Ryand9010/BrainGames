using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class ReactionManager : MonoBehaviour
{
    //Reference to ScoreScript
    public ScoreScript ScoreManager;
    //Reference to AudioManager
    public AudioManager audioManager;

    //Timer
    private ReactionTimer reactionTimer;
    //List of Prompts
    [SerializeField]
    public List<ReactionQuestionsAnswers> promptAnswerList;
    //Button choices
    public GameObject[] choices;
    //List of all Sprites to be used
    public List<Sprite> reactionSprites = new List<Sprite>(28);

    public int currentPrompt;
    public int totalPrompts = 0;

    public TMP_Text promptText;

    //Panels
    public GameObject reactionGamePanel;
    public GameObject reactionGameOverPanel;
    //Timer text for Reaction Game End
    public GameObject reactionGameOverTimeText;

    
    public void Start()
    {
        promptAnswerList = GenerateQuestionList();
        reactionTimer = GameObject.Find("Main Camera").GetComponent<ReactionTimer>();
        totalPrompts = promptAnswerList.Count;
        reactionGameOverPanel.SetActive(false);
        GeneratePrompt();
    }




    void GeneratePrompt()
    {

        if(promptAnswerList.Count > 0)
        {
            currentPrompt = Random.Range(0, promptAnswerList.Count);

            promptText.text = promptAnswerList[currentPrompt]._prompt;
            SetAnswers();
        }
        else
        {
            Debug.Log("Out of Questions");
            ReactionGameOver();
        }
        
    }


    void SetAnswers()
    {
        for (int i = 0; i < choices.Length; i++)
        {
            choices[i].GetComponent<ReactionAnswers>().isReactionGuessCorrect = false;
            choices[i].transform.GetChild(0).GetComponent<Image>().sprite = promptAnswerList[currentPrompt]._answers[i];

            if(promptAnswerList[currentPrompt]._correctAnswer == i + 1)
            {
                choices[i].GetComponent<ReactionAnswers>().isReactionGuessCorrect = true;
            }
        }
    }

    //remove question and remove it from list when the answer is correct
    public void CorrectAnswer()
    {
        promptAnswerList.RemoveAt(currentPrompt);
        GeneratePrompt();
        //play sound
        audioManager.PlaySound("correct");
    }


    public void IncorrectAnswer()
    {
        //play sound
        audioManager.PlaySound("incorrect");
    }

    public void ReactionGameOver()
    {
        reactionTimer.StopTimer();
        reactionGamePanel.SetActive(false);
        reactionGameOverPanel.SetActive(true);

        //calculate player's time
        var timer = reactionTimer.GetCurrentTime();
        int seconds = Mathf.RoundToInt(timer);
        var newText = seconds.ToString();
        reactionGameOverTimeText.GetComponent<TMPro.TMP_Text>().text = newText;
        //Add to Image Picker leaderboard and sort
        ScoreManager.AddImageScoreEntry(seconds, PlayerPrefs.GetString("DisplayName"));
        //play sound
        audioManager.PlaySound("gameOver");
    }


    public List<ReactionQuestionsAnswers> GenerateQuestionList()
    {
        List<ReactionQuestionsAnswers> questionList = new List<ReactionQuestionsAnswers>();

        //Load all sprites from resources into array
        var sprites = Resources.LoadAll<Sprite>("ReactionImages");

        //load sprites into reactionSprites List
        for(int i = 0; i < sprites.Length; i++)
        {
            reactionSprites.Add(sprites[i]);
        }

        //create a question for each sprite
        for(int i = 0; i < reactionSprites.Count; i++)
        {
            //array for answer/options
            Sprite[] answerArray = new Sprite[6];
            //new list to copy from ReactionSprites
            List<Sprite> tempImageList = new List<Sprite>(reactionSprites);
            //random number to pick where to put [i] index of ReactionSprite
            int randomScramble = Random.Range(0, 6);
            //assigning [i] of reactionSprite List to the random spot, this is the answer
            answerArray[randomScramble] = reactionSprites[i];
            //assigning the prompt string with the image's name
            string answer = answerArray[randomScramble].name;
                        
            //removing the sprite from the temp list
            tempImageList.Remove(answerArray[randomScramble]);

            //fill the rest of the answer array with unused sprites
            for (int j = 0; j < answerArray.Length; j++)
            {
                if(j != randomScramble)
                {
                    Sprite randomSprite = tempImageList[Random.Range(0, tempImageList.Count - 1)];

                    answerArray[j] = randomSprite;

                    tempImageList.Remove(randomSprite);
                }         
            }

            //finding the correct index of generated answer
            int answerSpot = FindCorrectAnswer(answerArray, answer);

            ReactionQuestionsAnswers question = new ReactionQuestionsAnswers(answer, answerArray, answerSpot);

            questionList.Add(question);
        }

        return questionList;
    }


    public int FindCorrectAnswer(Sprite[] array, string name)
    {
        int correctIndex = 0;

        for (int i = 0; i < array.Length; i++)
        {
            if(array[i].name == name)
            {
                correctIndex = i + 1;
            }
        }

        return correctIndex;
    }

}
