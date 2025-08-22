using TMPro;
using UnityEngine;

public class RoomSwitcherScript : MonoBehaviour
{
    private Animator fadeAnims;
    public Transform teleportPoint;
    private bool inRange;
    public string roomName;
    private TextMeshProUGUI roomNameText;
    private GameObject roomTextObject;
    private PlayerMainScript playerMainScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        fadeAnims = GameObject.FindWithTag("RoomFade").GetComponent<Animator>();
        roomNameText = GetComponentInChildren<TextMeshProUGUI>();
        roomTextObject = roomNameText.gameObject;
    }

    void Start()
    {
        roomNameText.text = "Press E to go to " + roomName;
        roomTextObject.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            roomTextObject.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            roomTextObject.SetActive(false);
        }
    }

    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.F))
        {
            playerMainScript = GameObject.Find("Player").GetComponent<PlayerMainScript>();
            playerMainScript.roomSwitcher = this;
            fadeAnims.SetTrigger("ExitRoom");
        }
    }

    public void SwitchRoom()
    {
        GameObject player = GameObject.Find("Player");
        player.transform.position = teleportPoint.position;
        fadeAnims.SetTrigger("EnterRoom");
    }
}
