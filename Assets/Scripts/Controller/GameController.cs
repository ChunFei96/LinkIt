using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Singleton

    public static GameController Instance;

    private void Awake()
    {
        Instance = this;
        selectedGOInstance = new Queue<GameObject>();
        allGOInstance = new List<GameObject>();
    }

    #endregion

    public Queue<GameObject> selectedGOInstance;

    [SerializeField]
    public List<GameObject> allGOInstance;

    [SerializeField]
    public float gapOffset;
    public Vector2 CalculateNodePos()
    {
        Vector2 pos = new Vector2();

        int Count = 0;

        while(Count != allGOInstance.Count)
        {
            Count = 0;
            float xRand = Random.Range(-8.0f, 8.0f);
            float yRand = Random.Range(-4.0f, 4.0f);
            pos = new Vector2(xRand, yRand);

            foreach (var go in allGOInstance)
            {
                if (!Mechanics.Instance.NodeOverlapped(pos, new Vector2(go.transform.position.x, go.transform.position.y), go.GetComponent<CircleCollider2D>().radius + gapOffset))
                    Count += 1;
                else
                    break;

            }
        }

        return pos;
    }
}
