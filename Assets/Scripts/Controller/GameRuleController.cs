using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameRuleController : MonoBehaviour
{
    #region Singleton

    public static GameRuleController Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public void TMTA()
    {
        for (int i = 0; i < GameController.Instance.activeGOs.Count; i++)
        {
            GameObject objectToSpawn = GameController.Instance.activeGOs[i];

            // apply img texture on GO
            int fileName = i + 1;
            string filePath = Environment.CurrentDirectory + "/Assets/Resources/" + fileName + ".png";
            objectToSpawn.GetComponent<SpriteRenderer>().sprite = SpriteController.instance.LoadNewSprite(filePath);

            // apply visibility
            objectToSpawn.SetActive(true);

            // assign position to the GO without overlapping others
            objectToSpawn.transform.position = GameController.Instance.CalculateNodePos();

            // assign the numeric value to the GO
            objectToSpawn.GetComponent<Node>().nodeModel.value = (i + 1).ToString();

            // LinkedList for validation purpose
            GameController.Instance.linkedListGO.AddLast((i + 1).ToString());

            // default select the first node
            if (i == 0)
            {
                objectToSpawn.GetComponent<SpriteRenderer>().color = Color.green;
                objectToSpawn.GetComponent<Node>().nodeModel.isConnect = true;
                GameController.Instance.selectedGOInstance.Enqueue(objectToSpawn);
            }
        }
    }
}
