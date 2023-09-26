using UnityEngine;

public class enemyDestroy : MonoBehaviour
{
    //public GameObject objectToDestroy;
    public PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            //Destroy(objectToDestroy);
            playerController.IncreaseRunsRemaining();
            Debug.Log("incr");
        }
    }
}
