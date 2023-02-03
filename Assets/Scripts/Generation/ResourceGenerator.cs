using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
	[SerializeField] private float _seed;

	[Header("Chances")]
	[SerializeField] private float _nothingChance;
	[SerializeField] private float _resourcesChance;
	[SerializeField] private float _hazardChance;
	[SerializeField] private float _pickupOneChance;
	[SerializeField] private float _pickupTwoChance;
	[SerializeField] private float _portalChance;


	private void Awake()
	{
		int randomSeed = Random.Range(0, 1000);
		_seed = randomSeed;
		Random.InitState(randomSeed);
	}
	private void Start()
	{

	}


	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.B))
		{
			Debug.Log("Resource Generation: Testing on X: 0 and Y: 0, Results: " + GetPositionValueWithSeed(0, 0));
			Debug.Log("Resource Generation: Testing on X: 3 and Y: 1, Results: " + GetPositionValueWithSeed(3, 1));
		}
	}


	public float GetPositionValueWithSeed(float x,float y)
	{
		return (_seed / (x + y));
	}

	public ResourceType GetObjectTypeToSpawn(float value)
	{
		if(value <= _nothingChance)
		{
			return ResourceType.Nothing;
		}
		else if (value <= _resourcesChance)
		{
			return ResourceType.Resources;
		}
		else if (value <= _hazardChance)
		{
			return ResourceType.Hazard;
		}
		else if (value <= _pickupOneChance)
		{
			return ResourceType.Pickup1;
		}
		else if (value <= _pickupTwoChance)
		{
			return ResourceType.Pickup2;
		}
		else if (value <= _portalChance)
		{
			return ResourceType.Portal;
		}
		return ResourceType.Nothing;
	}

}

public enum ResourceType
{
	Nothing,
	Resources,
	Hazard,
	Pickup1,
	Pickup2,
	Portal
}