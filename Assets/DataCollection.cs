using System;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.Networking;
using Oculus.Interaction.Grab;
using Oculus.Interaction.GrabAPI;
using Oculus.Interaction.Input;
using Oculus.Interaction.HandGrab;

namespace Oculus.Interaction.HandPosing
{
    public class DataCollection : MonoBehaviour
    {
        
        static DataCollection instance;

        [Header("Participant Index (e.g., P0)")]
        public string _participant = "P0"; // type in P0, P1,P2 ... to help you distinguish data from different users

        [Header("Enable Logging")] 
        public bool enable = true; // this script only collects data when enable is true
        [SerializeField] private HandGrabInteractor handGrab; // drag the dominant hand into this blank in the inspector
        
        [SerializeField] private GameObject cube; // drag the target cube into this blank in the inspector

        [SerializeField] private GameObject target;

        private bool isGrabbed = false; // if the object is grabbed this frame, isGrabbed is true
        private bool wasGrabbed = false; // if the object was grabbed last frame, wasGrabbed is true
        private bool isStart = false; // true when starting to grab
        private bool isEnd = false; // true when starting to release
        private float grabTime = Mathf.Infinity; // elapsed time of moving the cube from point A to point B
        private float grabDistance = Mathf.Infinity; // the distance between point A to point B
        private float grabSize = Mathf.Infinity; // the size of the cube
        private float initialPos; // the initial position of the cube
        Vector3 origin;
        private float initialTime; // the initial timestamp of user interaction (moving the cube from A to B)
        private float occupy_volume;
        private float occupy_percent;
        private float ob_volume;

        /*
        This template script only creates one cube. To investigate Fitts' Law, 
        we need to create many more cubes of various sizes, and move them to various distances.
        
        Please add variables here as per your need.  
        */

        private StreamWriter _writer; // to write data into a file
        private string _filename; // the name of the file

        // Ensure this script will run across scenes. Do not delete it.
        private void Awake()
        {
            
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else{
                instance = this;
                DontDestroyOnLoad(gameObject);
            }

        }

        // Save all data into the file when we quit the app
        private void OnApplicationQuit() {
            Debug.Log("On application quit");
            if (_writer != null) {
                _writer.Flush();
                _writer.Close();
                _writer.Dispose();
                _writer = null;
            }
        }
        
        // Save all data into the file when we pause the app
        private void OnApplicationPause(bool pauseStatus)
        {
            Debug.Log("On application pause");
            if (_writer != null) {
                _writer.Flush();
                _writer.Close();
                _writer.Dispose();
                _writer = null;
            }
        }
        
        private void Start()
        {
            // Create a csv file to save the data
            string filename = $"{_participant}-{Now}.csv";
            string path = Path.Combine(Application.persistentDataPath, filename); 
            // if you run it in the Unity Editor on Windows, the path is %userprofile%\AppData\LocalLow\<companyname>\<productname>
            // if you run it on Mac, the path is the ~/Library/Application Support/company name/product name
            // if you run it on a standalone VR headset, the path is Oculus/Android/data/<packagename>/files
            // reference here: https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html
            _writer = new StreamWriter(path);
            string msg = $"grabTime" +
                        $"grabSize" +
                        $"grabDistance";
            _writer.WriteLine(msg);
            Debug.Log(msg);
            _writer.Flush();
            origin = new Vector3(cube.transform.position.x, cube.transform.position.y, cube.transform.position.z);
        }

        void Update()
        {
            // only collect data when enable is true
            if (enable == false) return;
            
            // read the cube size
            grabSize = cube.transform.localScale.x;

            // read the grab status
            isGrabbed = (InteractorState.Select == handGrab.State);
            // print("isGrabbed: "+ isGrabbed);
            isStart = !wasGrabbed && isGrabbed;
            // print("isStart: "+ isStart);
            isEnd = wasGrabbed && !isGrabbed;
            // print("isEnd: "+ isEnd);
            ob_volume = grabSize * cube.transform.localScale.y * cube.transform.localScale.z;
            // start counting time and distance once a user grabs the cube
            if (isStart){
                initialPos = cube.transform.position.x;
                initialTime = Time.time;
            }
            // stop counting time and distance once a user releases the cube
            if (isEnd){
                float endPos = cube.transform.position.x;
                grabDistance = Mathf.Abs(endPos - initialPos);
                grabTime = Time.time - initialTime;
                occupy_volume = (cube.transform.localScale.x - Mathf.Abs(endPos - target.transform.position.x)) * target.transform.localScale.y * target.transform.localScale.z;

                occupy_percent = occupy_volume / ob_volume;
                if (occupy_percent>= 0.8)
                {
                    WriteToFile(grabTime, grabSize, grabDistance);
                    cube.transform.position = origin;
          
                }
                else
                {
                    WriteToFile(0, 0, 0);
                    cube.transform.position = origin;

                }
            }   
            
            wasGrabbed = isGrabbed;
            
        }
        
        // write T, W, D into the file.
        private void WriteToFile(float grabTime, float grabSize, float grabDistance) {
            if (_writer == null) return;
            
            string msg = $"{grabTime}," +
                        $"{grabSize}," +
                        $"{grabDistance}";
            _writer.WriteLine(msg);
            Debug.Log("test msg: "+msg);
            _writer.Flush();
        }
        
        // generate the current timestamp for filename
        private double Now {
            get {
                System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
                return (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
            }
        }
    
    }
}