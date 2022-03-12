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

    [System.Serializable]
    public class TMTARuleSelectors
    {
        public string instructionText;
    }

    [System.Serializable]
    public class TMTBRuleSelectors
    {
        public string instructionText;
    }

    public TMTARuleSelectors TMTARuleSelector;

    public TMTBRuleSelectors TMTBRuleSelector;

    private List<char> alphaList = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().ToList();

    private List<int> numList = Enumerable.Range(1, 25).ToList();

    public void TMTA()
    {
        for (int i = 0; i < GameController.Instance.activeGOs.Count; i++)
        {
            GameObject objectToSpawn = GameController.Instance.activeGOs[i];

            // apply img texture on GO
            int fileName = numList[i];
            string filePath = Environment.CurrentDirectory + "/Assets/Resources/" + fileName + ".png";
            objectToSpawn.GetComponent<SpriteRenderer>().sprite = SpriteController.instance.LoadNewSprite(filePath);

            // apply visibility
            objectToSpawn.SetActive(true);

            // assign position to the GO without overlapping others
            objectToSpawn.transform.position = GameController.Instance.CalculateNodePos();

            // assign the numeric value to the GO
            objectToSpawn.GetComponent<Node>().nodeModel.value = fileName.ToString();

            // LinkedList for validation purpose
            GameController.Instance.linkedListGO.AddLast(fileName.ToString());

            // default select the first node
            if (i == 0)
            {
                objectToSpawn.GetComponent<SpriteRenderer>().color = Color.green;
                objectToSpawn.GetComponent<Node>().nodeModel.isConnect = true;
                GameController.Instance.selectedGOInstance.Enqueue(objectToSpawn);
                GameController.Instance.TestTypeText.text = "TMTA";
                GameController.Instance.InstructionText.text = TMTARuleSelector.instructionText;
            }
        }
    }

    public void TMTB()
    {
        int numCounter = 0;
        int charCounter = 0;
        for (int i = 0; i < GameController.Instance.activeGOs.Count; i++)
        {
            GameObject objectToSpawn = GameController.Instance.activeGOs[i];

            // apply img texture on GO
            string fileName=string.Empty;
            

            if ((i + 1) % 2 == 1) // 0, 2, 4
            {
                fileName = numList[numCounter].ToString();
                numCounter += 1;
            } 
            else if ((i + 1) % 2 == 0) // 1, 3, 5
            {
                fileName = alphaList[charCounter].ToString();
                charCounter += 1;
            }
                


            string filePath = Environment.CurrentDirectory + "/Assets/Resources/" + fileName + ".png";
            objectToSpawn.GetComponent<SpriteRenderer>().sprite = SpriteController.instance.LoadNewSprite(filePath);

            // apply visibility
            objectToSpawn.SetActive(true);

            // assign position to the GO without overlapping others
            objectToSpawn.transform.position = GameController.Instance.CalculateNodePos();

            // assign the numeric value to the GO
            objectToSpawn.GetComponent<Node>().nodeModel.value = fileName;

            // LinkedList for validation purpose
            GameController.Instance.linkedListGO.AddLast(fileName);

            // default select the first node
            if (i == 0)
            {
                objectToSpawn.GetComponent<SpriteRenderer>().color = Color.green;
                objectToSpawn.GetComponent<Node>().nodeModel.isConnect = true;
                GameController.Instance.selectedGOInstance.Enqueue(objectToSpawn);                
                GameController.Instance.TestTypeText.text = "TMTB";
                GameController.Instance.InstructionText.text = TMTBRuleSelector.instructionText;
            }
        }
    }
}
