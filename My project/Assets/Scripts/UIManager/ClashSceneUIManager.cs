using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;


public class ClashSceneUIManager : MonoBehaviour
{
    public static ClashSceneUIManager Instance { get; private set; }
    public PostProcessVolume postProcessVolume;
    public DiceUIManager diceUIManager;
    public ShopItem ShopItem;

    public int fadeTimeDOF;

    public List<PlayerController> Players;

    public void Awake()
    {
        Instance = this;
        GameManager.Instance.OnStateChange += OnStateChanged;
    }

    public void Start()
    {
        StartCoroutine(DisableDice());
    }

    public IEnumerator UITurn()
    {
        yield return EnableDice();
        yield return new WaitWhile(diceUIManager.IsRotating);
        yield return DisableDice();
        yield return diceUIManager.Result;
    }

    private void OnStateChanged(GameState oldS, GameState newS)
    {
        if(newS == GameState.SHOP)
        {
            foreach(GameObject o in GameObject.FindGameObjectsWithTag("ClashNoShop"))
            {
                o.SetActive(false);
            }
            foreach (GameObject o in GameObject.FindGameObjectsWithTag("ClashShop"))
            {
                o.SetActive(true);
            }
            InitShop();
        }
    }

    private void InitShop()
    {
        Transform parent = GameObject.FindGameObjectWithTag("ClashShop").transform;
        foreach(var player in Players)
        {
            for(int i = 0; i < 4; i++)
            {
                ShopItem item = Instantiate(ShopItem, parent);
                ActionType type = GameManager.Instance.RandomAction();
                Players[i].Score = +50;
                item.ActionName.text = type.Name;
                item.CostText.text = type.Cost.ToString();
                item.BuyerText.text = player.name;
                item.player = player;
                item.type = type;
            }
        }
    }

    public void DecreaseScore(PlayerController player, int amount)
    {
        int index = Players.IndexOf(player);

    }

    private IEnumerator EnableDice()
    { 
        DepthOfField setting = postProcessVolume.profile.GetSetting<DepthOfField>();
        setting.focusDistance.Override(10);
        for (float i = 0; i < fadeTimeDOF + 1; i++)
        {
            setting.focusDistance.Interp(10, 2, i/ fadeTimeDOF);
            yield return new WaitForSeconds(1f / fadeTimeDOF);
        }
        diceUIManager.gameObject.SetActive(true);
        diceUIManager.StartRotate();
        yield return null;
    }

    private IEnumerator DisableDice()
    {
        diceUIManager.gameObject.SetActive(false);
        DepthOfField setting = postProcessVolume.profile.GetSetting<DepthOfField>();
        setting.focusDistance.Override(2);
        for (float i = 0; i < fadeTimeDOF + 1; i++)
        {
            setting.focusDistance.Interp(2, 10, i / fadeTimeDOF);
            yield return new WaitForSeconds(0.5f / fadeTimeDOF);
        }
        yield return null;
    }
}
