using System;
using System.Linq;
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
            OnStateChange?.Invoke(m_state, value);
            m_state = value;
        }
    }
    public delegate void GameStateDelegate(GameState oldState, GameState newState);
    public event GameStateDelegate OnStateChange;

    public System.Random random;
    private IEnumerator gameloop;
    private int rounds;

    public List<ActionType> ActionTypes;

    public List<PlayerController> Players;

    public int SlotAmount { get; private set; }

    

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
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
        OnStateChange += StateChangeHandler;

        SlotAmount = 6;
        InitActions(SlotAmount);
    }

    void StateChangeHandler(GameState oldState, GameState newState)
    {
        if (newState == GameState.CLASH)
        {
            gameloop = Turn();
            if (rounds < MenuSceneUIManager.Instance.rounds)
            {
                StartCoroutine(gameloop);
                foreach (var player in Players) player.Activate(true);
            }
            else
            {
                SwitchScene(GameState.MENU);
            }
        }
        else if (oldState == GameState.CLASH)
        {
            foreach (var player in Players) player.Activate(false);
            StopCoroutine(gameloop);
        }
        if(newState == GameState.SHOP)
        {
            foreach(var player in Players)
            {
                player.AddScore(50);
            }
        }
    }

    IEnumerator Turn()
    {
        int turns = 0;
        yield return new WaitUntil(() => ClashSceneUIManager.Instance);
        while (true)
        {
            IEnumerator enumerator = ClashSceneUIManager.Instance.UITurn();
            yield return enumerator;

            int input = random.Next(SlotAmount);
            if (enumerator.Current is int value)
            {
                if (value >= 0) input = value;
                else Debug.LogWarning("Could not determine result of dice");
            }

            yield return new WaitForSeconds(0.2f); 
            yield return PerformActions(input);
            yield return new WaitForSeconds(MenuSceneUIManager.Instance.interval);

            turns++;
            if (turns == MenuSceneUIManager.Instance.rolls) 
            {
                break;
            }
        }
        SwitchScene(GameState.SHOP);
    }

    private IEnumerator PerformActions(int input)
    {
        foreach(var player in Players)
        {
            yield return player.PerformAction(input);
        }
    }

    private void InitActions(int amount)
    {
        foreach(var player in Players)
        {
            player.InitActions(amount);
        }
    }


    public void SwitchScene(GameState newState)
    {
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
                rounds++;
                break;
        }
        State = newState;

    }

    public ActionType RandomAction()
    {
        return ActionTypes[random.Next(ActionTypes.Count)];
    }
}
