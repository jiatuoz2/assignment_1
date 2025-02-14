using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI collectibleText;
    public Button resetButton;
    public Button jumpButton;
    public Button exitButton;
    
    [Header("Game Elements")]
    public GameObject player;
    public Transform goal;
    public Rigidbody marbleRb;
    public float jumpForce = 5f;
    
    [Header("Audio Clips")]
    private AudioSource audioSource;
    public AudioClip rollingSound;
    public AudioClip hitWallSound;
    public AudioClip jumpSound;
    public AudioClip doorOpenSound;
    public AudioClip collectibleNearbySound;
    public AudioClip collectibleCollectedSound;
    public AudioClip goalReachedSound;
    public GameObject collectEffect;

    public static UIManager Instance; 
    private float timeElapsed = 0f;
    private int collectibleCount = 0;

    // Start is called before the first frame update
    void Start()
    {   
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject); 
        }
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timerControl(); 
    }

    void timerControl() {
        timeElapsed += Time.deltaTime;
        int minutes = (int)(timeElapsed / 60);
        int seconds = (int)(timeElapsed % 60); 
        timerText.text = $"Time: {minutes:00}:{seconds:00}";
    }

    public void resetLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void exitLevel() {
        SceneManager.LoadScene("Start"); 
    }

    public void Collect(Vector3 position) {
		audioSource.PlayOneShot(collectibleCollectedSound);
		Instantiate(collectEffect, position, Quaternion.identity);

        collectibleCount++;
        collectibleText.text = "Collectibles: " + collectibleCount;
    }

    public void CollectibleNearbySound() {
        audioSource.PlayOneShot(collectibleNearbySound); 
    }

    public void LevelUp() {
        audioSource.PlayOneShot(goalReachedSound); 

        // get current level
        string levelName = SceneManager.GetActiveScene().name;
        string[] parts = levelName.Split(' ');
        int currentLevel = int.Parse(parts[1]); 

        PlayerPrefs.SetInt("HighestLevel", currentLevel + 1); 
        PlayerPrefs.SetInt("Collectibles", collectibleCount); 
        PlayerPrefs.SetInt("Time", (int)timeElapsed); 
        SceneManager.LoadScene("Victory"); 
    }

    public void PlayJumpSound() {
        audioSource.PlayOneShot(jumpSound); 
    }

    public void PlayDoorOpenSound() {
        audioSource.pitch = 3.0f;
        audioSource.PlayOneShot(doorOpenSound);
        audioSource.pitch = 1.0f;
    }
}
