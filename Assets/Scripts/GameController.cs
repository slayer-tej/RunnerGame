using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPos;
    private ServicePool servicePool;
    private int count = 0;
    public float playerDistance;
    [SerializeField]
    private List<GroundController> pooledItems = new List<GroundController>();

    void Start()
    {
        servicePool = gameObject.GetComponent<ServicePool>();
        GetItemFromPool();
    }

    internal void GetItemFromPool()
    {
        GameObject ground = servicePool.GetObject();
        ground.transform.position = spawnPos.position;
        GroundController controller = ground.gameObject.GetComponent<GroundController>();
        spawnPos = controller.spawnPosition;
        controller.ActivateRandomObstacle();
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
}
