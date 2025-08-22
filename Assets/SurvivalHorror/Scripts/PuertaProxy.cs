using UnityEngine;

public class PuertaProxy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator animator;

    private AudioSource audioSource;
    public AudioClip[] sfx = new AudioClip[2];
    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            audioSource.PlayOneShot(sfx[0]);
            animator.SetTrigger("Open");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            audioSource.PlayOneShot(sfx[1]);
            animator.SetTrigger("Close");
        }
    }
}
