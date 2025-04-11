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


    // ==============================
    //        Unity Functions
    // ==============================
    private void LateUpdate()
    {
        Vector3 targetPosition = Player.instance.transform.position;
        targetPosition.z = this.transform.position.z;
        this.transform.position = targetPosition;
    }


    // ==============================
    //        Private Functions
    // ==============================


    // ==============================
    //        Public Functions
    // ==============================
}
