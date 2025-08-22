using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    PlayerEquipmentScript player;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player.pickedUpCard == true)
            {
                Menu_WinAsync();
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerEquipmentScript>();
    }

    private async void Menu_WinAsync()
    {
        Debug.Log("Async Game Load Started");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Win");
        while (!asyncLoad.isDone)
        {
            await System.Threading.Tasks.Task.Yield();
        }
    }
}
