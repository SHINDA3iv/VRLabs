using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class table : MonoBehaviour
{
    public bool isComplete = false;

    public static table instance;

    void Awake()
    {
        instance = this;
    }

    private void OnTriggerStay(Collider coll)
    {
        if (coll.gameObject.tag == "AK")
        {
            if (AKManager.instance.getisCompm() && !isComplete)
            {
                isComplete = true;
                Debug.Log("updateScore");
                AKManager.instance.UpdateScore();
            }
        }
    }
}
