using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
	[SerializeField] private string _seed;

	[Header("Chances")]
	[SerializeField] private float _nothingChance;
	[SerializeField] private float _resourcesChance;
	[SerializeField] private float _hazardChance;
	[SerializeField] private float _pickupOneChance;
	[SerializeField] private float _pickupTwoChance;
	[SerializeField] private float _portalChance;



	private void Start()
	{

	}


	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.B))
		{

		}
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