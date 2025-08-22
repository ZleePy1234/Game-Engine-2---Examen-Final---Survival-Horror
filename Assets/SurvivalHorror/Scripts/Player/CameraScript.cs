using Unity.Mathematics;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float sensX;
    public float sensY;
    float xRotation;
    float yRotation;
    [Header("------------------")]
    public Transform cameraPos;
    public Transform cam;
    public Transform aim;
    public Transform orientation;
    void Awake()
    {
        CursorStart();
    }

    void Update()
    {
        CameraRotations();
        AimRot();
    }
    void CursorStart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void CameraRotations()
    {
        transform.position = cameraPos.position;
        aim.rotation = Quaternion.LookRotation(cameraPos.forward, Vector3.up);



        float mouseX = Input.GetAxis("Mouse X") * sensX;
        float mouseY = Input.GetAxis("Mouse Y") * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = math.clamp(xRotation, -90f, 90f);

        cam.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientation.localRotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    void AimRot()
    {
        aim.SetPositionAndRotation(cam.position, cam.rotation);
    }
}
