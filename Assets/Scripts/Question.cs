using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "Cards/Question", order = 0)]
public class Question : ScriptableObject
{
    public string enunciado;

    public Card Option1;

    public Card Option2;
}
