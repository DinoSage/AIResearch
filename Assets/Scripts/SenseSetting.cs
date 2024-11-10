using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenseSetting : MonoBehaviour
{
    // -- Private Fields --
    WorldObject wobj;
    AICharacter character;

    void Start()
    {
        wobj = GetComponent<WorldObject>();
        character = wobj.GetComponent<AICharacter>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Setting setting = collision.GetComponent<Setting>();
        if (setting != null)
        {
            Debug.Log("HI!");
            wobj.UpdateProxemAll(string.Format("{0} are now in the {1}", character.name, setting.settingName));
        }
    }
}
