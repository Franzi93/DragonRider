using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Windows.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;

// Adapted from DiscreteGestureBasics-WPF by Momo the Monster 2014-11-25
// For Helios Interactive - http://heliosinteractive.com

public class GestureSourceManager : MonoBehaviour
{

    public struct EventArgs
    {
        public string name;
        public float confidence;

        public EventArgs(string _name, float _confidence)
        {
            name = _name;
            confidence = _confidence;
        }
    }

    public BodySourceManager _BodySource;
    public string databasePath;
    private KinectSensor _Sensor;
    private VisualGestureBuilderFrameSource _Source;
    private VisualGestureBuilderFrameReader _Reader;
    private VisualGestureBuilderDatabase _Database;

    // Gesture Detection Events
    public delegate void GestureAction(EventArgs e);
    public event GestureAction OnGesture;

    private bool fasterhappened;
    private bool slowerhappened;

    // Use this for initialization
    void Start()
    {

        fasterhappened = false;
        slowerhappened = false;


        _Sensor = KinectSensor.GetDefault();
        if(_Sensor != null)
        {

            if(!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }

            // Set up Gesture Source
            _Source = VisualGestureBuilderFrameSource.Create(_Sensor, 0);

            // open the reader for the vgb frames
            _Reader = _Source.OpenReader();
            if(_Reader != null)
            {
                _Reader.IsPaused = true;
                _Reader.FrameArrived += GestureFrameArrived;
            }

            // load the 'Seated' gesture from the gesture database
            string path = System.IO.Path.Combine(Application.streamingAssetsPath, databasePath);
            // TODO path irgendwann nicht mehr hardcoden
            _Database = VisualGestureBuilderDatabase.Create("Assets/Streaming Assets/gestures.gbd");

            // Load all gestures
            IList<Gesture> gesturesList = _Database.AvailableGestures;
            for(int g = 0; g < gesturesList.Count; g++)
            {
                Gesture gesture = gesturesList[g];
                _Source.AddGesture(gesture);
            }

        }
    }

    // Public setter for Body ID to track
    public void SetBody(ulong id)
    {
        if(id > 0)
        {
            _Source.TrackingId = id;
            _Reader.IsPaused = false;
        }
        else
        {
            _Source.TrackingId = 0;
            _Reader.IsPaused = true;
        }
    }

    // Update Loop, set body if we need one
    void Update()
    {
        if(!_Source.IsTrackingIdValid)
        {
            FindValidBody();
        }
    }

    // Check Body Manager, grab first valid body
    void FindValidBody()
    {

        if(_BodySource != null)
        {



            Body[] bodies = _BodySource.GetData();
            if(bodies != null)
            {

                foreach(Body body in bodies)
                {
                    if(body.IsTracked)
                    {

                        SetBody(body.TrackingId);
                        break;
                    }
                }
            }
        }

    }

    /// Handles gesture detection results arriving from the sensor for the associated body tracking Id
    private void GestureFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
    {

        VisualGestureBuilderFrameReference frameReference = e.FrameReference;
        using(VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
        {
            if(frame != null)
            {
                // get the discrete gesture results which arrived with the latest frame
                IDictionary<Gesture, DiscreteGestureResult> discreteResults = frame.DiscreteGestureResults;
                IDictionary<Gesture, ContinuousGestureResult> continuousResults = frame.ContinuousGestureResults;

                if(discreteResults != null)
                {
                    foreach(Gesture gesture in _Source.Gestures)
                    {
                        if(gesture.GestureType == GestureType.Discrete)
                        {


                            DiscreteGestureResult result = null;
                            discreteResults.TryGetValue(gesture, out result);

                            if(result != null)
                            {

                               // Debug.Log("Detected Gesture " + gesture.Name + " with Confidence " + result.Confidence);


                                if(gesture.Name == "Faster" && result.Confidence >= 0.5f && !fasterhappened)
                                {
                                    //TODO potenziell Namen vom Script ändern
                                    Debug.Log("faster");
                                    gameObject.GetComponentInChildren<CubeScript>().moveForward();
                                    fasterhappened = true;

                                }
                                fasterhappened = false;

                                if(gesture.Name == "Slower" && result.Confidence >= 0.5f && !slowerhappened)
                                {
                                    Debug.Log("slower");
                                    gameObject.GetComponentInChildren<CubeScript>().moveBackward();
                                    slowerhappened = true;
                                }
                                slowerhappened = false;

                            }
                        }
                        else if(gesture.GestureType == GestureType.Continuous)
                        {
                            ContinuousGestureResult result = null;
                            continuousResults.TryGetValue(gesture, out result);

                            if(result != null)
                            {
                                Debug.Log("Detected Gesture " + gesture.Name + " with Progress " + result.Progress);

                                if(gesture.Name == "LeanForwardBackward"
                                    && (result.Progress >= 0.2f || result.Progress <= -0.2f)
                                    )
                                {
                                    gameObject.GetComponentInChildren<CubeScript>().moveY(result.Progress);
                                }
                                if(gesture.Name == "LeanLeftRightProgress"
                                    // && (result.Progress >= 0.2f || result.Progress <= -0.2f)
                                    )
                                {
                                    gameObject.GetComponentInChildren<CubeScript>().moveX(result.Progress);
                                }
                                if(gesture.Name == "DrehungContinouos" 
                                  //  && (result.Progress >= 0.1f)
                                    )
                                {
                                    Debug.Log("gesture.Name == DrehungContinouos");
                                    gameObject.GetComponentInChildren<CubeScript>().rotate(result.Progress);
                                }

                            }
                        }
                    }
                }
            }
        }
    }

}