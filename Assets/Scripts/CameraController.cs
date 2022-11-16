using UnityEngine;

public class CameraController : MonoBehaviour, IDataPersistence
{
    [SerializeField] private Transform player;
    [SerializeField] private float sensitivity = 100f;
    [SerializeField] private float clampAngle = 85f;

    private float verticalRotation;
    private float horizontalRotation;
    private void OnValidate()
    {
        if (player == null)
            player = GetComponentInParent<Transform>();
    }

    private void Start()
    {
        verticalRotation = transform.localEulerAngles.x;
        horizontalRotation = player.transform.eulerAngles.y;
    }

    public void LoadData(GameDataCollection data) {
        player.transform.rotation = data.playerRotation;
        transform.localRotation = data.cameraRotation;
    }

    public void SaveData(ref GameDataCollection data) {
        data.playerRotation = player.transform.rotation;
        data.cameraRotation = transform.localRotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleCursorMode();

        if (Cursor.lockState == CursorLockMode.Locked)
            Look();
    }

    private void Look()
    {
        float mouseVertical = -Input.GetAxis("Mouse Y");
        float mouseHorizontal = Input.GetAxis("Mouse X");

        verticalRotation += mouseVertical * sensitivity * Time.deltaTime;
        horizontalRotation += mouseHorizontal * sensitivity * Time.deltaTime;

        verticalRotation = Mathf.Clamp(verticalRotation, -clampAngle, clampAngle);

        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        player.transform.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);
    }

    private void ToggleCursorMode()
    {
        Cursor.visible = !Cursor.visible;

        if (Cursor.lockState == CursorLockMode.None)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }
}
