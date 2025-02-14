using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.InputSystem.Android;
using TMPro;

public class MarbleController : MonoBehaviour
{
    private AttitudeSensor attitudeSensor; 
    private Rigidbody marbleRb; 
    public float maxTiltAngle = 30.0f; 
    public float tiltForce = 1.0f; 
    public float jumpForce = 100.0f;
    private bool isOnFloor = true; 
    public Text debugText; 
    public float deadZone = 2.0f; 

    // Start is called before the first frame update
    void Start()
    {
        marbleRb = GetComponent<Rigidbody>();
        // attitudeSensor = AttitudeSensor.current;
        attitudeSensor = InputSystem.GetDevice<AndroidGameRotationVector>();
        if (attitudeSensor == null)
        {
            Debug.LogError("No Attitude Sensor found!");
        } else {
            InputSystem.EnableDevice(attitudeSensor);
        }
    }

    // Update is called once per frame
    void Update()
    {
         if (attitudeSensor != null) {
            MarbleTilt(); 
         } else {
            debugText.text = "No Attitude Sensor found!";
         }

         // PC testing 
         MarbleMove(); 
    }

    // control using PC for testing
    void MarbleMove() {
        float horizontalInput = Input.GetAxis("Horizontal"); 
        float verticalInput = Input.GetAxis("Vertical"); 
        Vector3 force = new Vector3(horizontalInput, 0, verticalInput);
        marbleRb.AddForce(force, ForceMode.Acceleration);
    }

    void MarbleTilt() {
        Vector3 attitude = attitudeSensor.attitude.ReadValue().eulerAngles; // Get device rotation
        // note that the direction for the phone is different axis compared to the marble

        float tiltX = NormalizeEulerAngle(attitude.x); // Forward-backward tilt
        float tiltY = NormalizeEulerAngle(attitude.y); // Left-right tilt
        // Get rotation shift around Z (phone's rotation)
        float tiltZ = NormalizeEulerAngle(attitude.z); 
        debugText.text = $"Tilt X: {tiltX:F2}°\nTilt Y: {tiltY:F2}\nTilt Z: {tiltZ:F2}°";

        // Convert rotationZ to radians (since Sin/Cos use radians)
        float rotationRadians = tiltZ * Mathf.Deg2Rad;

        // Adjust tilt direction based on rotation shift
        float correctedTiltX = tiltX * Mathf.Cos(rotationRadians) + tiltY * Mathf.Sin(rotationRadians);
        float correctedTiltY = -tiltX * Mathf.Sin(rotationRadians) + tiltY * Mathf.Cos(rotationRadians);

        // Clamp tilt angles to a max of 30 degrees
        correctedTiltX = Mathf.Clamp(correctedTiltX, -maxTiltAngle, maxTiltAngle) / maxTiltAngle;
        correctedTiltY = Mathf.Clamp(correctedTiltY, -maxTiltAngle, maxTiltAngle) / maxTiltAngle;

        // Apply force based on tilt direction
        Vector3 force = new Vector3(correctedTiltY, 0, -correctedTiltX) * tiltForce;
        marbleRb.AddForce(force, ForceMode.Acceleration);
    }

    public void marbleJump() {
        if (isOnFloor) {
            Vector3 force = new Vector3(0, 1, 0); 
            marbleRb.AddForce(force * jumpForce, ForceMode.Impulse);
            isOnFloor = false;
            UIManager.Instance.PlayJumpSound(); 
        }
    }

    float NormalizeEulerAngle(float angle) {
        if (angle > 180) {
            angle -= 360; 
        }
        return angle; 
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Floor")) {
            isOnFloor = true;
        } 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Invoke("DelayedReset", 1f);
        } else if (other.gameObject.CompareTag("Goal")) {
            UIManager.Instance.LevelUp(); 
        }
    }

    void DelayedReset()
    {
        UIManager.Instance.resetLevel();
    }
}
