using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormWarning : MonoBehaviour
{
    private WormMove WormScript;
    private void Start()
    {
        WormScript = transform.parent.GetComponent<WormMove>();
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name != "Root" && collision.tag != "FOV")
        {
            Debug.Log(collision.name);
            WormScript.DetectItem();
        }
    }

}
