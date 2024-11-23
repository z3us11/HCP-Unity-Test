using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Constants.playerTag))
        {
            gameManager.ShowWinScreen();
        }
    }
}
