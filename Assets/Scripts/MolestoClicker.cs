using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolestoClicker : MonoBehaviour
{
    public Sprite face2;
    private void OnMouseDown()
    {
        //this.gameObject.SetActive(false);
        if(this.gameObject.transform.position.x > -2)
        {
            MolestoManager.instance.exit = 0;
        }
        else
        {
            MolestoManager.instance.exit = 1;
        }
        
        MolestoManager.instance.MolestoHit = true;

    }

    private void OnTriggerEnter(Collider col)
    {
        MolestoManager.instance.currentMarker = (MolestoManager.instance.currentMarker + 1) % 4;
    }
}
