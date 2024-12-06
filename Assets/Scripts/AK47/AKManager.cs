using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class AKManager : MonoBehaviour
{
    public List<GameObject> detailsList = new List<GameObject>();

    public bool isAssemblyComplete = false;
    public bool isDisassemblyComplete = false;

    private float playerTime = 0;
    public TMP_Text timer;
    private bool complete = false;
    private bool pause = true;

    public static AKManager instance;

    public GameObject keyboard;

    private void Awake()
    {
        instance = this;
    }

    public void ChangePause()
    {
        pause = false;
    }

    public void PauseAction()
    {
        if(pause) 
            pause = false;
    }

    public bool getPause()
    {
        return pause;
    }

    private void Update()
    {
        if (isAssemblyComplete && isDisassemblyComplete) {
            complete = true;
            pause = true;
        } else {
            complete = false;
        }    

        if (!complete && !pause && !table.instance.isComplete) {
            playerTime += Time.deltaTime;
            UpdateTimer();
        }
    }

    private void UpdateTimer()
    {
        float minutes = Mathf.FloorToInt(playerTime / 60);
        float seconds = Mathf.FloorToInt(playerTime % 60);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator GetPlayerName(List<LeaderboardEntry> scores)
    {
        if (scores.Count > 5)
        {
            foreach (LeaderboardEntry entry in scores)
            {
                if (entry.score > playerTime)
                {
                    keyboard.SetActive(true);
                    yield return new WaitUntil(() => Name.instance.done);
                    string name = Name.instance.playerName;
                    Name.instance.done = false;
                    keyboard.SetActive(false);
                    Name.instance.playerName = "";
                    entry.name = name;
                    entry.score = playerTime;
                    break;
                }
            }
        }
        else
        {
            keyboard.SetActive(true);
            yield return new WaitUntil(() => Name.instance.done);
            string name = Name.instance.playerName;
            Name.instance.done = false;
            keyboard.SetActive(false);
            Name.instance.playerName = "";
            Leaderboard.instance.AddNewScore(name, playerTime);
        }

        XMLManager.instance.SaveScores(scores);
        Leaderboard.instance.UpdateDisplay();
        SceneManager.LoadScene(3);
    }

    public void UpdateScore()
    {
        List<LeaderboardEntry> scores = XMLManager.instance.LoadScores();
        StartCoroutine(GetPlayerName(scores));
    }

    public void addDetail(GameObject detail)
    {
        detailsList.Add(detail);
        checkAssemblyComplete();
    }

    private void checkAssemblyComplete()
    {
        if(detailsList.Count == 7) {
            isAssemblyComplete = true;
        }
    }

    public void removeDetail(GameObject detail)
    {
        detailsList.Remove(detail);
        checkDisassemblyComplete();
    }

    private void checkDisassemblyComplete()
    {
        if (detailsList.Count == 0) {
            isDisassemblyComplete = true;
        }
    }

    public bool getisCompm()
    {
        return complete;
    }
}
