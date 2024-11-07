using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] pins; // Массив кеглей
    private Vector3[] initialPinPositions; // Изначальные позиции кеглей
    private Quaternion[] initialPinRotations; // Изначальные повороты кеглей

    private int[] scores = new int[10];
    private int rollIndex = 1;
    private int currentFrame = 1;

    private bool isStrike = false;
    private bool isSparePending = false;
    private bool isStrikePending = false;

    public static GameManager gameManager;

    GameOverScript gameOverScript;

    void Awake()
    {
        gameManager = this;
    }

    void Start()
    {
        SaveInitialPinStates();
        ScoreManager.scoreManager.UpdateScoreText(0, 0, 1, 1);
        gameOverScript = FindObjectOfType<GameOverScript>();
    }

    private void SaveInitialPinStates()
    {
        initialPinPositions = new Vector3[pins.Length];
        initialPinRotations = new Quaternion[pins.Length];
        for (int i = 0; i < pins.Length; i++)
        {
            initialPinPositions[i] = pins[i].transform.position;
            initialPinRotations[i] = pins[i].transform.rotation;
        }
    }

    public void CheckPins()
    {
        int knockedPins = 0;

        for (int i = 0; i < pins.Length; i++)
        {
            if (IsPinKnockedOver(pins[i], initialPinPositions[i], initialPinRotations[i]))
                knockedPins++;
        }

        CalculateScore(knockedPins);
        ResetPins(); // Возвращаем кегли на исходные позиции

        DebugManager.debugManager.DisplayMessage($"Сбито кеглей: {knockedPins}");
    }

    private bool IsPinKnockedOver(GameObject pin, Vector3 initialPosition, Quaternion initialRotation)
    {
        float positionThreshold = 0.3f;
        float rotationThreshold = 15f;

        if (Vector3.Distance(pin.transform.position, initialPosition) > positionThreshold)
        {
            return true;
        }

        float angleDifference = Quaternion.Angle(pin.transform.rotation, initialRotation);
        if (angleDifference > rotationThreshold)
        {
            return true;
        }

        return false;
    }

    private void CalculateScore(int knockedPins)
    {
        int frameScore = 0;

        if (isSparePending)
        {
            scores[currentFrame - 2] += knockedPins;
            isSparePending = false;
        }

        if (isStrikePending)
        {
            scores[currentFrame - 2] += knockedPins;
            isStrikePending = false;
        }

        if (isStrike)
        {
            scores[currentFrame - 1] += knockedPins;
            isStrikePending = true;
            isStrike = false;
        }

        if (knockedPins == 10 && rollIndex == 1)
        {
            scores[currentFrame - 1] += 10;
            isStrike = true;
            DebugManager.debugManager.DisplayMessage("Страйк!");
        }
        else
        {
            if (rollIndex == 2 && scores[currentFrame - 1] == 10)
            {
                isSparePending = true;
                DebugManager.debugManager.DisplayMessage("Страйк!");
            }

            scores[currentFrame - 1] += knockedPins;
            rollIndex++;
        }

        if (rollIndex > 2 || knockedPins == 10)
        {
            currentFrame++;
            rollIndex = 1;
        }

        if (currentFrame > 10)
        {
            EndGame();
        }

        frameScore = scores[currentFrame - 1];
        int totalScore = GetTotalScore();
        ScoreManager.scoreManager.UpdateScoreText(frameScore, totalScore, currentFrame, rollIndex);
    }

    private void ResetPins()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            pins[i].transform.position = initialPinPositions[i];
            pins[i].transform.rotation = initialPinRotations[i];
            pins[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
            pins[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    private void EndGame()
    {
        BlockScene(true);
        DebugManager.debugManager.DisplayMessage("Игра окончена. " + "\n" + "Ваш итоговый счёт: " + GetTotalScore(), 5f);
        ResetGame();
        gameOverScript.StopGame();
        BlockScene(false);
    }

    private void BlockScene(bool isBlocked)
    {
        foreach (GameObject pin in pins)
        {
            pin.GetComponent<Rigidbody>().isKinematic = isBlocked;
        }

        BallBehaviour[] balls = FindObjectsOfType<BallBehaviour>(); 
        foreach (BallBehaviour ball in balls)
        {
            Rigidbody ballRb = ball.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                ballRb.isKinematic = isBlocked;
            }
        }
    }

    private void ResetGame()
    {
        currentFrame = 1;
        rollIndex = 1;
        isStrike = false;
        isSparePending = false;
        isStrikePending = false;

        for (int i = 0; i < scores.Length; i++)
        {
            scores[i] = 0;
        }

        ScoreManager.scoreManager.UpdateScoreText(0, 0, 1, 1);

        ResetPins();

        DebugManager.debugManager.DisplayMessage("Новая игра началась!");
    }

    public int GetTotalScore()
    {
        int total = 0;
        foreach (int score in scores)
        {
            total += score;
        }
        return total;
    }
}
