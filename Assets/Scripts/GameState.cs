using UnityEngine;

public class GameState : MonoBehaviour
{
	private static GameState _instance;
	public static GameState Instance => _instance;

	[SerializeField] private GameStates _gameCurrentState;
	bool _isPausing;


	private void Awake()
	{
		if (_instance == null)
			_instance = this;
		else if (_instance != this)
			Destroy(this.gameObject);
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
}

public enum GameStates
{
	CarrotView,
	RootView//can go to carrot view only when touching water, finished max length, or touching hazard
}