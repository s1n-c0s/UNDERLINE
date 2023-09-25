using UnityEngine;

public class enemyDestroy : MonoBehaviour
{
    public GameObject objectToDestroy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            Destroy(objectToDestroy);
        }
    }
}
