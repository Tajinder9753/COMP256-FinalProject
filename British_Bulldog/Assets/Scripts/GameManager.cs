using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Transform xrOrigin;
    public Transform mainCamera;
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
        ResetPlayerToStart();
        messageText.text = "RUN!";
    }

    public void PlayerReachedFinish()
    {
        score++;
        scoreText.text = "Score: " + score;
        messageText.text = "You Win!";
        Invoke(nameof(StartRound), 2f);
    }

    public void PlayerCaught()
    {
        messageText.text = "CAUGHT!";
        Invoke(nameof(StartRound), 2f);
    }

    private void ResetPlayerToStart()
    {
        if (xrOrigin == null || mainCamera == null || startPoint == null)
        {
            Debug.LogWarning("GameManager is missing XR Origin, Main Camera, or Start Point reference.");
            return;
        }

        Vector3 cameraOffset = mainCamera.position - xrOrigin.position;
        Vector3 targetPosition = startPoint.position - cameraOffset;

        xrOrigin.position = targetPosition;
    }
}