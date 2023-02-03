using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootSpawn : MonoBehaviour
{
	[Range(0,360)]
	[SerializeField] private float _spawnDirection;
	public float SpawnDirection => _spawnDirection;

	[SerializeField] private GameObject _topPF;

	private GameObject _top;

	public void OnSpawnNewObject(float angle)
	{
		if (_top == null)
		{

			_top = Instantiate(_topPF, this.transform);
			_top.transform.rotation = Quaternion.Euler(0, 0, angle - 180f);
		}
	}

	

}
