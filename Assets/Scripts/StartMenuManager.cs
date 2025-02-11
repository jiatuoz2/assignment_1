using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
    public GameObject startGamePanel;
    public GameObject levelSelectPanel;
    public Button level2Button;
    public Button level3Button;

    // Start is called before the first frame update
    void Start()
    {
        startGamePanel.SetActive(true);
        levelSelectPanel.SetActive(false);

        int highestLevel = PlayerPrefs.GetInt("HighestLevel", 1); 
        if (highestLevel >= 2) {
            level2Button.interactable = true;
        } else if (highestLevel >= 3) {
            level3Button.interactable = true;
        } else {
            level2Button.interactable = false;
            level3Button.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        startGamePanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }

    public void LoadLevel(int level) {
        SceneManager.LoadScene("Level " + level);
    }
}
