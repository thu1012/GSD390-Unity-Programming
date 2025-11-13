using System;
using UnityEngine;

/// Attribtues (line 13)
///     Tells the editor how to display these fields in the Inspector. Allows modification during testing.
/// Properties (line 32)
///     Used for general properties of the class, s.t. threshold to fire event
///     Encapsulates the distance field, s.t. only this class can change, while allowing others for access.
/// Event (line 42)
///     Allows other scripts to subscribe and react accordingly when threshold is reached.
/// Update (line 103)

public class CameraControl : MonoBehaviour
{
    // ==========
    // ATTRIBUTES
    // ==========
    [Header("Movement Settings")]
    [SerializeField]
    private float speed = 3f;

    [Header("Distance Settings")]
    [SerializeField]
    private float distanceThreshold = 5f;

    [Header("Mouse Look Settings")]
    [SerializeField]
    private float mouseSensitivity = 2f;

    [SerializeField]
    private float pitchLimit = 80f;

    // ===========
    // PROPERTIES
    // ===========
    public float DistanceTravelled { get; private set; }

    private Vector3 startPosition;
    private bool thresholdFired;
    private Transform cameraTransform;
    private float currentPitch = 0f;

    // =======
    // EVENTS
    // =======
    public event Action<float> OnDistanceThresholdReached;

    private void Start()
    {
        Camera mainCam = Camera.main;
        cameraTransform = mainCam.transform;

        // Subscribe to own event
        OnDistanceThresholdReached += distance =>
        {
            Debug.Log($"[PlayerControl] Event fired: distance threshold reached at {distance:F2} units.");
        };

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // =========
    // UPDATE
    // =========
    // Update is used to invoke player movement,
    // camera direction, and distance.
    private void Update()
    {
        HandleMovement();
        HandleMouseLook();
        UpdateDistanceAndEvent();
    }

    // Handles WASD player movement
    private void HandleMovement()
    {
        if (cameraTransform == null)
            return;

        float horizontal = Input.GetAxis("Horizontal"); // A / D
        float vertical = Input.GetAxis("Vertical");     // W / S

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * vertical + camRight * horizontal;
        if (moveDir.sqrMagnitude > 0.001f)
        {
            transform.position += moveDir * speed * Time.deltaTime;
        }
    }


    // Handles camera rotation based on mouse
    private void HandleMouseLook()
    {
        if (cameraTransform == null)
        {
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(0f, mouseX, 0f);
        currentPitch -= mouseY;
        currentPitch = Mathf.Clamp(currentPitch, -pitchLimit, pitchLimit);

        Vector3 camEuler = cameraTransform.localEulerAngles;
        camEuler.x = currentPitch;
        camEuler.y = 0f;
        camEuler.z = 0f;

        cameraTransform.localEulerAngles = camEuler;
    }


    // ======
    // EVENT
    // ======
    // Updates distance property, logs it, and fires event if threshold is hit
    private void UpdateDistanceAndEvent()
    {
        DistanceTravelled = Vector3.Distance(startPosition, transform.position);
        //Debug.Log($"[CameraControl] DistanceTravelled = {DistanceTravelled:F2}");

        // Fire the event when distance threshold reached
        if (!thresholdFired && DistanceTravelled >= distanceThreshold)
        {
            thresholdFired = true;
            Debug.Log($"[CameraControl] Threshold {distanceThreshold} reached! Raising event.");
            OnDistanceThresholdReached?.Invoke(DistanceTravelled);
        }
    }
}
