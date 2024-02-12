using UnityEngine;

public class enemyDestroy : MonoBehaviour
{
    //public GameObject objectToDestroy;
    private PlayerController playerController;
    [SerializeField] private ParticleSystem fx_slash;

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
            fx_slash.Play();
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
