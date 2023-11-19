using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oculus.Interaction.Samples
{
    public class GestureTest : MonoBehaviour
    {

        [SerializeField] private ActiveStateSelector thumbsUp;

        [SerializeField] private GameObject cube;

        private bool isThumbsup = false;

        // Start is called before the first frame update
        void Start()
        {
            thumbsUp.WhenSelected += () => { isThumbsup = true; };
            thumbsUp.WhenUnselected += () => { isThumbsup = false; };
        }

        // Update is called once per frame
        void Update()
        {
            if (isThumbsup) { ChangeColor(cube); }
        }

        private void ChangeColor(GameObject cube)
        {
            var cubeRenderer = cube.GetComponent<Renderer>();
            Color newColor = new Color(Random.value, Random.value, Random.value, 1.0f);
            cubeRenderer.material.SetColor("_Color", newColor);
        }
    }
}