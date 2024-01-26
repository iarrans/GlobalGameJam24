using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    public Card buttonCard;

    public void ChooseCard()
    {
        GameManager.instance.ApplyCardResults(buttonCard);
    }
}
