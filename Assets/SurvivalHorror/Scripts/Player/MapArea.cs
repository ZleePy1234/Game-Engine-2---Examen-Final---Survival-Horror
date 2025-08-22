using TMPro;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI area;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(player.transform.position, Vector3.down, out RaycastHit hit, 10, LayerMask.GetMask("Level"));
        area.text = hit.collider.gameObject.name.ToString();
        
    }
}
