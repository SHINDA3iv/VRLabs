using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door3Behaviour : MonoBehaviour
{
    public Transform posOpen;
    public Transform posDefault;
    bool open = false;

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.name == "Door Ball")
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        if (open == false)
        {
            transform.position = posOpen.transform.position;
            open = true;
        }
    } 
}
