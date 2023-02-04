using UnityEngine;

public class Pickup : MonoBehaviour
{
	[SerializeField] private PickUpType _pickupType;
	[SerializeField] private float _bonus;


	void HandleBonuses(RootMovement root)
	{
		switch (_pickupType)
		{
			case PickUpType.More_Movement:
				MoreMovement(root);
				break;
			case PickUpType.Speed:
				AddSpeed(root);
				break;
			default:
				break;
		}
	}


	void MoreMovement(RootMovement root)
	{
		root.AddLength(_bonus);
		Debug.Log("Picked up more movement bonus");
	}
	void AddSpeed(RootMovement root)
	{
		//movementspeed
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.GetComponent<RootMovement>())
		{
			RootMovement root = collision.GetComponent<RootMovement>();
			HandleBonuses(root);
			this.gameObject.SetActive(false);
		}
	}


}


public enum PickUpType
{
	More_Movement,
	Speed
}