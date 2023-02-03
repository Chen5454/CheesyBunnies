using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormMove : MonoBehaviour
{
    public float speed = 10, rotationSpeed = 5;
    public int size = 10;
    public float frequency = 0.3f;
    public float detectHeader = 2.5f;

    private LineRenderer lineRenderer;
    private Vector3[] worm_positions;
    private bool Detected = false;
    private float turnDetected = 0;
    private float timer = 0;

    private TrailRenderer trailRenderer;
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).position = new Vector3(-detectHeader,0,0);
        lineRenderer=transform.GetComponent<LineRenderer>();
        lineRenderer.positionCount=size;
        worm_positions = new Vector3[size];
        for (int i = 1; i < size; i++)
            worm_positions[i] = worm_positions[i - 1] + Vector3.right;
        lineRenderer.SetPositions(worm_positions);
    }
    float regular_movement()
    {
        return Mathf.Sin(Time.time) * frequency;
    }
    // Update is called once per frame
    void Update()
    {
        float rotation = (Detected? turnDetected :regular_movement()) * Time.deltaTime* rotationSpeed;
        transform.position -= transform.right * Time.deltaTime * speed;
        transform.Rotate(0, 0, rotation);
        drawWorm();
        if (Detected)
        {
            timer -= Time.deltaTime;
            Detected = !(timer <= 0);
        }
    }
    public void DetectItem()
    {
        Detected = true;
        timer = detectHeader;
        turnDetected = Mathf.Sin(Time.time);

    }
    void drawWorm()
    {
        worm_positions[0] = transform.position;
        for (int i = size - 1; i >= 1; i--)
        {
            worm_positions[i] = Vector3.Lerp(worm_positions[i],worm_positions[i - 1], Time.deltaTime * speed);
        }
        lineRenderer.SetPositions(worm_positions);
    }
}
