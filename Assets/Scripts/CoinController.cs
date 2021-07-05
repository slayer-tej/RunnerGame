using UnityEngine;

public class CoinController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.IncrementScore();
        }
        Destroy(gameObject);
    }
}
