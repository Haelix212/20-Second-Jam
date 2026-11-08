using UnityEngine;
using UnityEngine.InputSystem; 

public class CameraLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 0.1f;
    private Vector2 lookInput;
    private float cameraVectorX;
    private float cameraVectorY;
    private int cameraInvert = 1; // 1 = no invert, -1 = inverted

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (PlayerPrefs.HasKey("CameraInvert"))
        {
            cameraInvert = PlayerPrefs.GetInt("CameraInvert");
        }

    }
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void UIModeCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
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
        cameraVectorX -= (mouseY * cameraInvert);

        cameraVectorX = Mathf.Clamp(cameraVectorX, -90f, 90f);
        cameraVectorY = Mathf.Clamp(cameraVectorY, -115f, 115f);

        transform.localRotation = Quaternion.Euler(cameraVectorX, cameraVectorY, 0f);
    }
}
