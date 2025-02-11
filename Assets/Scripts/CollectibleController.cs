using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour {
	public float rotationSpeed;
	public AudioClip collectSound;
	public GameObject collectEffect;
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
	}

    void CheckZoneTresspass() {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= tapZoneRadius) {
            // start rotating 
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime); 
            tappable = true; 
        } else {
            tappable = false;
        }
    }

    void CheckTap(Vector2 touchPosition) {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            if (hit.collider.gameObject == gameObject && tappable) {
                Collect();
            }
        }
    }

	public void Collect()
	{
		if(collectSound)
			AudioSource.PlayClipAtPoint(collectSound, transform.position);
		if(collectEffect)
			Instantiate(collectEffect, transform.position, Quaternion.identity);

		Destroy (gameObject);
	}
}
