using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Angry Person")]
public class AngryPerson : ScriptableObject
{

    public Sprite image;
    public string personName = "John Doe";
    public int strength = 1;
    public string field;

}
