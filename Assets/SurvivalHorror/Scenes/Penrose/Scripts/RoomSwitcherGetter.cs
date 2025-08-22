using UnityEngine;

public class RoomSwitcherGetter : MonoBehaviour
{
    private RoomSwitcherScript roomSwitcher;
    private PlayerMainScript playerMain;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerMain = GameObject.Find("Player").GetComponent<PlayerMainScript>();
    }


    void TeleportPlayer()
    {
        roomSwitcher = playerMain.roomSwitcher;
        roomSwitcher.SwitchRoom();
    }
}
