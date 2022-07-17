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

    public int fadeTimeDOF;

    public List<KeyValuePair<PlayerController, List<ActionSlot>>> Players { get; private set; }

    public void Awake()
    {
        Instance = this;
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

    private IEnumerator EnableDice()
    { 
        //postProcessVolume.enabled = true;
        DepthOfField setting = postProcessVolume.profile.GetSetting<DepthOfField>();
        setting.focusDistance.Override(10);
        for (float i = 0; i < fadeTimeDOF + 1; i++)
        {
            setting.focusDistance.Interp(10, 2, i/ fadeTimeDOF);
            //print()
            yield return new WaitForSeconds(1f / fadeTimeDOF);
        }
        diceUIManager.gameObject.SetActive(true);
        diceUIManager.StartRotate();
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
        //postProcessVolume.enabled = false;
        yield return null;
    }

    public void InitPlayers(List<ActionSlot> player1, List<ActionSlot> player2)
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        Players = new List<KeyValuePair<PlayerController, List<ActionSlot>>>
        {
            new KeyValuePair<PlayerController, List<ActionSlot>>(players[0], player1),
            new KeyValuePair<PlayerController, List<ActionSlot>>(players[1], player2)
        };
    }
}
