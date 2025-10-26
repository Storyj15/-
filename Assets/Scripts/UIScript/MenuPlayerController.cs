using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPlayerController : MonoBehaviour
{
    public Button ContinueGameButton;
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.gameManager_instance.LevelNumber != 0)
        {
            ContinueGameButton.interactable = true;
        }
        else
        {
            ContinueGameButton.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
