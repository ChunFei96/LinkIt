using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidationController : MonoBehaviour
{
    #region Singleton

    public static ValidationController Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public bool IsNextNodeValidated (LinkedList<string> linkedListGO, GameObject currentGO, GameObject nextGO)
    {
        bool isValid = false;

        if(linkedListGO != null && currentGO != null && nextGO != null)
        {
            string currentGOValue = currentGO.GetComponent<Node>().nodeModel.value;
            string nextGOValue = nextGO.GetComponent<Node>().nodeModel.value;

            LinkedListNode<string> currentNode = linkedListGO.Find(currentGOValue);

            if(currentNode.Next.Value == nextGOValue)
            {
                isValid = true;
                Debug.Log("CONNECT!!!");
            }
        }

        return isValid;
    }

    public bool IsNextNodeLast(LinkedList<string> linkedListGO, GameObject nextGO)
    {
        bool isValid = false;

        if (linkedListGO != null && nextGO != null)
        {
            string nextGOValue = nextGO.GetComponent<Node>().nodeModel.value;

            string lastGOValue = linkedListGO.Last.Value;

            if (lastGOValue == nextGOValue)
            {
                isValid = true;
                Debug.Log("END!!!");
            }
        }

        return isValid;
    }
}
