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
        [SerializeField] private ActiveStateSelector lR;
        [SerializeField] private ActiveStateSelector lL;

        public GameObject lineDetector;
        public GameObject scoreBoard;

        private bool isThumbsUpL = false;
        private bool isThumbsUpR = false;
        private bool isThumbsDownL = false;
        private bool isThumbsDownR = false;
        private bool isRockR = false;
        private bool isRockL = false;
        private bool isLR = false;
        private bool isLL = false;

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
            lR.WhenSelected += () => { isLR = true; };
            lR.WhenUnselected += () => { isLR = false; };
            lL.WhenSelected += () => { isLL = true; };
            lL.WhenUnselected += () => { isLL = false; };

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
            // Turn on the line detection - use L-pose
            if (isLL || isLR)
            {
                lineDetector.SetActive(true);
                isLineOn = true;
                isThumbsUpBefore = false;
            }

            // Pull up score board and update score - use thumbs up
            if ((isThumbsUpL || isThumbsUpR) && !isThumbsUpBefore)
            {
                // Update score
                if(isThumbsUpL) {
                    leftScore += 1;
                }
                else if(isThumbsUpR) { 
                    rightScore += 1; 
                }

                scoreBoard.GetComponent<CanvasGroup>().alpha = 1.0f;

                string scoreText = "Scoreboard!\nLeft Player: " + leftScore + "\nRight Player: " + rightScore;

                scoreBoard.GetComponent<Text>().text = scoreText;

                isThumbsUpBefore = true;
            }

            // Stop detection and score board
            else if ((isRockR || isRockL) && !(isThumbsUpL || isThumbsUpR))
            {
                isLineOn = false;
                scoreBoard.GetComponent<CanvasGroup>().alpha = 0.0f;
                isThumbsUpBefore = false;
            }

            //make sure if stop is shown alongside thumbsup/thumbsdown
        }
    }
}