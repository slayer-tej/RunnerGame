using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameManager: MonoSingletonGeneric<GameManager>
{
    [SerializeField]
    private Transform spawnPos;
    [SerializeField]
    private TextMeshProUGUI coinInput;
    [SerializeField]
    private TextMeshProUGUI scoreInput;
    [SerializeField]
    private List<GroundController> pooledItems = new List<GroundController>();
    [SerializeField]
    private GameObject[] hearts;

    private ServicePool servicePool;
    private int count = 0;
    public float playerDistance;
    private int coins;

   

    void Start()
    {
        servicePool = gameObject.GetComponent<ServicePool>();
        GetItemFromPool();
    }
    private void Update()
    {
        UpdateDistanceCovered();
    }

    public void UpdateDistanceCovered()
    {
        int dist = Convert.ToInt32(playerDistance);
        scoreInput.text = dist.ToString();

    }

    public void IncrementScore()
    {
        //scoreInput.text = playerDistance;
        coins++;
        coinInput.text = coins.ToString();
    }

    internal void GetItemFromPool()
    {
        GameObject ground = servicePool.GetObject();
        ground.transform.position = spawnPos.position;
        GroundController controller = ground.gameObject.GetComponent<GroundController>();
        spawnPos = controller.spawnPosition;
        controller.ActivateRandomObstacle();
        controller.SpawnCoins();
        controller.Enable();
        if (pooledItems.Count < 3)
        {
            pooledItems.Add(controller);
        }
        if (pooledItems.Count >= 3)
        {
            pooledItems[count].DeactivateAllObstacles();
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        servicePool.ReturnItem(pooledItems[count].gameObject);
        pooledItems[count].Disable();
        if (count >= 2)
        {
            count = 0;
            return;
        }
        count++;
    }

    public void DisplayHearts(int life)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < life);
        }
    }
}
