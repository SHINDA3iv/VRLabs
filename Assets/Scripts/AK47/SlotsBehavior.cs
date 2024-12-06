using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DetailBehaviour;

public class SlotsBehavior : MonoBehaviour
{
    public List<GameObject> slotsToPull = new List<GameObject>();
    public List<GameObject> detailsToPull = new List<GameObject>();

    public List<DetailBehaviour.DetailBehaviour> detailsParent = new List<DetailBehaviour.DetailBehaviour>();

    public GameObject thisDetail;

    public bool parentExist = false;

    public bool isON = false;

    private void Update()
    {
        if(AKManager.instance.getPause() || !thisDetail.GetComponent<DetailBehaviour.DetailBehaviour>().getIsInHand())
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        } else if(!parentExist && !isON && thisDetail.GetComponent<DetailBehaviour.DetailBehaviour>().getIsInHand())
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }

        foreach (DetailBehaviour.DetailBehaviour detailParent in detailsParent)
        {
            DetailBehaviour.DetailBehaviour detailsBehaviour = thisDetail.GetComponent<DetailBehaviour.DetailBehaviour>();
            if (detailsBehaviour.IsInSlot() && detailParent.IsInSlot())
            {
                thisDetail.GetComponent<BoxCollider>().enabled = false;
                parentExist = true;
                break;
            }
            else {
                thisDetail.GetComponent<BoxCollider>().enabled = true;
                parentExist = false;
            }
        }
        /*

        if (AKManager.instance.getPause())
        {
            thisDetail.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            thisDetail.GetComponent<BoxCollider>().enabled = true;
        }*/
    }

    public void TurnOffSlot()
    {
       foreach (GameObject slot in slotsToPull)
        {
            slot.GetComponent<BoxCollider>().enabled = false;
            slot.GetComponent<SlotsBehavior>().setOn(true);
        }

        /*foreach (GameObject detail in detailsToPull)
        {
            DetailBehaviour.DetailBehaviour detailsBehaviour = thisDetail.GetComponent<DetailBehaviour.DetailBehaviour>();
            if (detailsBehaviour.IsInSlot() && detail.GetComponent<DetailBehaviour.DetailBehaviour>().IsInSlot())
                detail.GetComponent<BoxCollider>().enabled = false;
        }*/
    }

    public void TurnOnSlot() 
    {
        foreach (GameObject slot in slotsToPull)
        {
            slot.GetComponent<BoxCollider>().enabled = true;
            slot.GetComponent<SlotsBehavior>().setOn(false);
        }

        /*foreach (GameObject detail in detailsToPull)
        {
            DetailBehaviour.DetailBehaviour detailsBehaviour = thisDetail.GetComponent<DetailBehaviour.DetailBehaviour>();
            if (!detailsBehaviour.IsInSlot() || !detail.GetComponent<DetailBehaviour.DetailBehaviour>().IsInSlot())
                detail.GetComponent<BoxCollider>().enabled = true;
        }*/
    }

    public void setOn(bool newIsON)
    {
        isON = newIsON;
    }

    public bool getOn(bool newIsON)
    {
        return isON;
    }
}

