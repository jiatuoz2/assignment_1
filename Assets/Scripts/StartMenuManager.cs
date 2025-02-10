using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public GameObject startGamePanel;
    public GameObject levelSelectPanel;

    // Start is called before the first frame update
    void Start()
    {
        startGamePanel.SetActive(true);
        levelSelectPanel.SetActive(false);
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
