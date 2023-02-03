using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
public class CameraController : MonoBehaviour
{
	private static CameraController _instance;
	public static CameraController Instance => _instance;

	//0 = root, 1 = carrot
	[SerializeField] private List<CinemachineVirtualCamera> _virtualCameraList = new List<CinemachineVirtualCamera>();
	int cameraCurrentIndex;


	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		else if (_instance != this)
		{
			Destroy(this.gameObject);
		}
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.E))
		{
			ChangeCamera(0);
		}
		if (Input.GetKeyDown(KeyCode.W))
		{
			ChangeCamera(1);
		}
	}



    /// <summary>
    /// 0 = carrot camera, 1 = root camera
    /// </summary>
    /// <param name="index"></param>
    public void ChangeCamera(int index)
	{
		cameraCurrentIndex = index;
		for (int i = 0; i < _virtualCameraList.Count; i++)
		{
			_virtualCameraList[i].Priority = i == index ? 1 : 0;
		}
	}
	public void SetNewRootCameraFollow(Transform newRootTrans)
	{
		_virtualCameraList[1].Follow = newRootTrans;
	}
}
