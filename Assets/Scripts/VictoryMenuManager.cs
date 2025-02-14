using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class VictoryMenuManager : MonoBehaviour
{
    private int timeTaken; 
    private int CollectibleCount; 
    public TextMeshProUGUI statsText;

    // Start is called before the first frame update
    void Start()
    {
        timeTaken = PlayerPrefs.GetInt("Time", 0);
        CollectibleCount = PlayerPrefs.GetInt("Collectibles", 0);
        int minutes = (int)(timeTaken / 60); 
        int seconds = (int)(timeTaken % 60); 
        string timeText = "Time: " + minutes.ToString("00") + ":" + seconds.ToString("00");
        string collectibleText = "Collectibles: " + CollectibleCount.ToString();

        statsText.text = timeText + "\n" + collectibleText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToStart() {
        SceneManager.LoadScene("Start"); 
    }

    public void ReplayLevel() {
        int currentLevel = PlayerPrefs.GetInt("HighestLevel", 2) - 1; 
        SceneManager.LoadScene("Level " + currentLevel); 
    }

    public void NextLevel() {
        int NextLevel = PlayerPrefs.GetInt("HighestLevel", 2); 
        SceneManager.LoadScene("Level " + NextLevel);
    }
}
