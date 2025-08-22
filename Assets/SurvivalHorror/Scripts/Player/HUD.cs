using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    private PlayerEquipmentScript player;
    private PlayerMainScript health;
    public TextMeshProUGUI weaponReserveAmmo;
    public TextMeshProUGUI cameraShots;
    public GameObject characterPortrait;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        animator = characterPortrait.GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<PlayerEquipmentScript>();
        health = GameObject.Find("Player").GetComponent<PlayerMainScript>();
    }
    // Update is called once per frame
    void Update()
    {
        weaponReserveAmmo.text = player.reserveAmmo.ToString();
        cameraShots.text = player.cameraTape.ToString();
        animator.SetInteger("Health", health.currentHealth);
    }
}
