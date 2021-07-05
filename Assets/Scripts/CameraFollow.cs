using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform Target;
    private Vector3 offset;
    private float smoothSpeed = 0.5f;

    private void Start()
    {
        offset = transform.position - Target.position;
    }

    private void LateUpdate()
    {
        if (Target != null)
        {
            Vector3 DesiredPos = Target.position + offset;
            DesiredPos.x = 0;
            Vector3 SmoothedPos = Vector3.Lerp(transform.position, DesiredPos, smoothSpeed);
            transform.position = SmoothedPos;
        }
    }
}
