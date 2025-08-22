using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMainScript player = other.GetComponentInParent<PlayerMainScript>();
            player.TakeDamage();
            Debug.Log("Player damaged!");
        }
    }
}
