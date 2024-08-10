using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeManager : MonoBehaviour
{
    public static SceneFadeManager Instance;

    [SerializeField]
    private Image _fadeoutImage;
    [Range(0.1f, 10f), SerializeField] private float _fadeoutSpeed = 5f;
    [Range(0.1f, 10f), SerializeField] private float _fadeInSpeed = 5f;
    [SerializeField] private Color _fadeoutStartColor;
    public bool IsFadingOut { get; private set; }
    public bool IsFadingIn { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        _fadeoutStartColor.a = 0f;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFadingOut)
        {
            if(_fadeoutImage.color.a < 1f)
            {
                _fadeoutStartColor.a += Time.deltaTime * _fadeoutSpeed;
                _fadeoutImage.color = _fadeoutStartColor;
            }
            else
            {
                IsFadingOut = false;
            }
        }

        if (IsFadingIn)
        {
            if (_fadeoutImage.color.a > 0f)
            {
                _fadeoutStartColor.a -= Time.deltaTime * _fadeInSpeed;
                _fadeoutImage.color = _fadeoutStartColor;
            }
            else
            {
                IsFadingIn = false;
            }
        }
    }
    public void StartFadeOut()
    {
        _fadeoutImage.color = _fadeoutStartColor;
        IsFadingOut = true;
    }

    public void StartFadeIn()
    {
        if (_fadeoutImage.color.a >= 1f)
        {
            _fadeoutImage.color = _fadeoutStartColor;
            IsFadingIn = true;
        }
    }
}
