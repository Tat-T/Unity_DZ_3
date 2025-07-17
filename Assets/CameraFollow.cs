using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    private void Start()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }

    private void FixedUpdate()
    {
        if (target == null) return;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, target.position + new Vector3(5, 3, 0), Time.deltaTime);

        smoothedPosition.z = transform.position.z;
        transform.position = smoothedPosition;
    }

}
