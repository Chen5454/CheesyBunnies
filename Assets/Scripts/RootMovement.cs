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
    //public CameraFollow CameraFollow;
    [SerializeField] public float totalLength = 0f;//serialized for debug
    [SerializeField] Text totalTxt;
    [SerializeField] public Image RootFillImage;
	[SerializeField] private Transform tipTrans;
	[SerializeField] private LineRenderer tunnelLine;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        RootFillImage = GameState.Instance.RootFillImage;
        //totalLength = maxLength;
        AudioManager.Instance.PlayAudio(AudioManager.Instance.digLoop,true,true);
    }

    private void Update()
    {

        float rotationSpeed = Input.GetAxis("Horizontal") * Time.deltaTime;

        if (GameState.Instance.isInvert)
        {
            angle -= rotationSpeed;

        }
        else
        {
            angle += rotationSpeed;

        }

        direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);

        lineRenderer.SetVertexCount(++numberOfPoints);
        lineRenderer.SetPosition(numberOfPoints - 1, transform.position);

		tunnelLine.SetVertexCount(numberOfPoints);
		tunnelLine.SetPosition(numberOfPoints - 1, transform.position);

		Vector3 lastPosition = transform.position;
        transform.position += direction * Time.deltaTime * speed;
        totalLength -= Vector3.Distance(lastPosition, transform.position);

		tipTrans.rotation = Quaternion.Euler(0,0,(angle * Mathf.Rad2Deg) - 180);



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

          //  CameraFollow.StartFollowingLine();
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
            AudioManager.Instance.StopAudio(AudioManager.Instance.digLoop);

            if (GameState.Instance)
            {

                GameState.Instance.ChangeGameState(GameStates.CarrotView);

            }
        }
        if(totalTxt != null)
        totalTxt.text = Convert.ToInt32(totalLength).ToString();

        RootFillImage.fillAmount= totalLength/maxLength;
    }



	public void AddTotalLength(float length) => totalLength += (length + maxLength);
	public void AddLength(float length) => totalLength += length;
	public void SetInitialDirection(Vector2 dir) => direction = dir;
	public void SetInitialAngle(float newAngle) =>  angle = newAngle * Mathf.Deg2Rad;
}
