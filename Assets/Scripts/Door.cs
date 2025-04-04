using UnityEngine;

public class Door : MonoBehaviour
{
    private static float MARGIN = 5f;

    // -- Serialize Fields --

    [SerializeField]
    private Vector2 direction;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 offset = new Vector3(direction.x, direction.y, 0);
            Vector3 start = this.transform.position + offset.normalized * MARGIN;
            RaycastHit2D hit = Physics2D.Raycast(start, MARGIN * direction.normalized, 100f, LayerMask.GetMask("Setting"));
            if (hit)
            {
                collision.gameObject.transform.position = hit.point + direction;
            }
        }
    }
}
