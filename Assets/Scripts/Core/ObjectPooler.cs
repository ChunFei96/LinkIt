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
        Instance = this;
        linkedListGO = new LinkedList<string>();
    }

    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public LinkedList<string> linkedListGO;

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
            }

            poolDictionary.Add(pool.tag, objectPool);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject SpawnFromPool (string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        if (poolDictionary[tag].Count > 0)
        {
            GameObject objectToSpawn = poolDictionary[tag].Dequeue();
            GameController.Instance.allGOInstance.Add(objectToSpawn);

            int fileName = poolDictionary[tag].Count + 1;
            string filePath = Environment.CurrentDirectory + "/Assets/Resources/" + fileName + ".png";

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = GameController.Instance.CalculateNodePos();
            objectToSpawn.GetComponent<Node>().nodeModel.value = (poolDictionary[tag].Count + 1).ToString();
            objectToSpawn.GetComponent<SpriteRenderer>().sprite = SpriteController.instance.LoadNewSprite(filePath);
            //poolDictionary[tag].Enqueue(objectToSpawn);

            linkedListGO.AddFirst(objectToSpawn.GetComponent<Node>().nodeModel.value.ToString());

            return objectToSpawn;
        }
        else
            return null;
        
    }
}
