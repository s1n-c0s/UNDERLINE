using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
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
