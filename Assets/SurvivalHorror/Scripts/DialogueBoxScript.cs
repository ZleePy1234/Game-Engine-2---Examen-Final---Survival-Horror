using TMPro;
using UnityEngine;

public class DialogueBoxScript : MonoBehaviour
{
    public GameObject dialogueHUD;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogue;
    public GameObject dialogueArrow;
    public TextMeshProUGUI startPrompt;

    void Awake()
    {
        dialogueHUD.SetActive(false);
    }
}
    