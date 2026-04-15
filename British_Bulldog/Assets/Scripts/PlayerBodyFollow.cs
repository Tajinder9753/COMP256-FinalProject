using UnityEngine;

public class PlayerBodyFollow : MonoBehaviour
{
    public Transform headCamera;
    public float bodyHeight = 1.0f;

    void Update()
    {
        if (headCamera == null) return;

        transform.position = new Vector3(
            headCamera.position.x,
            bodyHeight,
            headCamera.position.z
        );
    }
}