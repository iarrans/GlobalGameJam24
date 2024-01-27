using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    public Card buttonCard;

    //asognar iconos de los botones para poder cambiarlos desde le manaher
    public void ChooseCard()
    {
        GameManager.instance.ApplyCardResults(buttonCard);
    }
}
