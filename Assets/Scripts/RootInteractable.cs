using UnityEngine;

public class RootInteractable : MonoBehaviour
{
    GameObject _root;
    RootMovement _rootMovement;
    public CameraFollow CameraFollow;

	[SerializeField] private int _points;
	[SerializeField] private bool _isHazard;
	bool _canGiveNutrient = true;
	AudioSource _audioSource;

	private void Awake()
	{
		_canGiveNutrient = true;
        _audioSource = GetComponent<AudioSource>();	

    }



	private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Root"))
        {
            if (tag == "Portal")
            {
                Debug.Log("Exit Portal");
                other.GetComponent<FieldOfView>().SwitchLayers();
                return;
            }
            _root = other.gameObject;
            _rootMovement = _root.GetComponent<RootMovement>();
			_rootMovement.RootFillImage.fillAmount = 1;
            _rootMovement.enabled = false;
            AudioManager.Instance.StopAudio(AudioManager.Instance.digLoop);

            //  CameraFollow.FollowLineReverse();

            if (_isHazard)
			{
				//destroys root or something
				GameState.Instance.TouchedHazard();
                _audioSource.clip = AudioManager.Instance.Acid;
                _audioSource.Play();
            }
			else
			{
				//gives points to the carrot or something
				if (_canGiveNutrient)
				{
					_audioSource.clip = AudioManager.Instance.Buuble;
					_audioSource.Play();
                    _canGiveNutrient = false;
					GameState.Instance.TouchedResource(_points);
				}
			}

            GameState.Instance.ChangeGameState(GameStates.CarrotView);

		}
		else if (other.CompareTag("FOV") && tag!="Portal")
        {
			if (other.gameObject.layer != gameObject.layer)
            {
                if (transform.childCount > 0)
                    transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            }
        }
       
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("FOV") && tag != "Portal")
        {
            if (other.gameObject.layer != gameObject.layer)
            {
                if (transform.childCount > 0)
                    transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        
    }


}