using UnityEngine;

public class Player : MonoBehaviour
{
    public static GameObject instance;

    private void Awake()
    {
        instance = this.gameObject;
    }
}
