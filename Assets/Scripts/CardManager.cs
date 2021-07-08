using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardManager : MonoBehaviour
{

    //Reference to ScoreScript
    public ScoreScript scoreManager;
    //Reference to AudioManager
    public AudioManager audioManager;

    [HideInInspector]
    public List<Card> cardList;
    //Card and SpawnPos GameObjects
    public Card cardPrefab;
    public Transform cardSpawnPosition;

    [Space]
    [Header("Matching End Game Screen")]
    public GameObject MatchingEndPanel;
    public GameObject PlayerScoreText;
    public GameObject EndTimerText;

    public Vector2 startingPos = new Vector2(-350.15f, 13.62f);
    private Vector2 _offset = new Vector2(8.65f, 8.52f);


    public enum GameState 
    { 
        NoAction, 
        MovingOnPosition, 
        DeletingCards, 
        Flipback, 
        Checking, 
        GameEnd
    };


    public enum CardState
    {
        CardRotating,
        CanRotate
    };

    public enum RevealedState
    {
        NoneRevealed,
        OneRevealed,
        TwoRevealed
    };

    [HideInInspector]
    public GameState currentGameState;
    [HideInInspector]
    public CardState currentCardState;
    [HideInInspector]
    public RevealedState cardRevealedNumber;
    private int imageToDestroy1;
    private int imageToDestroy2;

    private List<Material> cardMaterialList = new List<Material>();
    private List<string> texturePathList = new List<string>();
    private Material firstMaterial;
    private string firstTexturePath;

    private int pairNum = 15;
    private int removedPairs;
    private MatchingTimer matchingTimer;

    private int firstRevealedImage;
    private int secondRevealedImage;
    private int revealedImageNumber  = 0;

    private bool corutineStarted = false;

    void Start()
    {
        currentGameState = GameState.NoAction;
        currentCardState = CardState.CanRotate;
        cardRevealedNumber = RevealedState.NoneRevealed;
        revealedImageNumber = 0;
        firstRevealedImage = -1;
        secondRevealedImage = -1;

        removedPairs = 0;
        matchingTimer = GameObject.Find("Main Camera").GetComponent<MatchingTimer>();

        LoadMaterials();

        currentGameState = GameState.MovingOnPosition;
        SpawnCardGrid(6, 5, startingPos, _offset, false);
        MoveCard(6, 5);
 
    }



    public void CheckImage()
    {
        audioManager.PlaySound("matchFlip");
        currentGameState = GameState.Checking;
        revealedImageNumber = 0;
        for (int id = 0; id < cardList.Count; id++)
        {
            //if clicked card is the first of two
           if (cardList[id].revealed && revealedImageNumber < 2)
            {
                if (revealedImageNumber == 0)
                {
                    firstRevealedImage = id;
                    revealedImageNumber++;
                }
                else if(revealedImageNumber == 1)
                {
                    secondRevealedImage = id;
                    revealedImageNumber++;
                }
            }
        }

        //if clicked card is the second of the two 
        if (revealedImageNumber == 2)
        {
            if(cardList[firstRevealedImage].GetIndex() == cardList[secondRevealedImage].GetIndex() && firstRevealedImage != secondRevealedImage)
            {
                //selected pair to be destroyed
                currentGameState = GameState.DeletingCards;
                imageToDestroy1 = firstRevealedImage;
                imageToDestroy2 = secondRevealedImage;
            }
            else
            {
                currentGameState = GameState.Flipback;
            }
            
        }

        currentCardState = CardManager.CardState.CanRotate;

        if(currentGameState == GameState.Checking)
        {
            currentGameState = GameState.NoAction;
        }
    }


    //get rid of the cards
    private void DestoryPicture()
    {
        //play sound
        audioManager.PlaySound("correct");
        //none revealed set
        cardRevealedNumber = RevealedState.NoneRevealed;
        cardList[imageToDestroy1].Deactivate();
        cardList[imageToDestroy2].Deactivate();
        revealedImageNumber = 0;
        removedPairs++;
        currentGameState = GameState.NoAction;
        currentCardState = CardState.CanRotate;
    }



    private IEnumerator FlipBack()
    {
        corutineStarted = true;
        //wait half a second to turn back
        yield return new WaitForSeconds(0.5f);
        //play sound
        audioManager.PlaySound("matchFlip");
        //flip back
        cardList[firstRevealedImage].FlipBack();
        cardList[secondRevealedImage].FlipBack();
        //cards are no longer revealed
        cardList[firstRevealedImage].revealed = false;
        cardList[secondRevealedImage].revealed = false;

        cardRevealedNumber = RevealedState.NoneRevealed;
        currentGameState = GameState.NoAction;

        corutineStarted = false;
    }


    private void LoadMaterials()
    {
        var materialFilePath = MatchingSettings.Instance.GetMaterialDirectoryName();
        var textureFilePath = MatchingSettings.Instance.GetCategoryTextureDirectoryName();
        const string matBaseName = "Img"; //all pictures are labeled with "Img1", "Img2" etc...
        var firstMaterialName = "Back"; //default question mark for back of card

        //assigning each image from chosen category into Materials folder
        for(var index = 1; index <= 15; index++)
        {
            var currentFilePath = materialFilePath + matBaseName + index;
            Material mat = Resources.Load(currentFilePath, typeof(Material)) as Material;
            cardMaterialList.Add(mat);

            var currentTextureFilePath = textureFilePath + matBaseName + index;
            texturePathList.Add(currentTextureFilePath);
        }

        firstTexturePath = textureFilePath + firstMaterialName;
        firstMaterial = Resources.Load(materialFilePath + firstMaterialName, typeof(Material)) as Material;
    }

    void Update() //always checking for these game states
    {
        if(currentGameState == GameState.DeletingCards)
        {
            if (currentCardState == CardState.CanRotate)
            {
                DestoryPicture();
                CheckMatchingGameEnd();
            }
        }
        //Flipback
        if(currentGameState == GameState.Flipback)
        {
            if(currentCardState == CardState.CanRotate && corutineStarted == false)
            {
                StartCoroutine(FlipBack());
            }
        }
        //if game is over, run ShowMatchingEndStats function
        if(currentGameState == GameState.GameEnd)
        {
            if(cardList[firstRevealedImage].gameObject.activeSelf == false && cardList[secondRevealedImage].gameObject.activeSelf == false && MatchingEndPanel.activeSelf == false)
            {
                ShowMatchingEndStats();
            } 
        }
    }

    //if all pairs are removed, stop the timer and switch the game state to GameEnd
    private bool CheckMatchingGameEnd()
    {
        if (removedPairs == pairNum && currentGameState != GameState.GameEnd)
        {
            currentGameState = GameState.GameEnd;
            matchingTimer.StopTimer();
        }
        return (currentGameState == GameState.GameEnd);
    }

    //Game is Over
    //Activate the end game panel, and apply timer time to panel text
    private void ShowMatchingEndStats()
    {
        MatchingEndPanel.SetActive(true);
        PlayerScoreText.SetActive(true);
        

        var timer = matchingTimer.GetCurrentTime();

        var seconds = Mathf.RoundToInt(timer);
        var newText = seconds.ToString();
        EndTimerText.GetComponent<TMPro.TMP_Text>().text = newText;
        //Add to Matching leaderboard and sort
        scoreManager.AddMatchingScoreEntry(seconds, PlayerPrefs.GetString("DisplayName"));
        //Play Sound
        audioManager.PlaySound("gameOver");
    }

    //create a matrix of the cards to be on the screen
    private void SpawnCardGrid(int rows, int columns, Vector2 pos, Vector2 offset, bool scale)
    {
        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                var tempCard = (Card)Instantiate(cardPrefab, cardSpawnPosition.position, cardPrefab.transform.rotation);

                tempCard.name = tempCard.name + "c" + col + "r" + row;

                cardList.Add(tempCard);
            }
        }
        //applying random images from chosen category to each card
        ApplyTextures();
    }



    public void ApplyTextures()
    {
        var rndMaterialIndex = Random.Range(0, cardMaterialList.Count);
        int[] appliedtimes = new int[cardMaterialList.Count];

        for(int i = 0; i < cardMaterialList.Count; i++)
        {
            appliedtimes[i] = 0;
        }
        //applying 2 of the 30 cards the same images
        foreach(var obj in cardList)
        {
            var randPrevious = rndMaterialIndex;
            int counter = 0;
            bool forceMat = false;

            while(appliedtimes[rndMaterialIndex] >= 2 || ((randPrevious == rndMaterialIndex) && !forceMat))
            {
                rndMaterialIndex = Random.Range(0, cardMaterialList.Count);
                counter++;
                if(counter > 100)
                {
                    for(var j = 0; j < cardMaterialList.Count; j++)
                    {
                        if(appliedtimes[j] < 2)
                        {
                            rndMaterialIndex = j;
                            forceMat = true;
                        }
                    }

                    if(forceMat == false)
                    {
                        return;
                    }
                }
            }

            obj.setFirstMaterial(firstMaterial, firstTexturePath);
            obj.ApplyFirstMaterial();
            obj.setSecondMaterial(cardMaterialList[rndMaterialIndex], texturePathList[rndMaterialIndex]);
            obj.SetIndex(rndMaterialIndex);
            obj.revealed = false;
            appliedtimes[rndMaterialIndex] += 1;
            forceMat = false;
        }
    }

    //For each card, apply MoveToPostition
    private void MoveCard(int rows, int columns)
    {
        var index = 0;
        for (var col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {

                var targetPosition = new Vector3((-21.75f + (8.65f * row)), (13.76f - (8.52f * col)), 0.0f);
                StartCoroutine(MoveToPosition(targetPosition, cardList[index]));
                index++;
            }
        }
    }

    //moving the cards from the spawn point to given position
    private IEnumerator MoveToPosition(Vector3 target, Card obj)
    {
        var cardSpeed = 70;

        while(obj.transform.position != target)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, cardSpeed * Time.deltaTime);
            yield return 0;
        }
    }
}

