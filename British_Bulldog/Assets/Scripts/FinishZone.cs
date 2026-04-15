using UnityEngine;

public class FinishZone : MonoBehaviour
{
    public GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered finish zone: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached finish zone!");

            if (gameManager != null)
            {
                gameManager.PlayerReachedFinish();
            }
            else
            {
                Debug.LogWarning("GameManager is not assigned in FinishZone.");
            }
        }
    }
}