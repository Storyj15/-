using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUIController : MonoBehaviour
{
    public static int textAssetIndex;
    public static bool DialogueEnd;
    public static bool closeDialogueUI;
    public static bool openDialogueUI;
    public GameObject DialogueUI;
    public TextAsset[] textAsset;
    public Sprite[] DialogueImages;
    public Sprite[] DialogueBacks;
    // Start is called before the first frame update
    void Start()
    {
        openDialogueUI = false;
        closeDialogueUI = false;
        DialogueEnd = false;
        DialogueUI.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (openDialogueUI)
        {
            openDialogueUI = false;
            DialogueUI.GetComponent<dialog>().dialogDataFile = textAsset[textAssetIndex];
            DialogueUI.GetComponent<dialog>().dialogueBackList = DialogueBacks;
            DialogueUI.GetComponent<dialog>().dialogueImageList = DialogueImages;
            DialogueUI.SetActive(true);
        }
        if (closeDialogueUI)
        {
            closeDialogueUI = false;
            DialogueUI.SetActive(false);
        }
    }
    public void SkipDialogue()
    {
        closeDialogueUI = true;
        DialogueEnd = true;
    }
}
