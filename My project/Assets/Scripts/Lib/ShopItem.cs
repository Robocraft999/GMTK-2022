using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public TMP_Text ActionName;
    public TMP_Text CostText;
    public TMP_Text BuyerText;
    public GameObject BuyButton;
    public PlayerController player;
    public ActionType type;

    public void OnBuy()
    {
        if (GameManager.Instance.scores[player.name] - type.Cost >= 0) {
            GameManager.Instance.ChangeScore(player, -type.Cost);
            BuyButton.SetActive(false);
            int index = ClashSceneUIManager.Instance.Players.IndexOf(player);
            GameManager.Instance.AddAction(type, index);
        }
    }
}
