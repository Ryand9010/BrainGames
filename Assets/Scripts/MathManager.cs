using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class MathManager : MonoBehaviour
{
    public Slot slot;
    public DragDrop[] dragDrop;
    //Timer
    private MathTimer mathTimer;
    //List of Math Prompts
    [SerializeField]
    public List<MathPrompt> mathPromptList;
    //List for Math Sprites
    public List<Sprite> mathSprites = new List<Sprite>(15);
    //answer options
    public Image[] mathChoices = new Image[9];

    //indexers for prompt list
    public int currentMathPrompt;
    public int totalMathPrompts = 0;

    //game objects for the symbols
    public Image statement1Symbol1;
    public Image statement1Operator;
    public Image statement1Symbol2;
    public Image statement1Answer;
    public Image statement2Symbol1;
    public Image statement2Operator;
    public Image statement2Symbol2;
    public Image statement2Answer;
    public Image statement3Symbol1;
    public Image statement3Operator;
    public Image statement3Symbol2;
    public Image statement3Answer;
    public Image finalSymbol;
    public Image finalAnswer;

    //UI For Game Over
    public GameObject mathGamePanel;
    public GameObject mathGameOverPanel;
    public GameObject mathGameOverTimeText;

    //Reference to ScoreScript
    public ScoreScript scoreManager;
    //Reference to AudioManager
    public AudioManager audioManager;


    public void Start()
    {
        mathPromptList = GenerateMathPromptList();
        mathTimer = GameObject.Find("Main Camera").GetComponent<MathTimer>();
        totalMathPrompts = mathPromptList.Count;
        DisplayMathPrompt();
       
    }



    public void CorrectMathAnswer()
    {      
        if (slot.correct)
        {
            audioManager.PlaySound("correct");
            mathPromptList.RemoveAt(currentMathPrompt);
            DisplayMathPrompt();
            slot.correct = false;        
        }
        
    }
    
    public void IncorrectMathAnswer()
    {
        audioManager.PlaySound("incorrect");
    }

    public void MathGameOver()
    {
        mathTimer.StopTimer();
        mathGamePanel.SetActive(false);
        mathGameOverPanel.SetActive(true);
        //calculate player's time
        var timer = mathTimer.GetCurrentTime();
        var seconds = Mathf.RoundToInt(timer);
        var newText = seconds.ToString();
        mathGameOverTimeText.GetComponent<TMPro.TMP_Text>().text = newText;
        //Add to Math leaderboard and sort
        scoreManager.AddMathScoreEntry(seconds, PlayerPrefs.GetString("DisplayName"));
        //play sound
        audioManager.PlaySound("gameOver");
    }



    public List<MathPrompt> GenerateMathPromptList()
    {
        List<MathPrompt> promptList = new List<MathPrompt>();

        Sprite[] sprites = Resources.LoadAll<Sprite>("MathImages");
        Image[] optionsArray = new Image[mathChoices.Length];
        
        //load sprites into MathSprite List
        for(int i = 0; i < sprites.Length; i++)
        {
            mathSprites.Add(sprites[i]);
        }

        
        for (int i = 0; i < 3; i++)
        {
            //load sprites into another temp list
            List<Sprite> tempMathSpriteList = new List<Sprite>(mathSprites);

            //FIRST STATEMENT
            ///////////////////
            ///first number and sprite
            int firstStatementNum1 = Random.Range(1, 10);
            //random num to choose a sprite from list
            int firstStatementImagePick1 = Random.Range(0, tempMathSpriteList.Count);
            //getting that sprite
            Sprite firstStatementNum1Image = tempMathSpriteList[firstStatementImagePick1];
            //Remove Sprite from temp list so its not chosen again
            tempMathSpriteList.Remove(firstStatementNum1Image);
            //second number
            int firstStatementNum2 = Random.Range(1, 10);
            //random num to choose a sprite from list
            int firstStatementImagePick2 = Random.Range(0, tempMathSpriteList.Count);
            //getting that sprite
            Sprite firstStatementNum2Image = tempMathSpriteList[firstStatementImagePick2];
            //Remove Sprite from temp list so its not chosen again
            tempMathSpriteList.Remove(firstStatementNum2Image);
            //operator
            int randomOperator1 = (Random.Range(0, 3));
            string firstStatementOperator = GetOperator(randomOperator1);
     
            //Create Statement
            MathStatement mathStatement1 = new MathStatement(firstStatementNum1Image, firstStatementNum1, firstStatementOperator, firstStatementNum2Image, firstStatementNum2 /*firstStatementAnswer*/);
            
            ///////////////////////
            //SECOND STATEMENT
            ///////////////////////
            //first number and sprite - carried down from above
            int secondStatementNum1 = firstStatementNum1;
            Sprite secondStatementNum1Image = firstStatementNum1Image;
            //second number - new num and sprite
            int secondStatementNum2 = Random.Range(1, 10);
            //random num to choose a sprite from list
            int secondStatementImagePick2 = Random.Range(0, tempMathSpriteList.Count);
            //getting that sprite
            Sprite secondStatementNum2Image = tempMathSpriteList[secondStatementImagePick2];
            //Remove Sprite from temp list so its not chosen again
            tempMathSpriteList.Remove(secondStatementNum2Image);
            //operator
            int randomOperator2 = (Random.Range(0, 3));
            string secondStatementOperator = GetOperator(randomOperator2);
           
            //Create Statement
            MathStatement mathStatement2 = new MathStatement(secondStatementNum1Image, secondStatementNum1, secondStatementOperator, secondStatementNum2Image, secondStatementNum2 /*secondStatementAnswer*/);

            //////////////////////////
            ///THIRD STATEMENT
            ////////////////////////
            //first number and sprite - carried down from second statement
            int thirdStatementNum1 = secondStatementNum2;
            Sprite thirdStatementNum1Image = secondStatementNum2Image;
            //second number - new num and sprite
            int thirdStatementNum2 = Random.Range(1, 10);
            //random num to choose a sprite from list
            int thirdStatementImagePick2 = Random.Range(0, tempMathSpriteList.Count);
            //getting that sprite
            Sprite thirdStatementNum2Image = tempMathSpriteList[thirdStatementImagePick2];
            //Remove Sprite from temp list so its not chosen again
            tempMathSpriteList.Remove(thirdStatementNum2Image);
            //operator
            int randomOperator3 = (Random.Range(0, 3));
            string thirdStatementOperator = GetOperator(randomOperator3);
           
            //Create Statement
            MathStatement mathStatement3 = new MathStatement(thirdStatementNum1Image, thirdStatementNum1, thirdStatementOperator, thirdStatementNum2Image, thirdStatementNum2 /*thirdStatementAnswer*/);


            //Get Final Answer for Prompt
            Sprite finalSprite = thirdStatementNum2Image;
            int finalSpriteNum = thirdStatementNum2;
            int finalAnswerNum = thirdStatementNum2;
           
            
            
            //Create Prompt with above 3 Statement Instances
            MathPrompt prompt = new MathPrompt(mathStatement1, mathStatement2, mathStatement3, finalSprite, finalSpriteNum, finalAnswerNum /*optionsArray*/);
            //add this prompt to the prompt list
            promptList.Add(prompt);
            
        }
        return promptList;     
    }


    public void DisplayMathPrompt()
    {
        //random select for symbol 1 or 2 in each statement
        int randomSlot1 = Random.Range(0, 2);
        int randomSlot2 = Random.Range(0, 2);


        if (mathPromptList.Count > 0)
        {
            //pick a prompt from the prompt list
            currentMathPrompt = Random.Range(0, mathPromptList.Count);
            //assign to images on screen
            //statement 1
            //first statement first symbol and num
            statement1Symbol1.sprite = mathPromptList[currentMathPrompt]._statement1._symbol1;
            int s1num1 = mathPromptList[currentMathPrompt]._statement1._num1;
            statement1Symbol1.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = mathPromptList[currentMathPrompt]._statement1._num1.ToString();
            //first statement operator
            string operator1 = mathPromptList[currentMathPrompt]._statement1._operatorSymbol;
            statement1Operator.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = operator1;
            //first statement second symbol and num
            statement1Symbol2.sprite = mathPromptList[currentMathPrompt]._statement1._symbol2;
            int s1num2 = mathPromptList[currentMathPrompt]._statement1._num2;
            statement1Symbol2.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = mathPromptList[currentMathPrompt]._statement1._num2.ToString();
            //first statement answer
            statement1Answer.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = GetStatementAnswer(operator1, s1num1, s1num2).ToString();

            //statement2
            //second statement operator 
            string operator2 = mathPromptList[currentMathPrompt]._statement2._operatorSymbol;
            statement2Operator.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = operator2;
            //randomize which spots in statement 2 are filled
            if (randomSlot1 == 0)
            {
                statement2Symbol1.sprite = mathPromptList[currentMathPrompt]._statement2._symbol1;
                statement2Symbol1.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = mathPromptList[currentMathPrompt]._statement2._num1.ToString();

                statement2Symbol2.sprite = mathPromptList[currentMathPrompt]._statement2._symbol2;
                statement2Symbol2.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = mathPromptList[currentMathPrompt]._statement2._num2.ToString();
            }
            else
            {
                statement2Symbol1.sprite = mathPromptList[currentMathPrompt]._statement2._symbol2;
                statement2Symbol1.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = mathPromptList[currentMathPrompt]._statement2._num2.ToString();

                statement2Symbol2.sprite = mathPromptList[currentMathPrompt]._statement2._symbol1;
                statement2Symbol2.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = mathPromptList[currentMathPrompt]._statement2._num1.ToString();
            } 
            //storing numbers
            int s2num1 = mathPromptList[currentMathPrompt]._statement2._num1;
            int s2num2 = mathPromptList[currentMathPrompt]._statement2._num2;
            //second statement answer
            statement2Answer.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = GetStatementAnswer(operator2, s2num1, s2num2).ToString();

            //statement 3
            //third statement operator 
            string operator3 = mathPromptList[currentMathPrompt]._statement3._operatorSymbol;
            statement3Operator.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = operator3;
            //randomize which spots in statement 3 are filled
            if (randomSlot2 == 0)
            {
                statement3Symbol1.sprite = mathPromptList[currentMathPrompt]._statement3._symbol1;
                statement3Symbol1.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = mathPromptList[currentMathPrompt]._statement3._num1.ToString();

                statement3Symbol2.sprite = mathPromptList[currentMathPrompt]._statement3._symbol2;
                statement3Symbol2.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = mathPromptList[currentMathPrompt]._statement3._num2.ToString();
            }
            else
            {
                statement3Symbol1.sprite = mathPromptList[currentMathPrompt]._statement3._symbol2;
                statement3Symbol1.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = mathPromptList[currentMathPrompt]._statement3._num2.ToString();

                statement3Symbol2.sprite = mathPromptList[currentMathPrompt]._statement3._symbol1;
                statement3Symbol2.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = mathPromptList[currentMathPrompt]._statement3._num1.ToString();
            }
            
            //storing numbers
            int s3num1 = mathPromptList[currentMathPrompt]._statement3._num1;
            int s3num2 = mathPromptList[currentMathPrompt]._statement3._num2;
            //second statement answer
            statement3Answer.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = GetStatementAnswer(operator3, s3num1, s3num2).ToString();

            //final statement
            finalSymbol.sprite = mathPromptList[currentMathPrompt]._finalSymbol;
            int finalAnswerNumber = mathPromptList[currentMathPrompt]._finalAnswerNum;
            finalSymbol.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = finalAnswerNumber.ToString();
            finalAnswer.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = finalAnswerNumber.ToString();



            //Generate options
            List<int> tempOptions = new List<int>(14);

            for(int i = 1; i <= 14; i++)
            {
                tempOptions.Add(i);
            }

            //remove correct answer from tempOptions
            tempOptions.Remove(finalAnswerNumber);
            //random pointers for tempOptions and MathChoices 
            int randomMathChoice = Random.Range(0, mathChoices.Length);

            for (int i = 0; i < mathChoices.Length; i++)
            {
                if (i == randomMathChoice)
                {
                    mathChoices[i].transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = finalAnswerNumber.ToString();
                    dragDrop[i].id = finalAnswerNumber;
                }
                else
                {
                    int randomOptionsSpot = Random.Range(0, tempOptions.Count);
                    mathChoices[i].transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = tempOptions[randomOptionsSpot].ToString();
                    tempOptions.RemoveAt(randomOptionsSpot);
                }
            }

            slot.id = finalAnswerNumber;
            
        }
        else
        {
            MathGameOver();
        }
    }


    public string GetOperator(int choice)
    {
        string chosenOperator = "";
        
       switch(choice)
        {
            case 0:
                chosenOperator = "*";
                break;
            case 1:
                chosenOperator = "+";
                break;
            case 2:
                chosenOperator = "-";
                break;        
        }

        return chosenOperator;
    }

    public int GetStatementAnswer(string operatorChoice, int num1, int num2)
    {
        int answer = 0;


        switch (operatorChoice)
        {
            case "*":
                answer = num1 * num2;
                break;
            case "+":
                answer = num1 + num2;
                break;
            case "-":
                answer = num1 - num2;
                break;
        }

        return answer;
    }



}
