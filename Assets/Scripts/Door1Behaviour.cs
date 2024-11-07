using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door1Behaviour : MonoBehaviour
{
    public Transform posOpen;
    public Transform posClose;
    bool open = false;

    public void OpenDoor()
    {
        if (open == false)
        {
            transform.position = posOpen.transform.position;
            open = true;

            Invoke("CloseDoor", 10.0f);
        }
        else
        {
            CancelInvoke();
            Invoke("CloseDoor", 10.0f);
        }
    }

    public void CloseDoor()
    {
        if (open == true)
        {
            transform.position = posClose.transform.position;
            open = false;
        }
    }
}
