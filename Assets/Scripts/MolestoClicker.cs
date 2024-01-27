using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolestoClicker : MonoBehaviour
{
    private void OnMouseDown()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        AngryPeopleManager.instance.molestoTime = 0;
    }

    private void OnTriggerEnter(Collider col)
    {
        //Debug.Log("Col");
        AngryPeopleManager.instance.dir *= -1;
    }
}
