using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Setting : MonoBehaviour
{
    // -- Serialize Fields --
    [SerializeField]
    private string settingName;

    [SerializeField]
    private float width;

    [SerializeField]
    private float height;

    // -- Public Functions --
    public Vector2 RandPointInSetting()
    {
        float posX = Random.Range(-1 * width / 2, width / 2);
        float posY = Random.Range(-1 * height / 2, height / 2);
        return new Vector2(posX, posY);
    }

    public string GetSettingName()
    {
        return settingName;
    }


}
