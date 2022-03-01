using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public NodeModel nodeModel = new NodeModel();

    private void OnMouseDown()
    {
        //TODO: move the logic to GameController
        //GameController.Instance.allGOInstance.Add(this.gameObject);

        if (this.gameObject.GetComponent<Node>().nodeModel.isConnect)
            return;

        GameController.Instance.NodeOnMouseDown(this.gameObject);

        //GameController.Instance.selectedGOInstance.Enqueue(this.gameObject);

        //if(GameController.Instance.selectedGOInstance.Count == 2)
        //{
        //    GameObject firstGO = GameController.Instance.selectedGOInstance.Dequeue();
        //    GameObject lastGO = GameController.Instance.selectedGOInstance.Dequeue();


        //    if (ValidationController.Instance.IsNextNodeValidated(ObjectPooler.Instance.linkedListGO, firstGO, lastGO))
        //    {
        //        LineRenderer line = new GameObject("Line").AddComponent<LineRenderer>();
        //        line.startColor = Color.black;
        //        line.endColor = Color.black;
        //        line.startWidth = 0.1f;
        //        line.endWidth = 0.1f;

        //        firstGO.GetComponent<Node>().nodeModel.isConnect = true;
        //        lastGO.GetComponent<Node>().nodeModel.isConnect = true;

        //        var firstPosition = firstGO.transform.position;
        //        var secondPosition = lastGO.transform.position;

        //        Vector3[] pathPoints = { firstPosition, secondPosition };
        //        line.positionCount = 2;
        //        line.SetPositions(pathPoints);

        //        GameController.Instance.selectedGOInstance.Enqueue(this.gameObject);

        //        // reset all GO to color white
        //        foreach (var go in GameController.Instance.allGOInstance)
        //            go.GetComponent<SpriteRenderer>().color = Color.white;

        //        GetComponent<SpriteRenderer>().color = Color.green;
        //    }
        //    else
        //    {
        //        GameController.Instance.selectedGOInstance.Enqueue(firstGO);
        //        //GameController.Instance.selectedGOInstance.Enqueue(lastGO);

        //        GetComponent<SpriteRenderer>().color = Color.red;
        //    }
        //}
        //else
        //{
        //    // reset all GO to color white
        //    foreach (var go in GameController.Instance.allGOInstance)
        //        go.GetComponent<SpriteRenderer>().color = Color.white;

        //    GetComponent<SpriteRenderer>().color = Color.green;
        //}

    }
}
