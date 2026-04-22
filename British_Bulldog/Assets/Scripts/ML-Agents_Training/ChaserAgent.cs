using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class ChaserAgent : Agent
{
    public Transform target;
    public Transform parent;
    public float speed = 10f;
    public GameManager gameManager;
    public Rigidbody rBody;

    public override void CollectObservations(VectorSensor sensor)
    {
        //training was done in localPosition, so have to convert to local space from world space wasn't working properly otherwise
        Vector3 agentLocalPosition = parent.InverseTransformPoint(transform.position);
        Vector3 targetLocalPosition = parent.InverseTransformPoint(target.position);

        sensor.AddObservation(targetLocalPosition - agentLocalPosition);
        sensor.AddObservation(agentLocalPosition);
        sensor.AddObservation(rBody.linearVelocity.x);
        sensor.AddObservation(rBody.linearVelocity.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        rBody.linearVelocity = new Vector3(
            actions.ContinuousActions[0] * speed,
            rBody.linearVelocity.y,
            actions.ContinuousActions[1] * speed
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameManager != null)
            {
                gameManager.PlayerCaught();
            }
        }
    }
}