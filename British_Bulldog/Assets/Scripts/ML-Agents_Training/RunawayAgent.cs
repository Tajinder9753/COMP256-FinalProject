using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class RunAwayAgent : Agent
{
    public Transform player;
    public Transform target;
    public float speed = 6f;

    private Rigidbody rb;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 agentPos;
        Vector3 playerPos;

        int attempts = 0;


        //Spawns agent and player at random positions
        do
        {
            agentPos = new Vector3(
                Random.Range(-4f, 4f),
                0.5f,
                Random.Range(-4f, 4f)
            );

            playerPos = new Vector3(
                Random.Range(-4f, 4f),
                0.5f,
                Random.Range(-4f, 4f)
            );

            attempts++;
            if (attempts > 50) break;

        } while (Vector3.Distance(agentPos, playerPos) < 15.5f); // to try to keep them a little far apart so they aren't spawning directly next to each other 

        transform.localPosition = agentPos;
        player.localPosition = playerPos;

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // direction to player (danger)
        sensor.AddObservation(player.localPosition - transform.localPosition);

        // direction to target (goal)
        sensor.AddObservation(target.localPosition - transform.localPosition);

        // velocity
        sensor.AddObservation(rb.linearVelocity.x);
        sensor.AddObservation(rb.linearVelocity.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 move = new Vector3(
            actions.ContinuousActions[0],
            0,
            actions.ContinuousActions[1]
        );

        rb.linearVelocity = new Vector3(
            move.x * speed,
            rb.linearVelocity.y,
            move.z * speed
        );

        float distance = Vector3.Distance(transform.localPosition, target.localPosition);
        float distPlayer = Vector3.Distance(transform.localPosition, player.localPosition);

        // reward moving toward target
        AddReward(-distance * 0.001f);

        // reward staying away from player
        AddReward(distPlayer * 0.001f);

        // caught by player
        if (distPlayer < 1.5f)
        {
            AddReward(-1f);
            EndEpisode();
        }

        // reached target
        if (distance < 1.5f)
        {
            AddReward(1f);
            EndEpisode();
        }

        // small survival penalty, so the agent doesn't just stand still or hide somewhere 
        AddReward(-0.001f);
    }
}