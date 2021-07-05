using UnityEngine;

public class ServicePool : PoolingService<GameObject>
{
    [SerializeField]
    private GameObject ground;
  

    public GameObject GetObject()
    {
        return GetItem();
    }

    protected override GameObject CreateItem()
    {
        GameObject pooledItem = Instantiate(ground);
        return pooledItem;
    }

  
}
