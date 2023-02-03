using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Root;
    public LineRenderer lineRenderer;
    public float smoothing = 5.0f;

    private Vector3 offset;
    public bool followingLine = false;
    bool reverse;
    private int lineSegmentCount;

    void Start()
    {
        offset = transform.position - Root.position;
        lineSegmentCount = lineRenderer.positionCount - 1;
    }

    void LateUpdate()
    {
        if (followingLine)
        {
            if (reverse)
            {
                Vector3 targetCamPos = new Vector3(lineRenderer.GetPosition(lineSegmentCount).x + offset.x, lineRenderer.GetPosition(lineSegmentCount).y + offset.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);

                if (lineSegmentCount == 0)
                {
                    followingLine = false;
                    reverse = false;
                    transform.position = new Vector3(Root.position.x + offset.x, Root.position.y + offset.y, transform.position.z);
                }

                lineSegmentCount--;
            }
            else
            {
                Vector3 targetCamPos = new Vector3(lineRenderer.GetPosition(lineSegmentCount).x + offset.x, lineRenderer.GetPosition(lineSegmentCount).y + offset.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);

                if (lineSegmentCount == lineRenderer.positionCount - 1)
                {
                    followingLine = false;
                }

                lineSegmentCount++;
            }
        }
        else
        {
            Vector3 targetCamPos = new Vector3(Root.position.x + offset.x, Root.position.y + offset.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
            lineSegmentCount = lineRenderer.positionCount - 1; // add this line
        }
    }
    public void StopFollowingLine()
    {
        lineSegmentCount = lineRenderer.positionCount - 1;
        followingLine = false;
    }

    public void StartFollowingLine()
    {
        followingLine = true;
        lineSegmentCount = lineRenderer.positionCount - 1;

    }

    public void FollowLineReverse()
    {
        followingLine = true;
        lineSegmentCount = lineRenderer.positionCount - 1;
        reverse = true;
        Root = null;
    }

}
