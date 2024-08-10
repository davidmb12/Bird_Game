using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPart : MonoBehaviour
{
    
    [SerializeField]
    public Transform endPoint;
    [SerializeField]
    public Transform startPoint;
    [SerializeField]
    float distanceToDestroy;
    public float offsetY;


    private void Start()
    {
        offsetY = Random.Range(-offsetY, offsetY);
        transform.position = new Vector3(transform.position.x, offsetY, transform.position.z);
    }
    private void Update()
    {
        float currentDistanceFromCamera =Mathf.Abs(transform.position.x - CameraManager.Instance.transform.position.x);
        if(currentDistanceFromCamera > distanceToDestroy&& transform.position.x < 0)
        {
            LevelManager.Instance.currentSpawnedParts -= 1;
            Destroy(gameObject);
        }
    }



}
