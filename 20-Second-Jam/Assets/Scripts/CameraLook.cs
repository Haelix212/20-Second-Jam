using UnityEngine;
using UnityEngine.InputSystem; 

public class CameraLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 0.1f;
    private Vector2 lookInput;
    private float cameraVectorX;
    private float cameraVectorY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    private void Update()
    {
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        cameraVectorY += mouseX;
        cameraVectorX -= mouseY;

        cameraVectorX = Mathf.Clamp(cameraVectorX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(cameraVectorX, cameraVectorY, 0f);
    }
}
