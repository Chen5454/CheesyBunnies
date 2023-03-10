using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormMove : MonoBehaviour
{
    public float speed = 10, rotationSpeed = 5;
    public int size = 10;
    public float frequency = 0.3f;
    public GameObject wormColliderPrefab;
    private GameObject[] wormColliders;
    private float detectHeader;

    private LineRenderer lineRenderer, trailRenderer;
    private Vector3[] worm_positions, trail_positions;
    private bool Detected = false;
    private float turnDetected = 0;
    private float timer = 0;

	[SerializeField] private Transform _wormButt;

    private void Start()
    {
        // Recieve header distance from header object.
        detectHeader = transform.GetChild(0).localPosition.x;
        trailRenderer = transform.GetChild(1).GetComponent<LineRenderer>();
        trailRenderer.useWorldSpace = true;
        lineRenderer = transform.GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = size;
        trailRenderer.positionCount = size * 3;

        wormColliders = new GameObject[size];

        trail_positions = new Vector3[size * 3];
        worm_positions = new Vector3[size];

        worm_positions[0] = trail_positions[0] = trail_positions[size] = trail_positions[size*2] = transform.position;
        for (int i = 1; i < size; i++)
        {
            worm_positions[i] = worm_positions[i - 1] + transform.right;
            trail_positions[i] = trail_positions[size*2 + i] = trail_positions[size + i] = worm_positions[0];
            GameObject collider = Instantiate(wormColliderPrefab, worm_positions[i], Quaternion.identity, transform);
            collider.transform.localScale = Vector3.one * 0.5f;//lineRenderer.widthCurve.
            wormColliders[i] = collider;
        }
        lineRenderer.SetPositions(worm_positions);
        trailRenderer.SetPositions(trail_positions);

        AudioManager.Instance.PlayAudio(AudioManager.Instance.WormDig,true,false);
    }

    float regular_movement()
    {
        return Mathf.Sin(Time.time) * frequency;
    }

    // Update is called once per frame
    void Update()
    {
        float rotation = (Detected? turnDetected :regular_movement()) * Time.deltaTime * rotationSpeed;
        transform.position -= transform.right * Time.deltaTime * speed;
        transform.Rotate(0, 0, rotation);
        DrawWorm();
        if (Detected)
        {
            timer -= Time.deltaTime;
            Detected = !(timer <= 0);
        }

		if (_wormButt != null)
		{
			//Vector2 dir = GetBeforeLastLineRendererPos() - GetLastLineRendererPos();
			//float angle = Vector2.Angle(dir, Vector2.right);
			//_wormButt.position = GetLastLineRendererPos();
			//_wormButt.rotation = Quaternion.Euler(0, 0, angle);
			//Debug.Log("Butt angle: "+  angle);
		}

	}




	Vector3 GetLastLineRendererPos() => lineRenderer.GetPosition(lineRenderer.positionCount - 1);
	Vector3 GetBeforeLastLineRendererPos() => lineRenderer.GetPosition(lineRenderer.positionCount - 2);


	public void DetectItem()
    {
        Detected = true;
        timer = detectHeader;
        turnDetected = Mathf.Sin(Time.time);

    }
    void DrawWorm()
    {
        worm_positions[0] = transform.position;
        for (int i = size - 1; i >= 1; i--)
        {
            worm_positions[i] = Vector3.Lerp(worm_positions[i],worm_positions[i - 1], Time.deltaTime * speed);
            wormColliders[i].transform.position = worm_positions[i];
            trail_positions[i + size*2] = worm_positions[i];
        }
        lineRenderer.SetPositions(worm_positions);

        for (int i = size*2+1; i > -1; i--) { 
            trail_positions[i] = Vector3.Lerp(trail_positions[i], trail_positions[i + 1], Time.deltaTime * speed);
            trailRenderer.SetPositions(trail_positions);
        }
    }
}
