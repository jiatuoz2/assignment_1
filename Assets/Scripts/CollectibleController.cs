using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class CollectibleController : MonoBehaviour {
	public float rotationSpeed = 3;
    public GameObject player; 
    public float tapZoneRadius = 2.0f; 
    private bool tappable = false; 

	// Use this for initialization
	void Start() {
        
	}
	
	// Update is called once per frame
	void Update() {
		CheckZoneTresspass(); 
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            Vector2 touchPosition = Input.GetTouch(0).position;
            CheckTap(touchPosition); 
        }

        // for pc testing 
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePosition = Input.mousePosition;
            checkClick(mousePosition);
        }
	}

    void CheckZoneTresspass() {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= tapZoneRadius) {
            // start rotating 
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime); 
            if (tappable == false) {
                UIManager.Instance.CollectibleNearbySound();
                tappable = true; 
            }
        } else {
            tappable = false;
        }
    }

    void CheckTap(Vector2 touchPosition) {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            if (hit.collider.gameObject == gameObject && tappable) {
                Collect(transform.position);
            }
        }
    }

    // for PC testing 
    void checkClick(Vector2 clickPosition) {
        Ray ray = Camera.main.ScreenPointToRay(clickPosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2f); // Draw ray in Scene view
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            if (hit.collider.gameObject == gameObject && tappable) {
                Collect(transform.position);
            }
        }
    }

    void Collect(Vector3 position) {
        UIManager.Instance.Collect(position); 
        Destroy(gameObject); 
    }
}
