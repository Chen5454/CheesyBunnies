using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootSpawn : MonoBehaviour
{
	[Range(0,360)]
	[SerializeField] private float _spawnDirection;
	public float SpawnDirection => _spawnDirection;


	

}
