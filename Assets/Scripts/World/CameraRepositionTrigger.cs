using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraRepositionTrigger : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CinemachineVirtualCamera cinemachine;

    [Header("Settings")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform diningAreaTarget;
    [SerializeField] private Transform backroomsTarget;

    private void Start()
    {
        // Set target to dining area
        cinemachine.Follow = diningAreaTarget;
    }

    private void LateUpdate()
    {
        // Change target based on if player passes this object
        if (playerTransform.position.y > transform.position.y)
        {
            cinemachine.Follow = backroomsTarget;
        }
        else if (playerTransform.position.y < transform.position.y)
        {
            cinemachine.Follow = diningAreaTarget;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.left * 50f);
        Gizmos.DrawRay(transform.position, Vector3.right * 50f);
    }
}
