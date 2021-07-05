using UnityEngine;

public class CoinController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            GameManager.Instance.IncrementScore();
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
