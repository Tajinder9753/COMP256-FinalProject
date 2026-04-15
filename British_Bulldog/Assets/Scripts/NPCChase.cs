using UnityEngine;

public class NPCChase : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;

    public GameManager gameManager;

    void Update()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;

        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameManager != null)
            {
                gameManager.PlayerCaught();
            }
            else
            {
                Debug.LogWarning("GameManager not assigned to NPC!");
            }
        }
    }
}