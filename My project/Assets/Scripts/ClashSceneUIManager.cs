using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ClashSceneUIManager : MonoBehaviour
{
    public static ClashSceneUIManager Instance { get; private set; }

    public List<KeyValuePair<PlayerController, List<ActionSlot>>> Players { get; private set; }

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        InitClashScene();
    }

    private void InitClashScene()
    {
        
    }

    public void InitPlayers(List<ActionSlot> player1, List<ActionSlot> player2)
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        Players = new List<KeyValuePair<PlayerController, List<ActionSlot>>>();
        Players.Add(new KeyValuePair<PlayerController, List<ActionSlot>>(players[0], player1));
        Players.Add(new KeyValuePair<PlayerController, List<ActionSlot>>(players[1], player2));
    }
}
