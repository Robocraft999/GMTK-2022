using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameState m_state;
    public GameState State { 
        get { 
            return m_state; 
        } 
        private set { 
            if (m_state == value) return;
            if (OnStateChange != null)
                OnStateChange(m_state, value);
            m_state = value;
        }
    }
    public delegate void GameStateDelegate(GameState oldState, GameState newState);
    public event GameStateDelegate OnStateChange;

    public System.Random random;
    private IEnumerator gameloop;

    public List<ActionType> ActionTypes;

    public List<ActionSlot> Slots_Building { get; set; }
    public List<ActionItem> Actions { get; set; }

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        random = new System.Random();
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        m_state = GameState.MENU;

        InitActions(6);
        gameloop = Turn();
    }

    void Update()
    {
        if(State == GameState.CLASH)
        {
            StartCoroutine(gameloop);
        }
    }

    IEnumerator Turn()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            yield return new WaitForFixedUpdate();
        }
    }

    private void InitActions(int amount)
    {
        Slots_Building = new List<ActionSlot>();
        Actions = new List<ActionItem>();
        for (int i = 0; i < amount; i++)
        {
            ActionSlot slot = new ActionSlot();
            slot.slotId = i;
            Slots_Building.Add(slot);
        }

        ActionItem actionItem = new ActionItem();
        actionItem.Type = ActionTypes[random.Next(ActionTypes.Count)];
        Actions.Add(actionItem);
    }


    //Util functions
    public void SwitchScene(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.MENU:
                SceneManager.LoadScene("Menu");
                break;
            case GameState.BUILDING:
                SceneManager.LoadScene("Building");
                break;
            case GameState.CLASH:
                SceneManager.LoadScene("Clash");
                break;
            case GameState.SHOP:
                SceneManager.LoadScene("Shop");
                break;
        }
        
    }
}
