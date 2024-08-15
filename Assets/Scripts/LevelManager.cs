using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    // Start is called before the first frame update
    [SerializeField]
    LevelScriptableObject[] levelsList;
    [SerializeField]
    GameObject[] levelParts;

    Transform currentLevelObject;

    [Header("References")]
    GameManagerScript gameManager;
    int currentLevelIndex;
    [Header("Level Settings")]
    [SerializeField]
    bool randomLevel;

    [Header("Spawn Settings")]
    [SerializeField] Transform startingSpawnPoint;
    [SerializeField]
    float levelPartSpawnRate;
    [Header("Randomize Settings")]
    [SerializeField] float levelSpawnRate;
    [SerializeField] float spawnHeight;
    [SerializeField] int spawnMaxAmount;
    [SerializeField] Transform nextSpawnPosition;
    [SerializeField] Transform levelHolder;
    public int currentSpawnedParts;


    [SerializeField]
    float initialLevelSpeed;
    [SerializeField]
    public float currentLevelSpeed;
    [SerializeField]
    float levelAcceleration;
    [SerializeField]
    float levelMaxSpeed;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        currentLevelIndex = PlayerPrefs.GetInt("CurrentLevel",0);
        gameManager = GameManagerScript.Instance;
        ResetLevelRandomLevel();
    }
    public void ResetLevelRandomLevel()
    {
        currentLevelSpeed = initialLevelSpeed;
        nextSpawnPosition = startingSpawnPoint;
        currentSpawnedParts = 0;
        if (randomLevel)
        {
            currentLevelObject = levelHolder;
        }
    }
    public void SpawnLevel()
    {
       
        if(!randomLevel)
        {
            currentLevelObject = Instantiate(levelsList[currentLevelIndex].levelObject);

        }
    }
    

    private void FixedUpdate()
    {
        if (randomLevel)
        {
            HandleRandomLevelSpawning();
        }
    }
    public void DeSpawnLevel()
    {
        foreach (var child in currentLevelObject.GetComponentsInChildren<LevelPart>())
        {
            Destroy(child.gameObject);
        }
        currentLevelObject.transform.position = Vector3.zero;
        ResetLevelRandomLevel();
    }

    void HandleRandomLevelSpawning()
    {
        if (currentSpawnedParts < spawnMaxAmount)
        {
            int idx = Random.Range(0,levelParts.Length);
            GameObject nextLevelPart = levelParts[idx];
            
            SpawnLevelPart(nextLevelPart);
        }
    }
    public void SpawnLevelPart(GameObject levelPart)
    {
        GameObject currentLevel = Instantiate(levelPart,levelHolder);
        currentLevel.transform.position = nextSpawnPosition.position;
        
        nextSpawnPosition = currentLevel.GetComponent<LevelPart>().endPoint;
        currentSpawnedParts += 1;
    }

    
    void HandleLevelAcceleration()
    {
        currentLevelSpeed += levelAcceleration * Time.deltaTime;
    }
    public void HandleLevelMovement()
    {
        currentLevelObject.transform.Translate(Vector3.left * currentLevelSpeed * Time.deltaTime);
        HandleLevelAcceleration();
    }
    // Update is called once per frame
    
}
