using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance;

    Camera mainCam;
    [SerializeField]
    TextMeshProUGUI scoreText;

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
        mainCam = Camera.main;
    }

    
    public void ChangeScoreTextColor(Color _color)
    {
        scoreText.color = _color;
    }
    public void ChangeBackgroundColor(Color _color)
    {
        mainCam.backgroundColor = _color;
    }
    
    public void Change3DObjectColor(MeshRenderer meshRenderer, Color color)
    {
        meshRenderer.material.color = color;
    }
    public void ChangeImageColor(Image image,Color color)
    {
        image.color = color;
    }
}
