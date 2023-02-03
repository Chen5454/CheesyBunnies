using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RootMovement : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private int numberOfPoints = 0;
    private Vector3 direction = Vector3.down;
    private float angle = 11;
    public float speed;
    public bool isAtStartPoint;
    public float maxLength;
    public CameraFollow CameraFollow;
    private float totalLength = 0f;
    [SerializeField] Text totalTxt;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        totalLength = maxLength;
    }

    private void Update()
    {
        float rotationSpeed = Input.GetAxis("Horizontal") * Time.deltaTime;
        angle -= rotationSpeed;
        direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);

        lineRenderer.SetVertexCount(++numberOfPoints);
        lineRenderer.SetPosition(numberOfPoints - 1, transform.position);

        Vector3 lastPosition = transform.position;
        transform.position += direction * Time.deltaTime * speed;
        totalLength -= Vector3.Distance(lastPosition, transform.position);

        float threshold = 0.1f;
        if (Vector3.Distance(transform.position, lineRenderer.GetPosition(0)) < threshold)
        {
            isAtStartPoint = true;
        }
        else
        {
            isAtStartPoint = false;
        }
        if (transform.position == lineRenderer.GetPosition(0))
        {
            isAtStartPoint = true;

            CameraFollow.StartFollowingLine();
        }
        else
        {
            isAtStartPoint = false;
        }
        if (totalLength <= 0f)
        {
            direction = -direction;
            totalLength = maxLength;
            enabled= false;
            GameState.Instance.ChangeGameState(GameStates.CarrotView);
        }
        if(totalTxt != null)
        totalTxt.text = Convert.ToInt32(totalLength).ToString();
    }
}
