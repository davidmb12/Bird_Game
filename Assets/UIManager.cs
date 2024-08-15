using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [SerializeField] RectTransform uiCanvas;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI maxScoreText;
    [SerializeField] TextMeshProUGUI lastScoreText;
    [SerializeField] RectTransform fadeCanvas;
    [SerializeField] PlayerController playerController;

    int maxScore;
    public void SetPlayerController(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GetMaxScore();
    }

    public void GetMaxScore()
    {
        maxScore = PlayerPrefs.GetInt("MaxScore");
        maxScoreText.text = maxScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController)
        {
            ColorManager.Instance.ChangeScoreTextColor(playerController.currentPlayerColor);
        }
    }
    public void HideScoreText()
    {
        scoreText.transform.gameObject.SetActive(false);
    }
    public void ShowScoreText()
    {
        scoreText.transform.gameObject.SetActive(true);

    }
    public void UpdateScore(int currentScore)
    {
        scoreText.text = currentScore.ToString();
    }

    public void UpdateLastScore(int lastScore)
    {
        lastScoreText.text = lastScore.ToString();
    }
    public void ResetScore()
    {
        scoreText.text = "0";
    }


}
