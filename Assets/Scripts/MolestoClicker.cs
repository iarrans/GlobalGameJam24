using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolestoClicker : MonoBehaviour
{
    private void OnMouseDown()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider col)
    {
        //Debug.Log("Col");
        MolestoManager.instance.dir *= -1;
    }
}
