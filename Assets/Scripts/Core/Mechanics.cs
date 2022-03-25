using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanics : MonoBehaviour
{
    #region Singleton

    public static Mechanics Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public bool NodeOverlapped(Vector2 node1, Vector2 node2, float radius)
    {
        bool isOverlapping = false;

        float distSq =  (node1.x - node2.x) * (node1.x - node2.x) +
                     (node1.y - node2.y) * (node1.y - node2.y);
        float radSumSq = (radius + radius) * (radius + radius);
        if (Mathf.Approximately(distSq, radSumSq))
            isOverlapping = false;
        else if (distSq > radSumSq)
            isOverlapping = false;
        else
            isOverlapping = true;

        return isOverlapping;
    }
}
