using UnityEngine;

public class enemyDestroy : MonoBehaviour
{
    //public GameObject objectToDestroy;
    private PlayerController playerController;

    private void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            playerController = GetComponent<PlayerController>();
        }
        else
        {
            playerController = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Destroy(other.gameObject);
            //Destroy(objectToDestroy);
            if (other.GetComponent<HealthSystem>().GetCurrentHealth() == 0)
            {
                playerController.IncreaseRunsRemaining();
                Debug.Log("Heal + 1");
            }
        }
    }
}
