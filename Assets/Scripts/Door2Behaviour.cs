using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door2Behaviour : MonoBehaviour
{
    public Transform posOpen;
    public Transform posDefault;
    private bool open = false;
    private bool switch1 = false;
    private bool switch2 = false;

    private void UpdateDoorState()
    {
        if (switch1 == true && switch2 == true)
        {
            OpenDoor();
        }
        else if (switch1 == false || switch2 == false)
        {
            CloseDoor();
        } 
    }

    private void OpenDoor()
    {
        if (open == false)
        {
            transform.position = posOpen.transform.position;
            open = true;
        }

    }

    private void CloseDoor()
    {      
        if (open == true)
        {
            transform.position = posDefault.transform.position;
            open = false;
        }
    }

    public void Switch1()
    {
        if (switch1 == true)
        {
            switch1 = false;
        }
        else
        {
            switch1 = true;
        }

        UpdateDoorState();
    }

    public void Switch2()
    {
        if (switch2 == true)
        {
            switch2 = false;
        }
        else
        {
            switch2 = true;
        }

        UpdateDoorState();
    }
}
