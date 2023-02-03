using UnityEngine;
using System.Collections.Generic;
using System;

public class GameState : MonoBehaviour
{
	private static GameState _instance;
	public static GameState Instance => _instance;


	[Header("Carrot Settings")]
	//references
	[SerializeField] private SpriteRenderer _carrotRenderer;
	[SerializeField] private List<CarrotVisual> _carrotVisuals = new List<CarrotVisual>();
	int currentVisualIndex;

	[SerializeField] private int _currentPoints;//will effect somehow on the next root

	[SerializeField] private int _hazardtouched;

	[Header("Game State")]
	[SerializeField] private GameStates _gameCurrentState;
	bool _isPausing;


	[Header("Root Spawn")]
	[SerializeField] private GameObject _rootPF;
	[SerializeField] private Transform _spawnPos;
	[SerializeField] private Transform _parent;

	//serialized for debugging
	[SerializeField] private RootMovement _currentRoot;

	private void Awake()
	{
		if (_instance == null)
			_instance = this;
		else if (_instance != this)
			Destroy(this.gameObject);

		currentVisualIndex = 0;
		UpdateCarrotVisuals(0);
	}

	private void Start()
	{
		//init the start state
		_gameCurrentState = GameStates.CarrotView;
		EnterState(_gameCurrentState);
		
	}
	private void Update()
	{
		if (_gameCurrentState == GameStates.CarrotView)
		{
			//can push on down button to change to root view
			if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
			{
				ChangeGameState(GameStates.RootView);
				InstantiateNewRoot();
			}



			//after playing for a while can click on the up button to pick up the carrot,
			//maybe carrot script or gamemanager which he can handle that and return a bool if can to pick it up

			//option to open pause menu
			PauseMenuHandle();

		}
		else if (_gameCurrentState == GameStates.RootView)
		{
			//option to open pause menu
			PauseMenuHandle();

		}
		//else if (_gameCurrentState == GameStates.PauseMenu)
		//{
		//	//option to click on certain button to unpause
		//}
	}

	#region Game State
	public void ChangeGameState(GameStates state)
	{
		ExitState(_gameCurrentState);
		_gameCurrentState = state;
		EnterState(_gameCurrentState);
	}

	void EnterState(GameStates stateToEnter)
	{
		switch (stateToEnter)
		{
			case GameStates.CarrotView:
				EnterCarrotView();
				break;
			case GameStates.RootView:
				EnterRootView();
				break;
			default:
				break;
		}
	}
	void ExitState(GameStates stateToEnter)
	{
		switch (stateToEnter)
		{
			case GameStates.CarrotView:
				ExitCarrotView();
				break;
			case GameStates.RootView:
				ExitRootView();
				break;
			default:
				break;
		}
	}
	#endregion
	#region Root View
	void EnterRootView()
	{
		//enable root movement or instantiate new root

		//change camera view to root camera
		CameraController.Instance.ChangeCamera(1);
	}
	void ExitRootView()
	{
		//disable root movement
	}
	#endregion
	#region Carrot View
	void EnterCarrotView()
	{
		CameraController.Instance.ChangeCamera(0);
	}
	void ExitCarrotView()
	{
		
	}
	#endregion
	#region Pause menu
	void EnterPauseMenu()
	{
		Time.timeScale = 0;
		//making root stop moving
	}
	void ExitPauseMenu()
	{
		Time.timeScale = 1f;


		//if at root view
		//making root to continue moving
	}


	void PauseMenuHandle()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			_isPausing = !_isPausing;

			if (_isPausing)
			{
				EnterPauseMenu();
			}
			else
			{
				ExitPauseMenu();
			}

		}
	}

	#endregion

	#region Root management
	void InstantiateNewRoot()
	{
	RootSpawn randomSpawn = GetRandomRootSpawn();
		float startAngle = randomSpawn.SpawnDirection;
		Vector2 spawnPos = randomSpawn.transform.position;

		randomSpawn.OnSpawnNewObject(startAngle);
		RootMovement newRoot = Instantiate(_rootPF, spawnPos, Quaternion.identity ,_parent).GetComponent<RootMovement>();
		newRoot.SetInitialAngle(startAngle);
		_currentRoot = newRoot;
		_currentRoot.AddTotalLength(_currentPoints);
		CameraController.Instance.SetNewRootCameraFollow(_currentRoot.transform);
	}


	#endregion

	#region Carrot management
	public void TouchedHazard()
	{
		_hazardtouched++;
	}
	public void TouchedResource(int points)
	{
		_currentPoints += points;

		CheckIfNeedToChangeCarrotVisual();
	}
	void CheckIfNeedToChangeCarrotVisual()
	{
		int newIndex = currentVisualIndex;
		for (int i = currentVisualIndex; i < _carrotVisuals.Count; i++)
		{
			if(_currentPoints >= _carrotVisuals[i].PointsRequired)
			{
				newIndex = i;
			}
			else
			{
				break;
			}
		}

		if(newIndex != currentVisualIndex)
		{
			//change visuals
			UpdateCarrotVisuals(newIndex);
		}

	}
	void UpdateCarrotVisuals(int index)
	{
		
		
		if(_carrotRenderer != null)
		{
			currentVisualIndex = index;
			_carrotRenderer.sprite = _carrotVisuals[currentVisualIndex].CarrotSprite;
		}
		else
		{
			//debug
			_carrotVisuals[currentVisualIndex].TestCarrotGO.SetActive(false);
			currentVisualIndex = index;
			_carrotVisuals[currentVisualIndex].TestCarrotGO.SetActive(true);
		}
	}

	RootSpawn GetRandomRootSpawn() => _carrotVisuals[currentVisualIndex].GetRandomSpawnTrans();
	#endregion

}
[Serializable]
public class CarrotVisual
{
	[SerializeField] private Sprite _carrotSprite;
	public Sprite CarrotSprite => _carrotSprite;

	[SerializeField] private int _pointsRequired;
	public int PointsRequired => _pointsRequired;

	//debug
	[SerializeField] private GameObject _testCarrotGO;
	public GameObject TestCarrotGO => _testCarrotGO;

	[SerializeField] private List<RootSpawn> _spawnList = new List<RootSpawn>();
	public List<RootSpawn> SpawnList => _spawnList;

	public RootSpawn GetRandomSpawnTrans()
	{
		int randomIndex = UnityEngine.Random.Range(0, _spawnList.Count);
		return _spawnList[randomIndex];
	}
}


public enum GameStates
{
	CarrotView,
	RootView//can go to carrot view only when touching water, finished max length, or touching hazard
}