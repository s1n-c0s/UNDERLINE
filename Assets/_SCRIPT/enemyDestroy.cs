using UnityEngine;

public class enemyDestroy : MonoBehaviour
{
    //public GameObject objectToDestroy;

    private void OnTriggerEnter(Collider Player)
    {
        if (Player.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
