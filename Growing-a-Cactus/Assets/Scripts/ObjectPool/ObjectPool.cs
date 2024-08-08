using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;

    private int size = 10;
    private Queue<GameObject> pool = new Queue<GameObject>();

    void Start()
    {
        InitPool();
    }

    private void InitPool()
    {
        for(int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        if(pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.SetParent(transform);
            return obj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
