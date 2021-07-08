using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MathPrompt
{

    public MathStatement _statement1;
    public MathStatement _statement2;
    public MathStatement _statement3;

    public Sprite _finalSymbol;
    public int _finalSymbolNum;

    public int _finalAnswerNum;

    //public Image[] _mathOptions;

    public MathPrompt(MathStatement statement1, MathStatement statement2, MathStatement statement3, Sprite finalSymbol, int finalSymbolNum, int finalAnswerNum /*Image[] mathOptions*/)
    {
        _statement1 = statement1;
        _statement2 = statement2;
        _statement3 = statement3;
        _finalSymbol = finalSymbol;
        _finalSymbolNum = finalSymbolNum;
        _finalAnswerNum = finalAnswerNum;
        //_mathOptions = mathOptions;
    }
}
