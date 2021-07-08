using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathStatement 
{

    public Sprite _symbol1;
    public int _num1;
    public string _operatorSymbol;
    public Sprite _symbol2;
    public int _num2;
    //public int _answer;

    public MathStatement(Sprite symbol1, int num1, string operatorSymbol, Sprite symbol2, int num2  /*int answer*/)
    {
        _symbol1 = symbol1;
        _num1 = num1;
        _operatorSymbol = operatorSymbol;
        _symbol2 = symbol2;
        _num2 = num2;
        //_answer = answer;
    }

}
