using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oculus.Interaction.Samples
{
    public class LineUpManager : MonoBehaviour
    {
        public GameObject headSet;
        public GameObject handR;
        public GameObject handL;
        public GameObject gestureDetector;

        Vector3 headPosition;
        Vector3 leftPosition;
        Vector3 rightPosition;

        LineRenderer lineRenderer;

        public bool isLeftFurther;

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
            updatePositions();
            isLeftFurther = false;
        }

        // Update is called once per frame
        void Update()
        {
            updatePositions();
            
            float distanceL = Vector3.Distance(headPosition, leftPosition);
            float distanceR = Vector3.Distance(headPosition, rightPosition);

            Vector3 furtherTarget = new Vector3();

            if (distanceL > distanceR) { 
                furtherTarget = leftPosition;
                isLeftFurther = true;
            }
            else { 
                furtherTarget = rightPosition; 
                isLeftFurther = false; 
            }

            if(gestureDetector.GetComponent<GestureDetection1>().isLineOn)
            {
                lineRenderer.SetPosition(0, headPosition);
                lineRenderer.SetPosition(1, furtherTarget);
            }
            else
            {
                //For drawing line in the world space, provide the x,y,z values
                lineRenderer.SetPosition(0, headPosition); //x,y and z position of the starting point of the line
                lineRenderer.SetPosition(1, headPosition); //x,y and z position of the end point of the line
            }
        }

        void updatePositions()
        {
            headPosition = headSet.transform.position;

            if (handL.transform.position != null && handL.transform.position != new Vector3(0,0,0))
            { 
                leftPosition = handL.transform.position;
            }
            if (handR.transform.position != null && handR.transform.position != new Vector3(0,0,0))
            {
                rightPosition = handR.transform.position;
            } 
        }
    }
}