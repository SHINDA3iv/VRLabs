using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBallScript : MonoBehaviour
{
    public Collider targetCollider;
    public Transform teleportPos;

    void Update()
    {
        if (targetCollider != null)
        {
            Bounds bounds = targetCollider.bounds;

            if (!bounds.Contains(transform.position))
            {
                transform.position = teleportPos.position;
            }
        }
    }
}
