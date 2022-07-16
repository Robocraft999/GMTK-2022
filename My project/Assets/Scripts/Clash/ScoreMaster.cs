using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class ScoreMaster : MonoBehaviour 
{
    public TMP_Text scoreTextP1;
    private int score1 = 0;
    public int Score1 { 
        get => score1;
        set {
            score1 = value;
            if (scoreTextP1) scoreTextP1.text = "" + value;
        }
    }

    public TMP_Text scoreTextP2;
    private int score2 = 0;
    public int Score2
    {
        get => score2;
        set
        {
            if (scoreTextP2) scoreTextP2.text = "" + value;
        }
    }

    void Update()
    {
        
    }
}


