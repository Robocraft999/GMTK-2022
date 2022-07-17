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

    public List<ActionSlot> SlotsBuildingPlayer { get; set; }
    public List<ActionSlot> SlotsBuildingAI { get; set; }
    public int SlotAmount { get; private set; }
    public List<ActionItem> Actions { get; set; }

    

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
        OnStateChange += StartGameLoop;

        SlotAmount = 6;
        InitActions(SlotAmount);
    }

    void Update()
    {

    }

    void StartGameLoop(GameState oldState, GameState newState)
    {
        if(newState == GameState.CLASH)
        {
            gameloop = Turn();
            StartCoroutine(gameloop);
        }
        else if(oldState == GameState.CLASH)
        {
            StopCoroutine(gameloop);
        }
    }

    IEnumerator Turn()
    {
        int turns = 0;
        yield return new WaitForSeconds(1);
        ClashSceneUIManager.Instance.InitPlayers(SlotsBuildingPlayer, SlotsBuildingAI);
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

            print(input);
            yield return new WaitForSeconds(1);
            PerformActions(input);
            yield return new WaitForSeconds(2);

            turns++;
            if(turns == 20)
            {
                break;
            }
        }
    }

    private void PerformActions(int input)
    {
        foreach(var kvp in ClashSceneUIManager.Instance.Players)
        {
            PerformAction(kvp.Key, input);
        }
    }

    private void PerformAction(PlayerController player, int input)
    {
        List<ActionSlot> slots = ClashSceneUIManager.Instance.Players.Where(kvp => kvp.Key == player).ToList()[0].Value;
        foreach (ActionSlot slot in slots.Where(slot => slot.slotId == input).Where(slot => (object)slot.CurrentItem != null))
        {
            ActionType type = ((ActionItem)slot.CurrentItem).Type;
            if (type.applyForce) player.applyForce(type.force);
            if (type.attack) player.attack();
        }
    }

    private void InitActions(int amount)
    {
        SlotsBuildingPlayer = new List<ActionSlot>();
        SlotsBuildingAI = new List<ActionSlot>();
        Actions = new List<ActionItem>();
        for (int i = 0; i < amount; i++)
        {
            ActionSlot slot = new ActionSlot();
            slot.slotId = i;
            SlotsBuildingPlayer.Add(slot);
        }

        ActionItem actionItem = new ActionItem();
        actionItem.Type = ActionTypes[random.Next(ActionTypes.Count)];
        Actions.Add(actionItem);
    }


    //Util functions
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
                SceneManager.LoadScene("Shop");
                break;
        }
        State = newState;

    }
}
