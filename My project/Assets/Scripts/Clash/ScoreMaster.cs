using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class ScoreMaster : MonoBehaviour 
{
    ScoreMaster Instance;

    

    public TMP_Text scoreTextP2;
    private int score2 = 0;
    public int Score2
    {
        get => score2;
        set
        {
            score2 = value;
            if (scoreTextP2) scoreTextP2.text = "" + value;
        }
    }

    private void Start()
    {
        Instance = this;
    }
}


