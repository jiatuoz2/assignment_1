using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DoorController : MonoBehaviour
{
    public GameObject redZone; 
    public GameObject greenZone; 
    public GameObject Door; 
    public float rotationAngle = -90.0f; 
    private bool isOpen = false; 
    private float originalYRotation;

    // Start is called before the first frame update
    void Start()
    {
        redZone.SetActive(true);
        greenZone.SetActive(false); 
        originalYRotation = Door.transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            redZone.SetActive(false);
            greenZone.SetActive(true); 
            StartCoroutine(openDoor(1.0f));
        }
    }

    private IEnumerator openDoor(float time) {
        if (!isOpen) {
            isOpen = true;
            UIManager.Instance.PlayDoorOpenSound();
            float timeElapsed = 0;

            while (timeElapsed < time) {
                Door.transform.Rotate(0, rotationAngle * Time.deltaTime, 0);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            Door.transform.rotation = Quaternion.Euler(0, originalYRotation + rotationAngle, 0);
        }
    }
}
