using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    [SerializeField]
    private int playerHelalth;

    public Transform spawnPosition;
    [SerializeField]
    public Transform startPoint;
    [SerializeField]
    public Transform leftPoint;
    [SerializeField]
    public Transform rightPoint;
    [SerializeField]
    public Transform yMin;
    [SerializeField]
    public Transform yMax;
    [SerializeField]
    public Transform endPoint;
    [SerializeField]
    private GameObject[] obstacles;
    [SerializeField]
    private GameObject coinPrefab;
    [SerializeField]
    private Transform coinPlaceHolder;

    private bool isFromPool = false;
    private List<GameObject> pooledItems = new List<GameObject>();
    private int numOfObstaclesToPlace;

    private void Awake()
    {
        numOfObstaclesToPlace = Random.Range(5,7);
    }

    public void SpawnCoins()
    {
        int coinsToSpawn = 10;
        for (int i = 0; i < coinsToSpawn; i++)
        {
            Vector3 randomPoint = GetRandomPoint();

            GameObject temp = Instantiate(coinPrefab,coinPlaceHolder);
            randomPoint.y = 1;
            temp.transform.position = randomPoint;
        }
    }

    public void ActivateRandomObstacle()
    {
        for(int i = 0; i < numOfObstaclesToPlace; i++)
        {
            int randomNumber = Random.Range(0, obstacles.Length);
            Vector3 randomPos = GetRandomPoint();
            if (!isFromPool)
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

    private Vector3 GetRandomPoint()
    {
        return new Vector3(Random.Range(leftPoint.position.x, rightPoint.position.x),
                                                    Random.Range(yMin.position.y, yMax.position.y),
                                                    Random.Range(startPoint.position.z, endPoint.position.z));
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
