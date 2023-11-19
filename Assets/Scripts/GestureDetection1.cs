using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oculus.Interaction.Samples
{
    public class GestureDetection1 : MonoBehaviour
    {
        [SerializeField] private ActiveStateSelector thumbsUpL;
        [SerializeField] private ActiveStateSelector thumbsUpR;
        [SerializeField] private ActiveStateSelector thumbsDownL;
        [SerializeField] private ActiveStateSelector thumbsDownR;
        [SerializeField] private ActiveStateSelector stopL;
        [SerializeField] private ActiveStateSelector stopR;
        [SerializeField] private ActiveStateSelector scissor;

        public GameObject lineDetector;
        public GameObject scoreBoard;

        private bool isThumbsUpL = false;
        private bool isThumbsUpR = false;
        private bool isThumbsDownL = false;
        private bool isThumbsDownR = false;
        private bool isStopL = false;
        private bool isStopR = false;
        private bool isScissor = false;

        // Start is called before the first frame update
        void Start()
        {
            thumbsUpL.WhenSelected += () => { isThumbsUpL = true; };
            thumbsUpL.WhenUnselected += () => { isThumbsUpL = false; };
            thumbsUpR.WhenSelected += () => { isThumbsUpR = true; };
            thumbsUpR.WhenUnselected += () => { isThumbsUpR = false; };
            thumbsDownL.WhenSelected += () => { isThumbsDownL = true; };
            thumbsDownL.WhenUnselected += () => { isThumbsDownL = false; };
            thumbsDownR.WhenSelected += () => { isThumbsDownR = true; };
            thumbsDownR.WhenUnselected += () => { isThumbsDownR = false; };
            stopL.WhenSelected += () => { isStopL = true; };
            stopL.WhenUnselected += () => { isStopL = false; };
            stopR.WhenSelected += () => { isStopR = true; };
            stopR.WhenUnselected += () => { isStopR = false; };
            scissor.WhenSelected += () => { isScissor = true; };
            scissor.WhenUnselected += () => { isScissor = false; };

        }

        // Update is called once per frame
        void Update()
        {
            // Turn on the detection
            if (isThumbsUpL || isThumbsUpR)
            {
                lineDetector.SetActive(true);
            }

            // Pull up score board
            if (isThumbsDownL || isThumbsDownR)
            {
                scoreBoard.SetActive(true);
            }

            // Stop detection and score board
            //if (isStopL || isStopR)
            if (isScissor)
            {
                if (lineDetector) { lineDetector.SetActive(false); }
                if (scoreBoard) { scoreBoard.SetActive(false); }

            }
        }
    }
}