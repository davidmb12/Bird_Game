using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [SerializeField] RectTransform uiCanvas;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] RectTransform fadeCanvas;
    [SerializeField] PlayerController playerController;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        ColorManager.Instance.ChangeScoreTextColor(playerController.currentPlayerColor);

    }

    public void UpdateScore(int currentScore)
    {
        scoreText.text = currentScore.ToString();
    }


}
