using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float damping = 5f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float scrollSpeed = 1f;
    [SerializeField] private float titleAngle = 45f;
    [SerializeField] private float panAngle = -45f;

    private string triggerEnterTag = "Player";
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag(triggerEnterTag).transform;
    }

    private void LateUpdate()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float distance = offset.magnitude;

        distance -= scroll * scrollSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        offset = Quaternion.Euler(titleAngle, panAngle, 0f) * new Vector3(0f, 0f, -maxDistance);
        offset = offset.normalized * distance;
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * damping);

        Vector3 targetDirection = Vector3.Normalize(target.position - transform.position);
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * damping);
    }

}
