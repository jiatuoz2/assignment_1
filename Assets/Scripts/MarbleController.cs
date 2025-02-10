using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class MarbleController : MonoBehaviour
{
    private AttitudeSensor attitudeSensor; 
    private Rigidbody marbleRb; 
    public float maxTiltAngle = 30.0f; 
    public float tiltForce = 1.0f; 
    public float jumpForce = 100.0f;
    private bool isOnFloor = true; 

    // Start is called before the first frame update
    void Start()
    {
        marbleRb = GetComponent<Rigidbody>();
        attitudeSensor = AttitudeSensor.current;
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
         }
         if (Input.GetKeyDown(KeyCode.Space) && isOnFloor) {
             marbleJump(); 
             isOnFloor = false; 
         }
    }

    void MarbleTilt() {
        Vector3 attitude = attitudeSensor.attitude.ReadValue().eulerAngles; // Get device rotation

        float tiltX = NormalizeEulerAngle(attitude.x); // Forward-backward tilt
        float tiltZ = NormalizeEulerAngle(attitude.z); // Left-right tilt

        // Clamp tilt angles to a max of 30 degrees
        tiltX = Mathf.Clamp(tiltX, -maxTiltAngle, maxTiltAngle) / maxTiltAngle;
        tiltZ = Mathf.Clamp(tiltZ, -maxTiltAngle, maxTiltAngle) / maxTiltAngle;

        // Apply force based on tilt direction
        Vector3 force = new Vector3(tiltZ, 0, -tiltX) * tiltForce;
        marbleRb.AddForce(force, ForceMode.Acceleration);
    }

    void marbleJump() {
        Vector3 force = new Vector3(0, 1, 0); 
        marbleRb.AddForce(force * jumpForce, ForceMode.Impulse);
    }

    float NormalizeEulerAngle(float angle) {
        if (angle > 180) {
            angle -= 180; 
        }
        return angle; 
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Floor")) {
            isOnFloor = true;
        }
    }
}
