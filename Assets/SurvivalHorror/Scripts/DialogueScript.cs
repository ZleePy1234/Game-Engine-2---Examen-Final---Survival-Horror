using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueScript : MonoBehaviour
{
    public string characterName;
    public Color characterColor;
    public List<string> dialogueLines;

    public int dialogueCount;

    public DialogueBoxScript dialogueBox;
    bool isNear = false;
    bool isTalking = false;
    bool finishedTalk = false;
    private PlayerMainScript playerMain;
    void Awake()
    {
        dialogueBox = GameObject.FindWithTag("DialogueBox").GetComponent<DialogueBoxScript>();
        dialogueCount = dialogueLines.Count -1;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isNear && Input.GetKeyDown(KeyCode.F))
        {
            EnterConversation();
        }
        PlayDialogue();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = true;
            PromptEnterConversation();
            
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = false;
            isTalking = false;
            RemovePrompt();
        }
    }
    void EnterConversation()
    {
        GiveItem();
        dialogueBox.dialogueHUD.SetActive(true);
        RemovePrompt();
        isTalking = true;
        if (dialogueCount > 1)
        {
            dialogueBox.dialogueArrow.SetActive(true);
        }
        
    }

    void GiveItem()
    {
        if (characterName == "Elster")
        {
            GameObject.Find("Player").GetComponent<PlayerEquipmentScript>().pickedUpCamera = true;
        }
        else if (characterName == "Kolibri")
        {
            GameObject.Find("Player").GetComponent<PlayerEquipmentScript>().pickedUpGun = true;
        }
        else if (characterName == "Ara")
        {
            GameObject.Find("Player").GetComponent<PlayerEquipmentScript>().pickedUpCard = true;
        }
    }
    void PlayDialogue()
    {
        if (isTalking == true)
        {
            DialogueArrow();
        }
        if (Input.GetKeyDown(KeyCode.F) && isTalking == true)
        {
            if (dialogueCount > 0)
            {
                DisplayDialogue(dialogueCount);
                dialogueCount--;
            }
            else if (dialogueCount == 0)
            {
                DisplayDialogue(0);
                dialogueCount = -1;
                finishedTalk = true;
            }
            else
            {
                EndConversation();
            }
            
        }
    }
    void PromptEnterConversation()
    {
        dialogueBox.startPrompt.GameObject().SetActive(true);
        dialogueBox.startPrompt.text = "Press F to talk to " + characterName + " Replika Unit";
        
    }

    void EndConversation()
    {
        if (finishedTalk == true)
        {
            dialogueBox.dialogueHUD.SetActive(false);
            isTalking = false;
            dialogueCount = 0;
        }
        else
        {
            dialogueBox.dialogueHUD.SetActive(false);
            dialogueCount = dialogueLines.Count;
        }
    }
    void RemovePrompt()
    {
        dialogueBox.startPrompt.GameObject().SetActive(false);
    }
    void DisplayDialogue(int dialogueNumber)
    {
        if (dialogueNumber < 0 || dialogueNumber >= dialogueLines.Count)
        {
            Debug.LogWarning("Invalid dialogue number.");
            return;
        }

        Debug.Log($"{characterName}: {dialogueLines[dialogueNumber]}");
        dialogueBox.characterName.text = characterName;
        dialogueBox.characterName.color = characterColor;
        dialogueBox.dialogue.text = dialogueLines[dialogueNumber];
    }

    private void DialogueArrow()
    {
        if (dialogueCount > 0)
        {
            dialogueBox.dialogueArrow.SetActive(true);
        }
        else
        {
            dialogueBox.dialogueArrow.SetActive(false);
        }
    }
}
