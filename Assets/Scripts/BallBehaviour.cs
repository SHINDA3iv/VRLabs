using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    private Transform teleportTarget;
    private Rigidbody rb;
    private bool isColide = false;
    private float idleTime = 0.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject targetObject = GameObject.Find("teleportTransform");
        if (targetObject != null)
        {
            teleportTarget = targetObject.transform;
        }
    }

    void Update()
    {
        CheckIfBallOutOfBounds();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Abroad") && !isColide)
        {
            isColide = true;
            TeleportToTarget();
            Invoke("CheckPinsAfterDelay", 2.0f);
        }
        else if (collision.gameObject.CompareTag("Pin") && !isColide)
        {
            isColide = true;
            Invoke("TeleportToTarget", 4.0f);
            Invoke("CheckPinsAfterDelay", 4.0f);
        }
        else if (collision.gameObject.CompareTag("Floor"))
        {
            CheckIfBallIdle();
        }
        else if (collision.gameObject.CompareTag("ballDoorSystem"))
            TeleportToTarget();
    }

    void CheckPinsAfterDelay()
    {
        GameManager.gameManager.CheckPins(); // Вызываем метод после задержки
        isColide = false;
    }

    void TeleportToTarget()
    {
        if (teleportTarget != null)
        {
            transform.position = teleportTarget.position;
        }
    }

    void CheckIfBallOutOfBounds()
    {
        if (transform.position.y < -1)
        {
            TeleportToTarget();
        }
    }

    void CheckIfBallIdle()
    {
        float velocityThreshold = 0.01f;  // Порог скорости для определения неподвижности
        float timeThreshold = 3.0f;      // Время, через которое мяч телепортируется, если неподвижен

        if (rb.velocity.magnitude < velocityThreshold) 
        {
            idleTime += Time.deltaTime;

            if (idleTime >= timeThreshold) // Если мяч неподвижен больше заданного времени
            {
                TeleportToTarget();
                GameManager.gameManager.CheckPins();
                idleTime = 0.0f; // Сбрасываем таймер
            }
        }
        else
        {
            idleTime = 0.0f; // Если мяч движется, сбрасываем таймер
        }
    }
}
