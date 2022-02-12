using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGeneratorController : MonoBehaviour
{
    public GameObject[] spawnedNodeGO;
    private void FixedUpdate()
    {
        ObjectPooler.Instance.SpawnFromPool("Circle");
    }
}
