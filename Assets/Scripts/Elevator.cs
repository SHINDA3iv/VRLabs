using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float moveSpeed = 2f;
    public bool isMoving = false;
    private bool isMovingToEnd = true;

    private bool playerOnPlatform = false;

    private GameObject enterObject;
    public GameObject startWall;
    public GameObject endWall;

    public Transform player;
    private BoxCollider platformCollider;

    private void Start()
    {
        enterObject = transform.Find("Enter").gameObject;
        platformCollider = GetComponent<BoxCollider>();
        UpdateEnterObjectState();
    }

    void Update()
    {
        CheckPlayerOnPlatform();

        if (isMoving)
        {
            MoveElevator();
        }
    }

    private void MoveElevator()
    {
        float step = moveSpeed * Time.deltaTime;

        if (isMovingToEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPoint.position, step);

            if (Vector3.Distance(transform.position, endPoint.position) < 0.001f)
            {
                isMoving = false;
                isMovingToEnd = false;
                player.SetParent(null);
                endWall.SetActive(false);
                startWall.SetActive(true);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPoint.position, step);

            if (Vector3.Distance(transform.position, startPoint.position) < 0.001f)
            {
                isMoving = false;
                isMovingToEnd = true;
                player.SetParent(null);
                startWall.SetActive(false);
                endWall.SetActive(true);
            }
        }
        UpdateEnterObjectState();
    }

    private void CheckPlayerOnPlatform()
    {
        Transform gaze = player.transform.Find("CameraOffset").Find("Gaze Interactor");
        Vector3 platformCenter = platformCollider.bounds.center;
        Vector3 platformSize = platformCollider.bounds.size;

        if (gaze.position.x >= platformCenter.x - platformSize.x / 2 && gaze.position.x <= platformCenter.x + platformSize.x / 2 &&
            gaze.position.y >= platformCenter.y - platformSize.y / 2 && gaze.position.y <= platformCenter.y + platformSize.y / 2 &&
            gaze.position.z >= platformCenter.z - platformSize.z / 2 && gaze.position.z <= platformCenter.z + platformSize.z / 2)
        {
            playerOnPlatform = true;
            player.SetParent(this.transform);
        }
        else
        {
            playerOnPlatform = false;
            player.SetParent(null);
        }
    }

    public void StartMoving()
    {
        if (playerOnPlatform)
        {
            isMoving = true;
            player.SetParent(this.transform);
        }
        else
        {
            isMoving = false;
            player.SetParent(null);
        }
    }

    private void UpdateEnterObjectState()
    {
        if (Vector3.Distance(transform.position, startPoint.position) < 0.1f ||
            Vector3.Distance(transform.position, endPoint.position) < 0.1f)
        {
            enterObject.SetActive(false);
        }
        else
        {
            enterObject.SetActive(true);
        }
    }
}