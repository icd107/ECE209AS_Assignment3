using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Oculus.Interaction.Samples
{
    public class GestureDetection1 : MonoBehaviour
    {
        [SerializeField] private ActiveStateSelector thumbsUpL;
        [SerializeField] private ActiveStateSelector thumbsUpR;
        [SerializeField] private ActiveStateSelector thumbsDownL;
        [SerializeField] private ActiveStateSelector thumbsDownR;
        [SerializeField] private ActiveStateSelector rockR;
        [SerializeField] private ActiveStateSelector rockL;

        public GameObject lineDetector;
        public GameObject scoreBoard;

        private bool isThumbsUpL = false;
        private bool isThumbsUpR = false;
        private bool isThumbsDownL = false;
        private bool isThumbsDownR = false;
        private bool isRockR = false;
        private bool isRockL = false;

        private bool isThumbsUpBefore = false;

        public bool isLineOn;
        private int leftScore;
        private int rightScore;

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
            rockR.WhenSelected += () => { isRockR = true; };
            rockR.WhenUnselected += () => { isRockR = false; };
            rockL.WhenSelected += () => { isRockL = true; };
            rockL.WhenUnselected += () => { isRockL = false; };

            scoreBoard.GetComponent<CanvasGroup>().alpha = 0.0f;
            isLineOn = false;
            leftScore = 0;
            rightScore = 0;

            string scoreText = "Scoreboard!\nPlayer 1: " + leftScore + "\nPlayer 2: " + rightScore;
            scoreBoard.GetComponent<Text>().text = scoreText;
        }

        // Update is called once per frame
        void Update()
        {
            // Turn on the detection
            if (isThumbsDownL || isThumbsDownR)
            {
                lineDetector.SetActive(true);
                isLineOn = true;
                isThumbsUpBefore = false;
            }

            // Pull up score board and update score
            if ((isThumbsUpL || isThumbsUpR) && !isThumbsUpBefore)
            {
                scoreBoard.GetComponent<CanvasGroup>().alpha = 1.0f;

                // Update score
                if(isThumbsUpL) {
                    leftScore += 1;
                }
                else if(isThumbsUpR) { 
                    rightScore += 1; 
                }

                string scoreText = "Scoreboard!\nLeft Player: " + leftScore + "\nRight Player: " + rightScore;

                scoreBoard.GetComponent<Text>().text = scoreText;

                isThumbsUpBefore = true;
            }

            // Stop detection and score board
            if (isRockR || isRockL)
            {
                isLineOn = false;
                scoreBoard.GetComponent<CanvasGroup>().alpha = 0.0f;
                isThumbsUpBefore = false;
            }

            //make sure if stop is shown alongside thumbsup/thumbsdown
        }
    }
}