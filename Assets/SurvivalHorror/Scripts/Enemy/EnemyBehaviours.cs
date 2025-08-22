using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyBehaviours : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private AItarget ai;
    [SerializeField] private float lostSightTolerance = 0.5f; // seconds
    private float lostSightTimer = 0f;

    public float searchTimer;

    private bool isSearching = false;
    public GameObject player;
    [SerializeField] private LayerMask ignore;

    [SerializeField] private GameObject[] loot = new GameObject[2];
    private enum EnemyHealthState
    {
        healthy,
        enraged,
        dead
    }
    [SerializeField] private EnemyHealthState enemyHealthState;


    private enum EnemyAlertState
    {
        idle,
        chasing,
        searching,
        returning

    }
    [SerializeField] private EnemyAlertState enemyAlertState = EnemyAlertState.idle;


    private AudioSource audioSource;

    [SerializeField] private AudioClip[] sfx = new AudioClip[4];

    public bool seenByCamera = false;
    private PlayerEquipmentScript playerEquipment;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        ai = GetComponent<AItarget>();
        player = GameObject.Find("PlayerCapsule");
        audioSource = GetComponent<AudioSource>();
        playerEquipment = GameObject.FindWithTag("Player").GetComponent<PlayerEquipmentScript>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemyAlertState();
        CheckVisibility();
    }

    void FixedUpdate()
    {
        if (enemyHealthState != EnemyHealthState.dead)
        {
            CheckPlayerDistanceAndChase();
        }

    }
    private bool isVisible = false;
    void CheckVisibility()
    {
        if (playerEquipment.photoTaken == false || isVisible == true)
        {
            return;
        }
        else
        {
            isVisible = true;
            gameObject.layer++;
            foreach (Transform child in transform)
            {
                child.gameObject.layer++;
            }
        }
    }

    [SerializeField] private float enemyRange;

    #region Enemy AI Behaviour
    private void CheckPlayerDistanceAndChase()
    {
        if (enemyHealthState == EnemyHealthState.dead)
        {
            return;
        }
        float distance = Vector3.Distance(transform.position, player.transform.position);

        // Draw a debug ray to visualize the enemy range
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        Debug.DrawRay(transform.position, directionToPlayer * enemyRange, Color.red);

        if (distance <= enemyRange && enemyAlertState != EnemyAlertState.chasing)
        {
            // Only start chasing if there is line of sight to the player
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, enemyRange, ~ignore))
            {
                if (hit.collider.gameObject == player)
                {
                    enemyAlertState = EnemyAlertState.chasing;
                }
            }
        }

        // Only check line of sight while chasing
        if (enemyAlertState == EnemyAlertState.chasing)
        {
            // Raycast with no range limit (Mathf.Infinity)
            Debug.DrawRay(transform.position, directionToPlayer * 100f, Color.magenta); // Purple debug ray
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject == player)
                {
                    lostSightTimer = 0f;
                }
                else
                {
                    lostSightTimer += Time.fixedDeltaTime;
                    if (lostSightTimer >= lostSightTolerance)
                    {
                        enemyAlertState = EnemyAlertState.searching;
                        lostSightTimer = 0f;
                    }
                }
            }
            else
            {
                // Nothing hit, lost line of sight, start timer
                lostSightTimer += Time.fixedDeltaTime;
                if (lostSightTimer >= lostSightTolerance)
                {
                    enemyAlertState = EnemyAlertState.searching;
                    lostSightTimer = 0f;
                }
            }
        }
    }
    private void CheckEnemyAlertState()
    {
        switch (enemyAlertState)
        {
            case EnemyAlertState.idle:
                ai.Idle();
                // Do nothing else
                break;

            case EnemyAlertState.chasing:
                ai.Chase();
                break;

            case EnemyAlertState.searching:
                if (!isSearching)
                {
                    StartCoroutine(Search());
                }
                // If already searching, do nothing
                break;

            case EnemyAlertState.returning:
                ai.ReturnToSpawn();
                if (ai.isAtSpawnPos)
                {
                    enemyAlertState = EnemyAlertState.idle;
                }
                break;

            default:
                ai.Idle();
                break;
        }
    }

    private IEnumerator Search()
    {

        isSearching = true;
        ai.TargetLastSeen();
        yield return new WaitForSeconds(searchTimer);
        isSearching = false;
        enemyAlertState = EnemyAlertState.returning;

    }

    #endregion

    #region Enemy Health Behaviour

    public void Damage(int damage)
    {
        health -= damage;
        if (health > 5)
        {
            SFX(2);
            enemyHealthState = EnemyHealthState.healthy;
        }
        else if (health < 5 && health > 0)
        {
            SFX(2);
            enemyHealthState = EnemyHealthState.enraged;
        }
        else if (health <= 0)
        {
            SFX(3);
            enemyHealthState = EnemyHealthState.dead;
        }
        CheckEnemyHealthState();
    }

    void CheckEnemyHealthState()
    {
        switch (enemyHealthState)
        {
            case EnemyHealthState.healthy:
                ai.EnrageUndo();
                break;

            case EnemyHealthState.enraged:
                ai.Enrage();
                break;

            case EnemyHealthState.dead:
                KillEnemy();
                break;

            default:
                break;
        }
    }
    void KillEnemy()
    {
        int i = Random.Range(0, 2);
        //Instantiate(loot[i], new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z), Quaternion.identity);
        ai.Dead();
        StartCoroutine(DespawnEnemy());
    }

    IEnumerator DespawnEnemy()
    {
        yield return new WaitForSeconds(20);
        Destroy(gameObject);
    }

    void SFX(int i)
    {
        audioSource.PlayOneShot(sfx[i]);
    }

    #endregion


}