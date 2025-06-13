using UnityEngine;

public class MainCameraMovement : MonoBehaviour
{

    // ==============================
    //    Public Static Variables
    // ==============================


    // ==============================
    //      Serialized Fields
    // ==============================


    // ==============================
    //        Public Fields
    // ==============================


    // ==============================
    //        Private Variables
    // ==============================
    private Camera mainCamera;
    private Locator locator;

    // ==============================
    //        Unity Functions
    // ==============================
    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        locator = Player.instance.GetComponent<Locator>();
        DebugEditor.OpenDebugWindow();
    }

    private void OnDestroy()
    {
        DebugEditor.CloseDebugWindow();
    }

    private void LateUpdate()
    {
        // calculate camera bounds
        Setting currSetting = locator.GetCurrSetting();
        Bounds settingBounds = currSetting.GetComponent<BoxCollider2D>().bounds;

        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        float camMinX = settingBounds.min.x + camWidth;
        float camMaxX = settingBounds.max.x - camWidth;

        float camMinY = settingBounds.min.y + camHeight;
        float camMaxY = settingBounds.max.y - camHeight;

        // move target position
        Vector3 targetPosition = Player.instance.transform.position;
        targetPosition.z = this.transform.position.z;
        targetPosition.x = Mathf.Clamp(targetPosition.x, camMinX, camMaxX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, camMinY, camMaxY);

        this.transform.position = targetPosition;
    }


    // ==============================
    //        Private Functions
    // ==============================


    // ==============================
    //        Public Functions
    // ==============================
}
