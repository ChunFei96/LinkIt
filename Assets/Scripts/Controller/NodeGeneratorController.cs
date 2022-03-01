using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGeneratorController : MonoBehaviour
{
    void Start()
    {
        // spawn all the circle tagged GO from pool to the scene
        // store all the GO in the scene to the allGOInstance 
        ObjectPooler.Instance.SpawnFromPool("Circle");

        // apply game rule based on the level selection
        GameRuleController.Instance.TMTA();
    }
}
