using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region Singleton

    public static GameController Instance;

    private void Awake()
    {
        Instance = this;
        selectedGOInstance = new Queue<GameObject>();
        activeGOs = new List<GameObject>();
        linkedListGO = new LinkedList<string>();
        GameEndPanel.SetActive(false);
    }

    #endregion

    public Queue<GameObject> selectedGOInstance;

    public LinkedList<string> linkedListGO;

    [SerializeField]
    public List<GameObject> activeGOs;

    [SerializeField]
    public float gapOffset;

    [SerializeField]
    public GameObject GameEndPanel;

    [SerializeField]
    public Text GameEndTotalTimeText;

    #region Public

    public void NodeOnMouseDown(GameObject currentNode)
    {
        selectedGOInstance.Enqueue(currentNode);

        if (selectedGOInstance.Count == 2)
        {
            GameObject firstGO = selectedGOInstance.Dequeue();
            GameObject lastGO = selectedGOInstance.Dequeue();


            if (ValidationController.Instance.IsNextNodeValidated(linkedListGO, firstGO, lastGO))
            {
                LineRenderer line = new GameObject("Line").AddComponent<LineRenderer>();
                line.startColor = Color.black;
                line.endColor = Color.black;
                line.startWidth = 0.1f;
                line.endWidth = 0.1f;

                firstGO.GetComponent<Node>().nodeModel.isConnect = true;
                lastGO.GetComponent<Node>().nodeModel.isConnect = true;

                var firstPosition = firstGO.transform.position;
                var secondPosition = lastGO.transform.position;

                Vector3[] pathPoints = { firstPosition, secondPosition };
                line.positionCount = 2;
                line.SetPositions(pathPoints);

                selectedGOInstance.Enqueue(currentNode);

                // reset all GO to color white
                foreach (var go in activeGOs)
                    go.GetComponent<SpriteRenderer>().color = Color.white;

                currentNode.GetComponent<SpriteRenderer>().color = Color.green;

                // Game Winning Condition
                if(ValidationController.Instance.IsNextNodeLast(linkedListGO, lastGO))
                {
                    // Stop Timer
                    TimerController.Instance.StopTimer();

                    // Show Pop up to input patient ID 
                    GameEndPanel.SetActive(true);
                    GameEndTotalTimeText.text = "Total Time: " + TimerController.Instance.GetTime();

                }
            }
            else
            {
                selectedGOInstance.Enqueue(firstGO);
                //GameController.Instance.selectedGOInstance.Enqueue(lastGO);

                currentNode.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
        else
        {
            // reset all GO to color white
            foreach (var go in activeGOs)
                go.GetComponent<SpriteRenderer>().color = Color.white;

            currentNode.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    public Vector2 CalculateNodePos()
    {
        Vector2 pos = new Vector2();

        int Count = 0;

        while(Count != activeGOs.Count)
        {
            Count = 0;
            float xRand = Random.Range(-8.0f, 8.0f);
            float yRand = Random.Range(-4.0f, 4.0f);
            pos = new Vector2(xRand, yRand);

            foreach (var go in activeGOs)
            {
                if (!NodeOverlapped(pos, new Vector2(go.transform.position.x, go.transform.position.y), go.GetComponent<CircleCollider2D>().radius + gapOffset))
                    Count += 1;
                else
                    break;

            }
        }

        return pos;
    }

    #endregion

    #region Private 
    private bool NodeOverlapped(Vector2 node1, Vector2 node2, float radius)
    {
        bool isOverlapping = false;

        float distSq = (node1.x - node2.x) * (node1.x - node2.x) +
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

    #endregion
}
