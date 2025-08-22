using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject creditsUI;

    public AudioSource menuSfxSource;

    public AudioClip[] menuSounds = new AudioClip[2];


    void Awake()
    {
        mainMenuUI.SetActive(true);
        creditsUI.SetActive(false);       
    }
    public void Menu_ShowMainMenu()
    {
        menuSfxSource.PlayOneShot(menuSounds[1]);
        mainMenuUI.SetActive(true);
        creditsUI.SetActive(false);
    }
    public void Menu_ShowCredits()
    {
        menuSfxSource.PlayOneShot(menuSounds[1]);
        mainMenuUI.SetActive(false);
        creditsUI.SetActive(true);
    }

    public void Menu_StartGame()
    {
        menuSfxSource.PlayOneShot(menuSounds[0]);
        Menu_StartGameAsync();
    }

    public void Menu_QuitGame()
    {
        menuSfxSource.PlayOneShot(menuSounds[0]);
        Debug.Log("Game Quit");
        Application.Quit();
    }

    private async void Menu_StartGameAsync()
    {
        Debug.Log("Async Game Load Started");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game");
        while (!asyncLoad.isDone)
        {
            await System.Threading.Tasks.Task.Yield();
        }
    }
}
