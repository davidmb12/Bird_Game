using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    [SerializeField]
    CinemachineVirtualCamera virtualCamera;
    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    Vector3 initCameraPosition;
    [SerializeField]
    Transform levelHolder;
    PlayerController playerController;
    // Start is called before the first frame update
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {  
    }
    private void FixedUpdate()
    {
        if(playerController != null)
        {
           
            HandleCameraMovement();
        }

    }
    public void SetPlayerController(PlayerController _playerController)
    {
        playerController = _playerController;
    }

     public void ResetCameraPosition() 
    {
        mainCamera.transform.position = initCameraPosition;
    }

    void HandleCameraMovement()
    {
        if (!playerController.isGrounded && (playerController.transform.position.x >= mainCamera.transform.position.x))
        {
            Vector3 newCamPosition = new Vector3(playerController.transform.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newCamPosition, Time.fixedDeltaTime *0.25f);

        }
    }
}
