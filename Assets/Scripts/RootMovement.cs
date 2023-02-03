using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RootMovement : MonoBehaviour
{

    private LineRenderer lineRenderer;
    private int numberOfPoints = 0;
    private Vector3 direction = Vector3.down;
    private float angle = 11;
    public float speed;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

    }

    private void Update()
    {
        float rotationSpeed = Input.GetAxis("Horizontal") * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = Quaternion.Euler(0, 0, -15) * direction;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Quaternion.Euler(0, 0, 15) * direction;
        }
        angle -= rotationSpeed;
        direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);

        lineRenderer.SetVertexCount(++numberOfPoints);
        lineRenderer.SetPosition(numberOfPoints - 1, transform.position);

        transform.position += direction * Time.deltaTime* speed;
    }
}

















