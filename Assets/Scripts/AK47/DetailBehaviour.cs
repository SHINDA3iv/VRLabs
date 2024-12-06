using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DetailBehaviour
{
    public class DetailBehaviour : MonoBehaviour
    {
        public bool isInSlot = false;

        public bool isInHand = false;

        public void ChangeIsInSlot()
        {
            isInSlot = !isInSlot;
        }

        public bool IsInSlot()
        {
            return isInSlot;
        }

        public bool getIsInHand()
        {
            return isInHand;
        }

        public void setIsInHand(bool newIsInHand)
        {
            isInHand = newIsInHand;
        }
    }
}
