using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    [SerializeField]
    private PlayerController controller;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            return;
        }
        if (collision.collider.CompareTag("collider"))
        {
            controller.OnCharacterColliderHit();
            //collision.collider.gameObject.SetActive(false);
        }
    }
}
