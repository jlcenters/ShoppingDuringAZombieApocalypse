using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 
 * 
    GAME CONTROLLER FUNCTIONS:
        Overseer: performs different functions depending on current game state
        game tutorial state will freeze controls and movement for specified time at start of scene
        communicates with UI and menus
        public function to check current game state
 *
 *
 */
public enum GameStates
{
    GameTutorial, PlayGame, Pause, GameEnd, GameOver
}
public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    private PlayerInputController inputController;

    private GameStates currentState = GameStates.GameTutorial;

    [SerializeField] private float gameTutorialCounter = 10f;

    //ui objects
    [SerializeField] private GameObject pauseMenu;

    private void Awake()
    {
        Instance = this;

        inputController = GetComponent<PlayerInputController>();
    }
    private void Start()
    {
        inputController.OnPause += PlayerInputController_OnPause;
    }
    private void Update()
    {
        switch (currentState)
        {
            case GameStates.GameTutorial:
                gameTutorialCounter -= Time.deltaTime;

                if(gameTutorialCounter <= 0)
                {
                    currentState = GameStates.PlayGame;
                }

                break;
            case GameStates.PlayGame:
                break;
            case GameStates.Pause:
                
                break;
            case GameStates.GameEnd:
                break;
        }
    }
    private void OnDestroy()
    {
        Instance = null;
        inputController.OnPause -= PlayerInputController_OnPause;
    }


    public bool IsCurrentState(GameStates state)
    {
        return currentState == state;
    }
    private void PlayerInputController_OnPause()
    {
        //if current state is pause, unpause; otherwise, pause
        if(currentState == GameStates.Pause)
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            currentState = GameStates.PlayGame;

        }
        else 
        {
            Time.timeScale = 0f;
            currentState = GameStates.Pause;
            pauseMenu.SetActive(true);
        }
    }
}
