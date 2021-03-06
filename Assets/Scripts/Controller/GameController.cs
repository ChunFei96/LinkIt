using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region Singleton

    public static GameController Instance;
    private DatabaseController db;

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

    public Text TestTypeText;
    public Text InstructionText;
    public Text GameEndTestTypeText;
    public Text GameEndTotalTimeText;
    private InputField GameEndUserIDInput;
    private Text errorMsg;

    string nextNode;
    public static string instruction = "Next Node";
    public static string TMPA_Instruction = "Please connect all the nodes in ascending numerical order." + "\n Next Node >>>  ";

    public static string TMPB_Instruction = "Please connect all the nodes in ascending order alternatively" + "\n Next Node >>>  ";

    #region Public

    void Start()
    {
        TestTypeText = GameObject.Find("TestType").GetComponent<Text>();
        InstructionText = GameObject.Find("Instruction").GetComponent<Text>();


        if (GlobalManager.Instance.GetSelectedLevel() == GameRule.TMTA){
            nextNode = "2";
            instruction = TMPA_Instruction;
        }
        else if (GlobalManager.Instance.GetSelectedLevel() == GameRule.TMTB){ 
            nextNode = "A";
            instruction = TMPB_Instruction;
        }
    }

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
                {
                    if (!go.GetComponent<Node>().nodeModel.isConnect)
                        go.GetComponent<SpriteRenderer>().color = Color.white;
                    else
                        go.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
                }

                currentNode.GetComponent<SpriteRenderer>().color = Global_Var.CurPositionNodeColor;

                // Game Winning Condition
                if (ValidationController.Instance.IsNextNodeLast(linkedListGO, lastGO))
                {
                    // Stop Timer
                    TimerController.Instance.StopTimer();

                    // Show Pop up to input patient ID 
                    GameEndPanel.SetActive(true);
                    GameEndTestTypeText = GameObject.Find("GameEndTestType").GetComponent<Text>();
                    GameEndTotalTimeText = GameObject.Find("GameEndTotalTime").GetComponent<Text>();
                    GameEndUserIDInput = GameObject.Find("Input_UserID").GetComponent<InputField>();

                    GameEndTotalTimeText.text = "Total Time: " + TimerController.Instance.GetTime();
                    GameEndTestTypeText.text = TestTypeText.text;

                    errorMsg = GameObject.Find("txt_ErrorMsg").GetComponent<Text>();
                    errorMsg.text = "";
                }
                else 
                    // get Next node
                    nextNode = linkedListGO.Find(lastGO.GetComponent<Node>().nodeModel.value).Next.Value;
            }
            else
            {
                selectedGOInstance.Enqueue(firstGO);
                //GameController.Instance.selectedGOInstance.Enqueue(lastGO);

                currentNode.GetComponent<SpriteRenderer>().color = Global_Var.UnsuccessNodeColor;
            }
        }
        else
        {
            // reset all GO to color white
            foreach (var go in activeGOs)
            {
                if (!go.GetComponent<Node>().nodeModel.isConnect)
                    go.GetComponent<SpriteRenderer>().color = Color.white;
                else
                    go.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
            }

            currentNode.GetComponent<SpriteRenderer>().color = Global_Var.CurPositionNodeColor;
        }

        // set instructon 
        InstructionText.text = instruction + nextNode;
        Debug.Log("next:");
    }

    public Vector2 CalculateNodePos()
    {
        Vector2 pos = new Vector2();

        int Count = 0;

        while (Count != activeGOs.Count)
        {
            Count = 0;
            float xRand = Random.Range(-8.0f, 8.0f);
            float yRand = Random.Range(-4.0f, 3.0f);
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

    public void SaveRecord()
    {
        db = new DatabaseController();
        db.InitDb();

        string paitientID = GameEndUserIDInput.text;
        string GameMode = TestTypeText.text;
        string TimeTaken = TimerController.Instance.GetTime();
        string CreatedOn = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        System.DateTime ConvertcreatedOnDateTime = System.Convert.ToDateTime(CreatedOn);

        if (!db.ValidPatientId(paitientID))
        {
            errorMsg.GetComponent<Text>().text = "Error: Invalid id!";
            Debug.Log("Invalid id: " + paitientID);
            return;
        }

        Score saveScore = new Score(paitientID, GameMode, TimeTaken, ConvertcreatedOnDateTime);

        // Save to db
        db.AddScore(saveScore);
        Debug.Log("db saveScore successfuly!");

        // if success disable field
        GameEndUserIDInput.enabled = false;

        ClearNodes();
        SceneManager.LoadScene("LeaderboardScene");
    }

    public void ClearNodes()
    {
        ObjectPooler.Instance.EnqueueToPool();
        //for (int i = 0; i < activeGOs.Count; i++)
        //{
        //    GameObject objectToSpawn = GameController.Instance.activeGOs[i];
        //    objectToSpawn.SetActive(false);
        //}
    }
}
