using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    public Transform spawnPosition;
    [SerializeField]
    public Transform startPoint;
    [SerializeField]
    public Transform leftPoint;
    [SerializeField]
    public Transform rightPoint;
    [SerializeField]
    public Transform endPoint;
    [SerializeField]
    private GameObject[] obstacles;

    private bool isFromPool = false;
    private List<GameObject> pooledItems = new List<GameObject>();
    private int numOfObstaclesToPlace;

    private void Awake()
    {
        numOfObstaclesToPlace = Random.Range(5,10);
    }

    public void ActivateRandomObstacle()
    {
        for(int i = 0; i < numOfObstaclesToPlace; i++)
        {
            int randomNumber = Random.Range(0, obstacles.Length);
            Vector3 randomPos = new Vector3(Random.Range(leftPoint.position.x, rightPoint.position.x), 0, Random.Range(startPoint.position.z, endPoint.position.z));
            if(!isFromPool)
            {
                GameObject obstacle = Instantiate(obstacles[randomNumber], randomPos, Quaternion.identity, spawnPosition);
                pooledItems.Add(obstacle);
                obstacle.SetActive(true);
            }
            else
            {
                pooledItems[i].transform.position = randomPos;
                pooledItems[i].SetActive(true);
            }
        }
        isFromPool = true;
    }

    public void DeactivateAllObstacles()
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i].SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            other.gameObject.GetComponent<PlayerController>().CreateGround();
        }
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

}
