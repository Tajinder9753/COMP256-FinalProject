using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class ChaserAgent : Agent
{
    Rigidbody rBody;
    public Transform Target;

    public float forceMultiplier = 10;
    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(Target.localPosition - transform.localPosition);
        sensor.AddObservation(transform.localPosition);
        // Agent velocity
        sensor.AddObservation(rBody.linearVelocity.x); //1 float
        sensor.AddObservation(rBody.linearVelocity.z); //1 float

        //total: 8 floats
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actions.ContinuousActions[0];
        controlSignal.z = actions.ContinuousActions[1];


        Vector3 move = new Vector3(
            actions.ContinuousActions[0],
            0,
            actions.ContinuousActions[1]
        );

        rBody.linearVelocity = new Vector3(
            move.x * forceMultiplier,
            rBody.linearVelocity.y,
            move.z * forceMultiplier
        );


        // Rewards
        float distanceToTarget = Vector3.Distance(transform.localPosition, Target.localPosition);

        // reward movement toward target
        AddReward(-distanceToTarget * 0.001f);

        // reward for catching
        if (distanceToTarget < 1.5f)
        {
            SetReward(1.0f);
            EndEpisode();
        }


        // Fell off platform, probably not needed since I added walls but will keep anyways 
        else if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }

        // small penalty
        AddReward(-0.001f);
    }

    public override void OnEpisodeBegin()
    {
        // If the Agent fell, zero its momentum
        if (this.transform.localPosition.y < 0)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.linearVelocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
        }

        // Move the target to a new spot
        Target.localPosition = new Vector3(Random.value * 8 - 4,
        0.5f,
        Random.value * 8 - 4);
        //move self to new spot 
        transform.localPosition = new Vector3(
            Random.Range(-4f, 4f),
            0.5f,
            Random.Range(-4f, 4f)
        );

        rBody.linearVelocity = Vector3.zero;
        rBody.angularVelocity = Vector3.zero;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }
}