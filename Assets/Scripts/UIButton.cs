using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    public Card buttonCard;

    public TextMeshProUGUI statement;

    public Image risaStat;
    public Image audienciaStat;
    public Image familyFriendlyStat;

    //asognar iconos de los botones para poder cambiarlos desde le manaher
    public void ChooseCard()
    {
        GameManager.instance.ApplyCardResults(buttonCard);
    }
}
