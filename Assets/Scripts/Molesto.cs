using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Molesto")]
public class Molesto : ScriptableObject
{

    public GameObject sprite;
    public string personName = "Mol Esto";
    public int strength = 1;
    public string field;

}
