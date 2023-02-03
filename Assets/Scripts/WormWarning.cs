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
        Debug.Log(collision.name);
        if(collision.name!="Root")
            WormScript.DetectItem();
    }

}
