using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineUpManager : MonoBehaviour
{
    public GameObject headSet;
    public GameObject handR;
    public GameObject handL;

    LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //For creating line renderer object
        lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;   
    }

    // Update is called once per frame
    void Update()
    {
        var headPosition = headSet.transform.position;
        var leftPosition = handL.transform.position;
        float distance = Vector3.Distance(headPosition, leftPosition);
                        
        //For drawing line in the world space, provide the x,y,z values
        lineRenderer.SetPosition(0, headPosition); //x,y and z position of the starting point of the line
        lineRenderer.SetPosition(1, leftPosition); //x,y and z position of the end point of the line
    }
}
