using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] clip = new AudioClip[2];
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Button_Restart()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void PlayBGM()
    {
        audioSource.PlayOneShot(clip[1]);
        audioSource.loop = true;
    }
    public void PlayPianoStinger()
    {
        
        audioSource.PlayOneShot(clip[0]);
    }
}
