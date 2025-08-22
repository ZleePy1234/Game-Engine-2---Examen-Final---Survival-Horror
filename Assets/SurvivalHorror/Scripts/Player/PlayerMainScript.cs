using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMainScript : MonoBehaviour
{
    #region Variables
    [Header("Variables")]
    [Space(10)]
    public float walkSpeed;
    public float runSpeed;
    public float drag;
    public enum PlayerState
    {
        Idle,
        Walking,
        Running,
        Interacting,
        Dead
    }
    public PlayerState currentState = PlayerState.Idle;
    [Space(5)]
    #endregion
    #region Player Stats
    [Header("Player Stats")]
    [Space(10)]
    public int maxHealth;
    public int currentHealth;
    public float damageCooldown;
    private bool canTakeDamage = true;
    [Space(5)]
    [Header("Player Components")]
    [Space(10)]
    public Rigidbody rb;
    Vector3 moveDirection;
    public Transform orientation;
    #endregion

    public RoomSwitcherScript roomSwitcher;

    void Awake()
    {
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        currentHealth = maxHealth;
    }
    void Update()
    {
        HandleMovement();

    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * vertical + orientation.right * horizontal;

        float speed = currentState == PlayerState.Running ? runSpeed : walkSpeed;

        rb.AddForce(moveDirection.normalized * speed, ForceMode.Force);

        rb.linearDamping = drag;

        if (horizontal != 0 || vertical != 0)
        {
            currentState = PlayerState.Walking;
        }
        else
        {
            currentState = PlayerState.Idle;
        }

        if (Input.GetKey(KeyCode.LeftShift) && (horizontal != 0 || vertical != 0))
        {
            currentState = PlayerState.Running;
        }
        SpeedControl();
    }

    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVel.magnitude > walkSpeed && currentState == PlayerState.Walking)
        {
            Vector3 limitedVel = flatVel.normalized * walkSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
        else if (flatVel.magnitude > runSpeed && currentState == PlayerState.Running)
        {
            Vector3 limitedVel = flatVel.normalized * runSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }
    public void TakeDamage()
    {
        if (!canTakeDamage) return; // Prevent taking damage if in cooldown
        StartCoroutine(DamageCooldown());
        currentHealth -= 1;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        currentState = PlayerState.Dead;
        rb.isKinematic = true; // Disable physics
        // Add any additional death logic here, such as playing an animation or sound
        Debug.Log("Player has died.");
        Menu_DieAsync();
    }
    private async void Menu_DieAsync()
    {
        Debug.Log("Async Game Load Started");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Death");
        while (!asyncLoad.isDone)
        {
            await System.Threading.Tasks.Task.Yield();
        }
    }

    IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }
}
