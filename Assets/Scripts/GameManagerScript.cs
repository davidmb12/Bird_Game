using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance;
    int gamePlayed = 0;

    [Header("References")]
    [SerializeField]
    LevelManager levelManager;
    [SerializeField]
    GameObject playerPrefab;
    [SerializeField]
    GameObject startButton;
    [SerializeField]
    Camera mainCam;
    public float levelSpeed;
    bool gameStarted;

    [SerializeField]
    LevelScriptableObject[] levelsList;
    [SerializeField]
    SceneField[] scenes;
    ColorManager colorManager;
    Transform currentLevelObject;

    int currentLevelIdx = 0;
    int currentSceneIndex = 0;
    [SerializeField]
    Color initBackgroundColor;
    enum GameState
    {
        Menu,
        InGame,
        GameOver
    }
    GameState currentGameState;
    // Start is called before the first frame update
    private void Awake()
    {
        Application.targetFrameRate = 60;

        if (Instance == null)
        {
            Instance = this;
        }
        
    }
    void Start()
    {    
        colorManager = FindObjectOfType<ColorManager>();
        currentGameState = GameState.Menu;
        gameStarted = false;
        levelManager = LevelManager.Instance;
        levelManager.SpawnLevel();

    }

    // Update is called once per frame
    void Update()
    {
        if(gameStarted) 
        {
            levelManager.HandleLevelMovement();
        }
    }

    public void HandleGameOver(int currentScore)
    {
        UIManager.Instance.UpdateLastScore(currentScore);
        UIManager.Instance.HideScoreText();
        int maxScore = PlayerPrefs.GetInt("MaxScore",0);
        if(currentScore > maxScore)
        {
            PlayerPrefs.SetInt("MaxScore", currentScore);
        }

        gameStarted = false;
        gamePlayed += 1;
        currentGameState = GameState.Menu;
        CameraManager.Instance.ResetCameraPosition();
        ResetMenuUI();
        levelManager.DeSpawnLevel();

        AudioManager.Instance.StopMusic();
        if(gamePlayed%5 == 0)
        {
            AdsManager.Instance.interstitialAds.ShowInterstitialAd();
        }
    }
    public void StartGame()
    {
        UIManager.Instance.ShowScoreText();
        mainCam.backgroundColor = initBackgroundColor;
        var player= Instantiate(playerPrefab);
        CameraManager.Instance.SetPlayerController(player.GetComponent<PlayerController>());
        startButton.SetActive(false);
        gameStarted = true;
        currentGameState = GameState.InGame;
        AudioManager.Instance.StartMusic();

    }
    public void ResetMenuUI()
    {
        colorManager.ChangeBackgroundColor(Color.black);
        startButton.SetActive(true);
        UIManager.Instance.GetMaxScore();
        UIManager.Instance.ResetScore();
    }

    public void GoToNextScene()
    {
        if(currentSceneIndex < scenes.Length)
        {
            currentSceneIndex++;
        }
        
        SceneSwapManager.SwapScene(scenes[currentSceneIndex]);
    }
}
