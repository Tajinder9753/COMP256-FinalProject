using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Transform player;
    public Transform startPoint;

    public TMP_Text messageText;
    public TMP_Text scoreText;

    private int score = 0;

    void Start()
    {
        StartRound();
    }

    public void StartRound()
    {
        player.position = startPoint.position;
        messageText.text = "RUN!";
    }

    public void PlayerReachedFinish()
    {
        score++;
        scoreText.text = "Score: " + score;
        messageText.text = "You Win!";
        Invoke("StartRound", 2f);
    }

    public void PlayerCaught()
    {
        messageText.text = "CAUGHT!";
        Invoke("StartRound", 2f);
    }
}