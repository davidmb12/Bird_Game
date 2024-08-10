using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaItem : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float colorRegenValue;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            PlayerController player;
            other.TryGetComponent<PlayerController>(out player);
            if(player != null)
            {
                player.Heal(colorRegenValue);
                Destroy(gameObject);
            }

        }
    }
}
