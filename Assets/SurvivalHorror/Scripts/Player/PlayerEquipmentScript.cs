using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class PlayerEquipmentScript : MonoBehaviour
{
    public Camera maincamera;
    public GameObject cameraEquipment;

    public GameObject mapScreen;
    public GameObject hudScreen;
    public GameObject cameraScreen;

    public GameObject gunEquipment;
    public enum EquippedItem
    {
        None,
        Camera,
        Gun,
        End
    }
    public EquippedItem currentEquippedItem = EquippedItem.None;
    public bool pickedUpCamera = false;
    public bool pickedUpGun = false;
    public bool pickedUpCard = false;

    public enum CameraScreen
    {
        Map,
        HUD,
        Record,
        End
    }
    public  CameraScreen cameraSelectedScreen = CameraScreen.Record;


    [Space(10)]
    [Header("-----------------------------------------")]
    [Space(10)]

    public int gunDamage;
    public int magazineSize;
    public int currentAmmo;
    public int reserveAmmo;
    public float reloadTime;
    public int cameraTape;
    public int healingItems;
    public int maxHealingItems;
    private GameObject rayStartPoint;
    private bool isAiming = false;
    private bool isReloading = false;
    private bool canFire = true;

    private PlayerMainScript playerMain;

    public GameObject crosshair;

    // public GameObject[] obj = new GameObject[3];

    void Awake()
    {
        rayStartPoint = GameObject.FindWithTag("RayPos");
        maincamera = Camera.main;
        playerMain = GetComponent<PlayerMainScript>();
        EquipmentSwitch();
    }
    /*void Objectives()
    {
        if (pickedUpGun == false)
        {
            obj[0].SetActive(true);
        }
        if (pickedUpCard == false && pickedUpGun == true)
        {
            obj[0].SetActive(false);
            obj[1].SetActive(true);
        }
        if (pickedUpCard == true)
        {
            obj[0].SetActive(false);
            obj[1].SetActive(false);
            obj[2].SetActive(true);
        }
    }
    */
    void Update()
    {
        //Objectives();
        Equipment();
        GunFunc();
        CamFunc();
    }
    void AimGun()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && currentEquippedItem == EquippedItem.Gun)
        {
            crosshair.SetActive(true);
            isAiming = true;
            StartCoroutine(SmoothFOV(maincamera.fieldOfView, 40f, 0.2f)); // Smoothly transition to aiming FOV
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1) && currentEquippedItem == EquippedItem.Gun)
        {
            crosshair.SetActive(false);
            isAiming = false;
            StopCoroutine(nameof(SmoothFOV));
            StartCoroutine(SmoothFOV(maincamera.fieldOfView, 60f, 0.2f)); // Smoothly transition back to normal FOV
        }
    }
    IEnumerator SmoothFOV(float startFOV, float endFOV, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            // Ease in-out (smoothstep)
            t = t * t * (3f - 2f * t);
            maincamera.fieldOfView = Mathf.Lerp(startFOV, endFOV, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        maincamera.fieldOfView = endFOV; // Ensure we set the final value
    }

    void GunFunc()
    {
        if (currentEquippedItem != EquippedItem.Gun)
        {
            return;
        }
        else
        {
            ShootGun();
            AimGun();
            ReloadGun();
        }
    }
    void ReloadGun()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo != 12)
        {
            Debug.Log("Reloading!");
            StartCoroutine(Reload());
        }
    }
    IEnumerator Reload()
    {
        isReloading = true;
        canFire = false;
        currentAmmo = 0;
        yield return new WaitForSeconds(reloadTime);
        if (reserveAmmo >= 12)
        {
            reserveAmmo -= 12;
            currentAmmo = 12;
        }
        else
        {
            currentAmmo = reserveAmmo;
            reserveAmmo = 0;
        }
        isReloading = false;
        canFire = true;
    }

    public LayerMask gunMask;
    void ShootGun()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && currentEquippedItem == EquippedItem.Gun && canFire && isAiming)
        {
            if (currentAmmo > 0)
            {
                Debug.Log("Shooting gun");
                RaycastHit hit;
                Debug.DrawRay(rayStartPoint.transform.position, rayStartPoint.transform.forward * 100f, Color.red, 4f);
                if (Physics.Raycast(rayStartPoint.transform.position, rayStartPoint.transform.forward, out hit, 100f, ~gunMask))
                {
                    Debug.Log("Hit: " + hit.collider.name);
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        EnemyBehaviours enemyBehaviours = hit.collider.GetComponent<EnemyBehaviours>();
                        enemyBehaviours.Damage(gunDamage);
                    }
                    else if (hit.collider.CompareTag("Interactable"))
                    {
                        Debug.Log("Hit an interactable object!");
                        // Add interaction logic here if needed
                    }
                }
                else
                {
                    Debug.Log("Missed shot");
                }
                currentAmmo--;
            }
            else
            {
                Debug.Log("Out of ammo!");
            }
        }
    }
    public void Equipment()
    {
        if (!Input.GetKeyDown(KeyCode.E))
        {
            return;
        }
        else if (Input.GetKeyDown(KeyCode.E) && isReloading == false)
        {
            currentEquippedItem++;
            if (currentEquippedItem >= EquippedItem.End)
            {
                currentEquippedItem = 0;
            }
        }
        EquipmentSwitch();
    }

    void CamFunc()
    {
        if (currentEquippedItem != EquippedItem.Camera)
        {
            return;
        }
        else
        {
            CameraSwap();
            CameraFire();
        }
    }

    void CameraSwap()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            cameraSelectedScreen++;
            if (cameraSelectedScreen >= CameraScreen.End)
            {
                cameraSelectedScreen = 0;
            }
        }
        CamSwitch();
    }
    void CamSwitch()
    {
        switch (cameraSelectedScreen)
        {
            case CameraScreen.Map:
                mapScreen.SetActive(true);
                hudScreen.SetActive(false);
                cameraScreen.SetActive(false);
                cameraSelectedScreen = CameraScreen.HUD;
                break;
            case CameraScreen.HUD:
                mapScreen.SetActive(false);
                hudScreen.SetActive(true);
                cameraScreen.SetActive(false);
                break;
            case CameraScreen.Record:
                mapScreen.SetActive(false);
                hudScreen.SetActive(false);
                cameraScreen.SetActive(true);
                break;
            default:
                Debug.Log("No Screen Found! switching back to Map!");
                cameraSelectedScreen = CameraScreen.Map;
                CameraSwap();
                break;
        }
    }
    private bool canFireCamera = true;
    public float cameraCooldown;
    private float photoTakenLenght = 0.25f;
    public bool photoTaken;
    void CameraFire()
    {
        if (cameraSelectedScreen != CameraScreen.Record)
        {
            return;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && canFireCamera == true)
            {
                    if (cameraTape < 0)
                {
                    Debug.Log("No camera tape Left!");
                }
                else
                {
                    Debug.Log("Firing Camera!");
                    StartCoroutine(FireCamera());
                }
            }
        }
    }
    IEnumerator FireCamera()
    {
        canFireCamera = false;
        StartCoroutine(CameraActivation());
        yield return new WaitForSeconds(cameraCooldown);
        canFireCamera = true;


    }
    IEnumerator CameraActivation()
    {
        photoTaken = true;
        yield return new WaitForSeconds(photoTakenLenght);
        photoTaken = false;
    }
    void EquipmentSwitch()
    {
        switch (currentEquippedItem)
        {
            default:
                currentEquippedItem = EquippedItem.None;
                cameraEquipment.SetActive(false);
                gunEquipment.SetActive(false);
                break;
            case EquippedItem.None:
                if(pickedUpCamera == false)
                {
                    Debug.Log("Camera has not been picked up, keeping empty");
                    currentEquippedItem = EquippedItem.None;
                    cameraEquipment.SetActive(false);
                    gunEquipment.SetActive(false);
                    break;
                }
                Debug.Log("Camera has been picked up, skipping to camera");
                currentEquippedItem++;
                EquipmentSwitch();
                break;
            case EquippedItem.Camera:
                if (pickedUpCamera == false)
                {
                    Debug.Log("Camera has not been picked up, switching to gun");
                    currentEquippedItem++;
                    EquipmentSwitch();
                    return;
                }
                Debug.Log("Switched to camera");
                currentEquippedItem  = EquippedItem.Camera;
                cameraEquipment.SetActive(true);
                gunEquipment.SetActive(false);
                break;
            case EquippedItem.Gun:
            if (pickedUpGun == false)
                {
                    Debug.Log("Gun has not been picked up, switching to Next");
                    currentEquippedItem++;
                    EquipmentSwitch();
                    return;
                }
                Debug.Log("Switched to gun");
                currentEquippedItem = EquippedItem.Gun;
                cameraEquipment.SetActive(false);
                gunEquipment.SetActive(true);
                break;
            case EquippedItem.End:
                Debug.Log("Cycle complete, switching back to none");
                currentEquippedItem = 0;
                EquipmentSwitch();
                break;
        }
    }
    
}
