using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    [SerializeField]
    private PlayerController controller;
    private int damage = 1;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            return;
        }
        if (collision.collider.CompareTag("collider"))
        {
            controller.UpdateLives(damage);
            collision.collider.gameObject.SetActive(false);
        }
    }
}
