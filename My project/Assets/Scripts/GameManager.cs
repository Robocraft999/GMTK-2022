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

    public List<KeyValuePair<List<ActionSlot>, List<ActionItem>>> PlayerData { get; set; }
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
        OnStateChange += StartGameLoop;

        SlotAmount = 6;
        InitActions(SlotAmount);
    }

    void StartGameLoop(GameState oldState, GameState newState)
    {
        if (newState == GameState.CLASH)
        {
            gameloop = Turn();
            if (rounds < MenuSceneUIManager.Instance.rounds)
            {
                StartCoroutine(gameloop);
            }
            else
            {
                SwitchScene(GameState.MENU);
            }
        }
        else if (oldState == GameState.CLASH)
        {
            StopCoroutine(gameloop);
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

            print(input);
            yield return new WaitForSeconds(0.2f); 
            PerformActions(input);
            yield return new WaitForSeconds(1);

            turns++;
            if (turns == MenuSceneUIManager.Instance.rolls) 
            {
                break;
            }
        }
        SwitchScene(GameState.SHOP);
    }

    private void PerformActions(int input)
    {
        foreach(var player in ClashSceneUIManager.Instance.Players)
        {
            PerformAction(player, input);
        }
    }

    private void PerformAction(PlayerController player, int input)
    {
        List<ActionSlot> slots = PlayerData[ClashSceneUIManager.Instance.Players.IndexOf(player)].Key;//TODO FIX ME
        foreach (ActionSlot slot in slots.Where(slot => slot.slotId == input).Where(slot => (object)slot.CurrentItem != null))
        {
            ActionType type = ((ActionItem)slot.CurrentItem).Type;
            if (type.ApplyForce) player.ApplyForce(type.Force);
            if (type.Attack) player.Attack();
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Type Safety", "UNT0010:Component instance creation", Justification = "<Pending>")]
    private void InitActions(int amount)
    {
        PlayerData = new List<KeyValuePair<List<ActionSlot>, List<ActionItem>>>();
        //amount of players
        for (int i = 0; i < 2; i++)
        {
            List<ActionItem> actions = new List<ActionItem>();
            List<ActionSlot> slots = new List<ActionSlot>();
            for (int j = 0; j < amount; j++)
            {
                ActionSlot slot = new ActionSlot
                {
                    slotId = j
                };
                slots.Add(slot);
            }

            ActionItem actionItem = new ActionItem
            {
                Type = ActionTypes[random.Next(ActionTypes.Count)]
            };
            actions.Add(actionItem);

            PlayerData.Add(new KeyValuePair<List<ActionSlot>, List<ActionItem>>(slots, actions));
        }
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
                rounds++;
                break;
        }
        State = newState;

    }
}
