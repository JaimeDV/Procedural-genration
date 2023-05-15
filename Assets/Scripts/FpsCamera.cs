using UnityEngine;

public class FpsCamera : MonoBehaviour
{
    [SerializeField]
    private float sensitivity = 10F;

    private Vector2 mouselook;
    private Vector2 mouseDelta;
    private Transform CameraTr;
    private bool isOnTalking = false;
    private bool pause = false;

    private void Awake()
    {
        CameraTr = Camera.main.transform;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        mouseDelta = new Vector2(
            Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime,
            Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime
            );
        mouselook += mouseDelta;
        mouselook.y = Mathf.Clamp(mouselook.y, -90, 90);
        transform.localRotation = Quaternion.AngleAxis(mouselook.x, Vector3.up);
        CameraTr.localRotation = Quaternion.AngleAxis(-mouselook.y, Vector3.right);
    }

}