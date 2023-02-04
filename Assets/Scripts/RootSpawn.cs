using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootSpawn : MonoBehaviour
{
	[Range(0,360)]
	[SerializeField] private float _spawnDirection;
	public float SpawnDirection => (transform.rotation.eulerAngles.z + 180);



	[SerializeField] private GameObject _topPF;

	[SerializeField]private GameObject _top;

	private void Awake()
	{
		if(_top != null)
		{
			_top.SetActive(false);
		}

	}



	public void OnSpawnNewObject(float angle)
	{
		if (_top == null)
		{
			_top = Instantiate(_topPF, this.transform);
			_top.transform.rotation = Quaternion.Euler(0, 0, angle - 180f);
		}
		else
		{
			_top.SetActive(true);
			_top.transform.rotation = Quaternion.Euler(0, 0, angle - 180f);
		}
		Debug.Log("Z rotation" + transform.rotation.eulerAngles.z);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		float angle = _spawnDirection * Mathf.Deg2Rad;

		Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
		Vector3 target = this.transform.position + direction * 2f;
		Gizmos.DrawLine(this.transform.position, target);


		Gizmos.color = Color.blue;

		float secondAngle = (transform.rotation.eulerAngles.z + 180) * Mathf.Deg2Rad;

		Vector3 secondDirection = new Vector3(Mathf.Cos(secondAngle), Mathf.Sin(secondAngle), 0);
		Vector3 secondTarget = this.transform.position + secondDirection * 2f;
		Gizmos.DrawLine(this.transform.position, secondTarget);
	}

}
