using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform xrOrigin;
    public Transform mainCamera;
    public Transform startPoint;

    public TMP_Text messageText;
    public TMP_Text scoreText;
    public TMP_Text targetScoreText;

    public ChaserAgent[] chaserAgents;
    private int score = 0;
    public int targetScore = 5;

    private void Awake()
    {
        targetScoreText.text = "Target Score: " + targetScore;
    }

    void Start()
    {
        StartRound();
    }

    public void StartRound()
    {
        ResetPlayerToStart();
        messageText.text = "RUN!";
    }

    public void PlayerReachedFinish()
    {
        score++;
        scoreText.text = "Score: " + score;
        messageText.text = "You Win!";
        if (score >= targetScore)
        {
            SceneManager.LoadScene("GameOver");
        }
        Invoke(nameof(StartRound), 2f);
    }

    public void PlayerCaught()
    {
        messageText.text = "CAUGHT!";
        //add penalty for getting caught
        if (score > 0)
        {
            score--;
            scoreText.text = "Score: " + score;
        }
        Invoke(nameof(StartRound), 2f);
    }

    private void ResetPlayerToStart()
    {
        if (xrOrigin == null || mainCamera == null || startPoint == null)
        {
            Debug.LogWarning("GameManager is missing XR Origin, Main Camera, or Start Point reference.");
            return;
        }


        //reset player
        Vector3 cameraOffset = mainCamera.position - xrOrigin.position;
        Vector3 targetPosition = startPoint.position - cameraOffset;

        xrOrigin.position = targetPosition;

        //reset agents
        foreach (ChaserAgent agent in chaserAgents)
        {
            agent.transform.position = agent.startingPoint;
            agent.rBody.linearVelocity = Vector3.zero;
        }
    }
}