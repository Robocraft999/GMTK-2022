using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSceneUIManager : MonoBehaviour
{
    public static BuildingSceneUIManager Instance { get; private set; }

    public ActionItem ActionPrefab;
    public GameObject ActionSlot;
    public GameObject buttonClash;
    public GameObject buttonNext;
    public GameObject buttonPrevious;
    public GameObject playerText;

    private Transform slots;

    public int playerIndex { get; set; } = 0;
    
    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        buttonClash.SetActive(false);
        buttonNext.SetActive(true);
        buttonPrevious.SetActive(false);
        InitBuildingScene();
    }

    public void LoadBuildingDeck(PlayerController  player)
    {
        if (playerIndex == 0)
        {
            playerText.GetComponent<TMP_Text>().text = "Player 1";
        } else
        {
            playerText.GetComponent<TMP_Text>().text = "Player 2";
        }


        var PlayerSlots = player.actionSlots;
        var PlayerActions = player.actions;
        int DiceSlotAmount = GameManager.Instance.SlotAmount;

        ReloadSlots();

        List<ActionSlot> NewSlots = new List<ActionSlot>();
        List<ActionItem> NewActions = new List<ActionItem>();

        int i;
        for (i = 0; i < PlayerSlots.Count; i++)
        {
            ActionSlot slot = PlayerSlots[i];

            if (i < DiceSlotAmount || (i >= DiceSlotAmount && slot.Items.Count > 0))
            {
                GameObject Slot = Instantiate(ActionSlot, slots);
                ActionSlot actionSlot = Slot.GetComponentInChildren<ActionSlot>();
                actionSlot.slotId = slot.slotId;
                actionSlot.MaxItems = slot.MaxItems;
                if(i < DiceSlotAmount)
                    Slot.GetComponentInChildren<TMP_Text>().text = (i+1).ToString();

                ActionItem action = null;
                foreach(var item in slot.Items)
                {
                    if (item is object)
                    {
                        action = InstantiateActionItem(actionSlot, item);
                        NewActions.Add(action);
                    }
                    //actionSlot.Items.Add(action);
                }
                NewSlots.Add(actionSlot);
                
            }
        }

        int j = i;
        foreach (ActionItem action in PlayerActions.Where(item => item.currentSlot is null))
        {
            ActionSlot actionSlot = Instantiate(ActionSlot, slots).GetComponentInChildren<ActionSlot>();
            actionSlot.slotId = i;
            actionSlot.MaxItems = 1;

            ActionItem actionItem = InstantiateActionItem(actionSlot, action);
            NewSlots.Add(actionSlot);
            NewActions.Add(actionItem);
            i++;
        }
        if(i == j)
        {
            ActionSlot actionSlot = Instantiate(ActionSlot, slots).GetComponentInChildren<ActionSlot>();
            actionSlot.slotId = i;
            actionSlot.MaxItems = 1;
            NewSlots.Add(actionSlot);
            i++;
        }
        player.actionSlots = NewSlots;
        player.actions  = NewActions;
    }

    private void InitBuildingScene()
    {
        //TODO FIX ME: Slots and SlotCanvas Object both have canvas components
        Canvas canvas = FindObjectOfType<Canvas>();

        slots = Instantiate(canvas.transform);
        slots.name = "Slots";
        slots.SetParent(canvas.transform);

        GridLayoutGroup grid = slots.gameObject.AddComponent<GridLayoutGroup>();
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = GameManager.Instance.SlotAmount;
        grid.cellSize = ActionSlot.GetComponent<RectTransform>().sizeDelta;
        grid.spacing = new Vector2(10, 100); //TODO FIX ME!!!
        grid.padding = new RectOffset(20, 20, 20, 20);

        LoadBuildingDeck(GameManager.Instance.Players[playerIndex]);
    }

    private ActionItem InstantiateActionItem(ActionSlot slot, ActionItem parent)
    {
        ActionItem actionItem = Instantiate(ActionPrefab, slot.transform);
        actionItem.Type = parent.Type;
        actionItem.currentSlot = slot;
        actionItem.GetComponentInChildren<TMP_Text>().text = actionItem.Type.Name;
        slot.Items.Add(actionItem);

        return actionItem;
    }

    private void ReloadSlots()
    {
        for(int i = 0; i < slots.childCount; i++)
        {
            Destroy(slots.GetChild(i).gameObject);
        }
    }

    public void ButtonPressedBack()
    {
        GameManager.Instance.SwitchScene(GameState.MENU);
    }

    public void ButtonPressedClash()
    {
        GameManager.Instance.SwitchScene(GameState.CLASH);
    }

    public void ButtonPressedPrevious()
    {
        playerIndex--;
        buttonNext.SetActive(true);
        if (playerIndex == 0)
        {
            buttonPrevious.SetActive(false);
        }
        if (playerIndex < GameManager.Instance.Players.Count - 1)
        {
            buttonClash.SetActive(false);
        }
        LoadBuildingDeck(GameManager.Instance.Players[playerIndex]);
    }

    public void ButtonPressedNext()
    {
        playerIndex++;
        buttonPrevious.SetActive(true);
        if (playerIndex == GameManager.Instance.Players.Count - 1)
        {
            buttonNext.SetActive(false);
            buttonClash.SetActive(true);
        }
        LoadBuildingDeck(GameManager.Instance.Players[playerIndex]);
    }


}
