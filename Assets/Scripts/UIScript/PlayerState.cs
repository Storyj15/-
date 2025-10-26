using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerState : MonoBehaviour
{
    public AudioClip[] UISound;
    public Sprite[] JobIcon;
    public Image JobImage;
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI DefenseText;
    // Start is called before the first frame update
    void Start()
    {
        JobImage.sprite = JobIcon[GameManager.gameManager_instance.PlayerJob];
        Debug.Log(GameManager.gameManager_instance.PlayerJob);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerUIManager.GetInstance().PlayerData.HP <= 0)
        {
            HPText.text = 0.ToString();
        }
        else
        {
            HPText.text = PlayerUIManager.GetInstance().PlayerData.HP.ToString();
        }
        DefenseText.text = PlayerUIManager.GetInstance().PlayerData.Defense.ToString();
    }
    public void ButtonSound(int type)
    {
        GameManager.gameManager_instance.audioManager.PlayClip(1, UISound[type], false);
    }
}
