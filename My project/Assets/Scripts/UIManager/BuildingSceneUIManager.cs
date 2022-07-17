using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSceneUIManager : MonoBehaviour
{
    public static BuildingSceneUIManager Instance { get; private set; }

    public ActionItem ActionPrefab;
    public ActionSlot ActionSlot; //Inv and dice-eyes

    
    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        InitBuildingScene();
    }

    private void InitBuildingScene()
    {
        //TODO FIX ME: Slots and SlotCanvas Object both have canvas components
        Canvas canvas = FindObjectOfType<Canvas>();

        Transform slots = Instantiate(canvas.transform);
        slots.name = "Slots";
        slots.SetParent(canvas.transform);

        GridLayoutGroup grid = slots.gameObject.AddComponent<GridLayoutGroup>();
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = GameManager.Instance.SlotAmount;
        grid.cellSize = ActionSlot.GetComponent<RectTransform>().sizeDelta;
        grid.spacing = new Vector2(10, 100); //TODO FIX ME!!!
        grid.padding = new RectOffset(20, 20, 20, 20);

        List< ActionSlot> NewSlots = new List<ActionSlot> ();
        for (int i = 0; i < GameManager.Instance.SlotsBuildingPlayer.Count; i++)
        {
            ActionSlot slot = GameManager.Instance.SlotsBuildingPlayer[i];

            ActionSlot actionSlot = Instantiate(ActionSlot, slots);
            actionSlot.slotId = slot.slotId;

            ActionItem action = null;
            if (slot.CurrentItem != null)
            {
                action = InstantiateActionItem(actionSlot, (ActionItem)slot.CurrentItem);
            }
            actionSlot.CurrentItem = action;

            NewSlots.Add(actionSlot);
        }

        int lastIndex = GameManager.Instance.SlotsBuildingPlayer[GameManager.Instance.SlotAmount - 1].slotId + 1;

        foreach (ActionItem action in GameManager.Instance.Actions)
        {
            ActionSlot actionSlot = Instantiate(ActionSlot, slots);
            actionSlot.slotId = lastIndex;

            ActionItem actionItem = InstantiateActionItem(actionSlot, action);
            NewSlots.Add(actionSlot);

            lastIndex++;
        }

        GameManager.Instance.SlotsBuildingPlayer = NewSlots;
    }

    private ActionItem InstantiateActionItem(ActionSlot slot, ActionItem parent)
    {
        ActionItem actionItem = Instantiate(ActionPrefab, slot.transform);
        actionItem.Type = parent.Type;
        actionItem.currentSlot = slot;
        actionItem.GetComponentInChildren<TMP_Text>().text = actionItem.Type.Name;
        slot.CurrentItem = actionItem;

        return actionItem;
    }

    public void ButtonPressedBack()
    {
        GameManager.Instance.SwitchScene(GameState.MENU);
    }

    public void ButtonPressedClash()
    {
        GameManager.Instance.SwitchScene(GameState.CLASH);
    }


}
