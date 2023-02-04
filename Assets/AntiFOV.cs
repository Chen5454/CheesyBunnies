using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiFOV : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteRenderer myrenderer;
    private void Start()
    {
        myrenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collider enter");
        if (collision.gameObject.layer != gameObject.layer)
            myrenderer.enabled = false;

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("Collider exit");
        if (collision.gameObject.layer != gameObject.layer)
            myrenderer.enabled = true;
    }
}
