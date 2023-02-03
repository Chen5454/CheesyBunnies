using UnityEngine;

public class RootInteractable : MonoBehaviour
{
    GameObject _root;
    RootMovement _rootMovement;
    public CameraFollow CameraFollow;

	private Collider2D _collider;

	[SerializeField] private int _points;
	[SerializeField] private bool _isHazard;



	private void Awake()
	{
		_collider = GetComponent<Collider2D>();
	}



	private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Root"))
        {
            _root = other.gameObject;
            _rootMovement = _root.GetComponent<RootMovement>();
            _rootMovement.enabled = false;
            //  CameraFollow.FollowLineReverse();

			if(_isHazard)
			{
				//destroys root or something
				GameState.Instance.TouchedHazard();
			}
			else
			{
				//gives points to the carrot or something
				GameState.Instance.TouchedResource(_points);
			}

            GameState.Instance.ChangeGameState(GameStates.CarrotView);
			_collider.enabled = false;



		}
    }



}