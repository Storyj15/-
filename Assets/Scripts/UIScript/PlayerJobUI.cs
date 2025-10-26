using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerJobUI : MonoBehaviour
{
    public Image JobImage;
    public Sprite[] Jobs;
    public TextMeshProUGUI JobName;
    public TextMeshProUGUI SkillName;
    public TextMeshProUGUI DefenseCardEffect;
    public TextMeshProUGUI ATKCardEffect;
    public TextMeshProUGUI SkillCardEffect;
    // Start is called before the first frame update
    private void Start()
    {
        var playerjob = GameManager.gameManager_instance.PlayerJob;
        var playerdata = PlayerUIManager.GetInstance().PlayerData;
        JobImage.sprite = Jobs[playerjob];
        JobName.text = "·§©À " + GameManager.gameManager_instance.Jobs[playerjob].JobName;
        SkillName.text = "¯à¤O " + playerdata.SkillName;
        DefenseCardEffect.text = playerdata.DefenseText;
        ATKCardEffect.text = playerdata.ATKText;
        SkillCardEffect.text = playerdata.SkillText;
    }
}
