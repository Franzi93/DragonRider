using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class CubeScript : MonoBehaviour
{

    private KinectSensor _Sensor;
    private BodyFrameReader _Reader;
    private Body[] _Data = null;

    bool initialized;

    Vector3 initialspinebase;
    Vector3 initialspineshoulder;
    Vector3 initialelbowright;
    Vector3 initialelbowleft;
    Vector3 initialshoulderright;
    Vector3 initialshoulderleft;

    Vector3 spinebase;
    Vector3 spineshoulder;
    Vector3 elbowleft;
    Vector3 elbowright;
    Vector3 shoulderright;
    Vector3 shoulderleft;

    void Start()
    {
        initialized = false;
        _Sensor = KinectSensor.GetDefault();

        if(_Sensor != null)
        {
            _Reader = _Sensor.BodyFrameSource.OpenReader();

            if(!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
        }

    }

    void OnApplicationQuit()
    {
        if(_Reader != null)
        {
            _Reader.Dispose();
            _Reader = null;
        }

        if(_Sensor != null)
        {
            if(_Sensor.IsOpen)
            {
                _Sensor.Close();
            }
            _Sensor = null;
        }
    }

    void Update()
    {

        
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("reset");
            this.gameObject.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            this.gameObject.transform.Rotate(Vector3.down, 1.0f * 30);
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            this.gameObject.transform.Rotate(Vector3.up, 1.0f * 30);
        }



        if(_Reader != null)
        {


            var frame = _Reader.AcquireLatestFrame();

            if(frame != null)
            {
                if(_Data == null)
                {
                    _Data = new Body[_Sensor.BodyFrameSource.BodyCount];
                }

                frame.GetAndRefreshBodyData(_Data);

                frame.Dispose();
                frame = null;

                int idx = -1;
                for(int i = 0; i < _Sensor.BodyFrameSource.BodyCount; i++)
                {
                    if(_Data[i].IsTracked)
                    {
                        idx = i;
                    }
                }
                if(idx > -1)
                {

                    if(Input.GetKeyDown(KeyCode.Space))
                    {
                        Debug.Log("initialized");
                        init(idx);
                    }


                    if(initialized)
                    {

                        spinebase = new Vector3(_Data[idx].Joints[JointType.SpineBase].Position.X,
                            _Data[idx].Joints[JointType.SpineBase].Position.Y,
                            _Data[idx].Joints[JointType.SpineBase].Position.Z);

                        spineshoulder = new Vector3(_Data[idx].Joints[JointType.SpineShoulder].Position.X,
                            _Data[idx].Joints[JointType.SpineShoulder].Position.Y,
                            _Data[idx].Joints[JointType.SpineShoulder].Position.Z);

                        elbowleft = new Vector3(_Data[idx].Joints[JointType.ElbowLeft].Position.X,
                         _Data[idx].Joints[JointType.ElbowLeft].Position.Y,
                         _Data[idx].Joints[JointType.ElbowLeft].Position.Z);

                        elbowright = new Vector3(_Data[idx].Joints[JointType.ElbowRight].Position.X,
                            _Data[idx].Joints[JointType.ElbowRight].Position.Y,
                            _Data[idx].Joints[JointType.ElbowRight].Position.Z);

                        shoulderright = new Vector3(_Data[idx].Joints[JointType.ShoulderRight].Position.X,
                            _Data[idx].Joints[JointType.ShoulderRight].Position.Y,
                            _Data[idx].Joints[JointType.ShoulderRight].Position.Z);

                        shoulderleft = new Vector3(_Data[idx].Joints[JointType.ShoulderLeft].Position.X,
                        _Data[idx].Joints[JointType.ShoulderLeft].Position.Y,
                        _Data[idx].Joints[JointType.ShoulderLeft].Position.Z);

                        //if (leanBackward())
                        //{
                        //    moveDown();
                        //}

                        //if (leanForward())
                        //{
                        //    moveUp();
                        //}


                        //if (leanLeft())
                        //{
                        //    moveLeft();
                        //}

                        //if (leanRight())
                        //{
                        //    moveRight();
                        //}

                        //if (pull())
                        //{
                        //    moveBackward();
                        //}

                        //if (shakeHands())
                        //{
                        //    moveForward();
                        //}

                    }



                }

            }
        }

    }

    public bool leanForward()
    {
        return (spinebase.z - spineshoulder.z < initialspinebase.z - initialspineshoulder.z - 0.1f);
    }

	public bool leanBackward()
    {
        return (spinebase.z - spineshoulder.z > initialspinebase.z - initialspineshoulder.z + 0.1f);
    }

	public bool leanLeft()
    {
        return (spinebase.x - spineshoulder.x > initialspinebase.x - initialspineshoulder.x + 0.1f);
    }

	public bool leanRight()
    {
        return (spinebase.x - spineshoulder.x < initialspinebase.x - initialspineshoulder.x - 0.1f);
    }

	public bool pull()
    {
        return (elbowleft.z > initialelbowleft.z + 0.1f);

    }

	public bool rotateLeft()
    {
        Debug.Log("rotateLeft");
        return (shoulderright.z > shoulderleft.z + 0.1f);
    }


	public bool rotateRight()
    {
        Debug.Log("rotateRight");
        return (shoulderright.z < shoulderleft.z - 0.1f);
    }
	public bool shakeHands()
    {
        return Input.GetKeyDown(KeyCode.S);
    }

    void init(int idx)
    {
        initialspinebase = new Vector3(_Data[idx].Joints[JointType.SpineBase].Position.X,
                        _Data[idx].Joints[JointType.SpineBase].Position.Y,
                        _Data[idx].Joints[JointType.SpineBase].Position.Z);

        initialspineshoulder = new Vector3(_Data[idx].Joints[JointType.SpineShoulder].Position.X,
            _Data[idx].Joints[JointType.SpineShoulder].Position.Y,
            _Data[idx].Joints[JointType.SpineShoulder].Position.Z);

        initialelbowleft = new Vector3(_Data[idx].Joints[JointType.ElbowLeft].Position.X,
            _Data[idx].Joints[JointType.ElbowLeft].Position.Y,
            _Data[idx].Joints[JointType.ElbowLeft].Position.Z);

        initialelbowright = new Vector3(_Data[idx].Joints[JointType.ElbowRight].Position.X,
            _Data[idx].Joints[JointType.ElbowRight].Position.Y,
            _Data[idx].Joints[JointType.ElbowRight].Position.Z);

        initialshoulderright = new Vector3(_Data[idx].Joints[JointType.ShoulderRight].Position.X,
            _Data[idx].Joints[JointType.ShoulderRight].Position.Y,
            _Data[idx].Joints[JointType.ShoulderRight].Position.Z);

        initialshoulderleft = new Vector3(_Data[idx].Joints[JointType.ShoulderLeft].Position.X,
        _Data[idx].Joints[JointType.ShoulderLeft].Position.Y,
        _Data[idx].Joints[JointType.ShoulderLeft].Position.Z);

        initialized = true;

    }

    void moveUp()
    {
        this.gameObject.transform.position = new Vector3(
                            this.gameObject.transform.position.x,
                            this.gameObject.transform.position.y + 0.5f,
                            this.gameObject.transform.position.z);
    }

    void moveDown()
    {

        this.gameObject.transform.position = new Vector3(
                           this.gameObject.transform.position.x,
                           this.gameObject.transform.position.y - 0.5f,
                           this.gameObject.transform.position.z);

    }

    public void moveY(float val)
    {
        if(val > 0.1f || val < -0.1f)
        {
            this.gameObject.transform.position = new Vector3(
                               this.gameObject.transform.position.x,
                               this.gameObject.transform.position.y + 0.5f * val,
                               this.gameObject.transform.position.z);
        }
    }

    void moveRight()
    {
        this.gameObject.transform.position = new Vector3(
                               this.gameObject.transform.position.x + 0.5f,
                               this.gameObject.transform.position.y,
                               this.gameObject.transform.position.z);
    }

    public void moveLeft()
    {
        this.gameObject.transform.position = new Vector3(
                               this.gameObject.transform.position.x - 0.5f,
                               this.gameObject.transform.position.y,
                               this.gameObject.transform.position.z);
    }

    public void moveX(float val)
    {
        val = System.Math.Abs(val);

        if(leanLeft())
        {
            this.gameObject.transform.position = new Vector3(
                               this.gameObject.transform.position.x - 0.5f * val,
                               this.gameObject.transform.position.y,
                               this.gameObject.transform.position.z);
        }
        if(leanRight())
        {
            this.gameObject.transform.position = new Vector3(
                               this.gameObject.transform.position.x + 0.5f * val,
                               this.gameObject.transform.position.y,
                               this.gameObject.transform.position.z);
        }
    }

    public void moveForward()
    {
        this.gameObject.transform.position = new Vector3(
                               this.gameObject.transform.position.x,
                               this.gameObject.transform.position.y,
                               this.gameObject.transform.position.z + 1.5f);

    }

    public void moveBackward()
    {

        this.gameObject.transform.position = new Vector3(
                           this.gameObject.transform.position.x,
                           this.gameObject.transform.position.y,
                           this.gameObject.transform.position.z - 1.5f);

    }

    public void rotate(float val)
    {
        Debug.Log("rotate");
        val = System.Math.Abs(val);

        if(rotateLeft())
        {
            this.gameObject.transform.Rotate(Vector3.up, val * 5);
        }
        if(rotateRight())
        {
            this.gameObject.transform.Rotate(Vector3.down, val * 5);
        }
    }

}