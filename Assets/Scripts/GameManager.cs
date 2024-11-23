using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    PlayerCharacter player;
    [SerializeField]
    GameObject loseScreen;
    [SerializeField]
    GameObject winScreen;
    int nextLevel;
    // Start is called before the first frame update
    void Start()
    {
        if (player != null)
        {
            player.SetGameManager(this);
        }
        loseScreen.SetActive(false);
        winScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ShowLoseScreen()
    {
        loseScreen.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ShowWinScreen()
    {
        nextLevel = SceneManager.GetActiveScene().buildIndex;
        nextLevel++;
        Time.timeScale = 0f;
        if (nextLevel <=Constants.endLevel)
        {
            winScreen.SetActive(true);
        }
        else
        {
            LoadLevel(Constants.thanksLevel);
        }

    }
    public void RestartLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        LoadLevel(nextLevel);
    }
    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }
}


