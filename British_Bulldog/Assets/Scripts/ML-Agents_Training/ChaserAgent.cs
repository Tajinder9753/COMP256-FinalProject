using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class ChaserAgent : Agent
{
    public Transform Target;
    public float speed = 10f;

    private Rigidbody rBody;

    public override void Initialize()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // direction to player
        sensor.AddObservation(Target.localPosition - transform.localPosition);

        // own position
        sensor.AddObservation(transform.localPosition);

        // velocity
        sensor.AddObservation(rBody.linearVelocity.x);
        sensor.AddObservation(rBody.linearVelocity.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 move = new Vector3(
            actions.ContinuousActions[0],
            0,
            actions.ContinuousActions[1]
        );

        rBody.linearVelocity = new Vector3(
            move.x * speed,
            rBody.linearVelocity.y,
            move.z * speed
        );
    }
}