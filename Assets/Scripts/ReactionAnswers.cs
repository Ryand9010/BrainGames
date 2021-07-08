using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionAnswers : MonoBehaviour
{

    public bool isReactionGuessCorrect = false;
    public ReactionManager reactionManager;

    public void ReactionAnswer()
    {
        if(isReactionGuessCorrect)
        {
            Debug.Log("Correct Answer");
            reactionManager.CorrectAnswer();
        }
        else
        {
            Debug.Log("Wrong Answer");
            reactionManager.IncorrectAnswer();
        }
    }
}
