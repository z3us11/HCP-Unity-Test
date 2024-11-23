using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    PlayerCharacter character;
    void Start()
    {
        int cutSceneStatus = PlayerPrefs.GetInt("CutscenePlayed", 0);
        if(cutSceneStatus == 1)
        {
            gameObject.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("CutscenePlayed", 1);
        }
    }

    // Update is called once per frame
    void StartCutscene()
    {
        character.canMove = false;
    }
    void EndCutscene()
    {
        character.canMove = true;
        gameObject.SetActive(false);
    }
}
