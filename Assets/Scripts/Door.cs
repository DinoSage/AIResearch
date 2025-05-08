using UnityEngine;

public class Door : MonoBehaviour
{
    // ==============================
    //       Serialized Fields
    // ==============================
    [SerializeField] private Vector2 direction;


    // ==============================
    //        Other Variables
    // ==============================
    private static float MARGIN = 2.5f;


    // ==============================
    //        Unity Functions
    // ==============================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 offset = new Vector3(direction.x, direction.y, 0);
            Vector3 start = this.transform.position + offset.normalized * MARGIN;
            RaycastHit2D hit = Physics2D.Raycast(start, direction.normalized, 100f, LayerMask.GetMask("Door"));
            if (hit)
            {
                collision.gameObject.transform.position = hit.point + MARGIN * direction.normalized;
            }
        }
    }


    // ==============================
    //       Private Functions
    // ==============================


    // ==============================
    //        Public Functions
    // ==============================
    public Setting GetDestination()
    {
        Vector3 offset = new Vector3(direction.x, direction.y, 0);
        Vector3 start = this.transform.position + offset.normalized * MARGIN;
        RaycastHit2D hit = Physics2D.Raycast(start, direction.normalized, 100f, LayerMask.GetMask("Setting"));

        return (hit) ? hit.collider.gameObject.GetComponent<Setting>() : null;
    }

}
