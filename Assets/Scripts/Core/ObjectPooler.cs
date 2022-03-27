using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable] 
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton

    public static ObjectPooler Instance;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    #endregion

    public List<Pool> pools;

    public static Dictionary<string, Queue<GameObject>> poolDictionary;

    // Start is called before the first frame update
    void Start()
    {
        var test = new Dictionary<string, LinkedList<GameObject>>();
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
                DontDestroyOnLoad(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
            
        }
    }

    public void SpawnFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
        }

        while (poolDictionary[tag].Count > 0)
        {
            // take a GO from the object pooler
            GameObject objectToSpawn = poolDictionary[tag].Dequeue();

            // keep a GO list to calculate the node position
            GameController.Instance.activeGOs.Add(objectToSpawn);
        };
    }

    public void EnqueueToPool()
    {
        Queue<GameObject> objectPool = new Queue<GameObject>();
        string tagName = string.Empty;

        for (int i = 0; i < GameController.Instance.activeGOs.Count; i++)
        {
            GameObject GO = GameController.Instance.activeGOs[i];

            // remove visibility
            GO.SetActive(false);

            // reset all GO to white
            GO.GetComponent<SpriteRenderer>().color = Color.white;

            // reset isConnect to false
            GO.GetComponent<Node>().nodeModel.isConnect = false;

            tagName = GO.tag;

            objectPool.Enqueue(GO);
        };

        if(tagName != string.Empty && objectPool != null)
        {
            // return unused GO back to object pool by tag
            poolDictionary[tagName] = objectPool;

            // no more active GO on the scene
            GameController.Instance.activeGOs = new List<GameObject>();
        }
            
    }
}
