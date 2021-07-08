using UnityEngine;
using System.Collections.Generic;


public class ReactionQuestionsAnswers 
{
    public string _prompt;
    public Sprite[] _answers;
    public int _correctAnswer;

    public ReactionQuestionsAnswers(string prompt, Sprite[] answers, int correctAnswer)
    {
        _prompt = prompt;
        _answers = answers;
        _correctAnswer = correctAnswer;
    }
}
