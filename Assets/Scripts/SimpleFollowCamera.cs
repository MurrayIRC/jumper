using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SimpleFollowCamera : MonoBehaviour {
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float followAngle = 15f;
    [SerializeField] private float followDistance = 13.5f;
    [SerializeField] private float followLag = 0f;

    private Vector3 directionFromTarget;
    private Vector3 desiredPosition;

    private void Awake() {
        Debug.Assert(cameraTarget != null);
    }

    private void Update() {
        directionFromTarget = Quaternion.Euler(-90f - followAngle, 0f, 0f) * cameraTarget.forward;
        desiredPosition = cameraTarget.position + (directionFromTarget.normalized * followDistance);
        Debug.DrawLine(cameraTarget.position, desiredPosition, Color.green);
    }

    private void LateUpdate() {
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followLag * Time.deltaTime);
        transform.LookAt(cameraTarget);
    }
}
