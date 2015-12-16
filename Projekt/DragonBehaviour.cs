using UnityEngine;
using System.Collections;

public class DragonBehaviour : MonoBehaviour {
    
    public float speed = 10;
    public float maxSpeed = 30;
    float vertical = 0f;
    float horizontal = 0f;
    float tilt;
	public float acceleration = 50f;
	private float rotateHSpeed ;
	private float rotateVSpeed ;
	public float rotateHStart = 0.01f;
	public float rotateVStart = 0.01f;
	 CubeScript bodyMovement;

	void Start ()
    {
		bodyMovement = GetComponent<CubeScript> ();
	}
	
    void Update()
    {
		GetInputSpeed();
        GetInputVertical();
        GetInputHorizontal();
		rotateHSpeed = rotateHStart + ( speed * 0.0001f);
		rotateVSpeed = rotateVStart + ( speed * 0.0001f);
    }

	void FixedUpdate ()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        
        transform.eulerAngles = (new Vector3(-vertical * 90, -horizontal * 90, tilt * 90));
	}

    void GetInputSpeed()
    {
        if (Input.GetKeyDown(KeyCode.W) || bodyMovement.shakeHands())
        {
            speed += acceleration;
        }
        if (Input.GetKey(KeyCode.S) || bodyMovement.pull())
        {
         //   speed -= acceleration/15;
        }
        speed = Mathf.Clamp(speed, 0f, maxSpeed);
    }

    void GetInputVertical()
    {
        if (Input.GetKey(KeyCode.DownArrow) || bodyMovement.leanForward())
        {
            vertical += rotateVSpeed;
        }
		else if (Input.GetKey(KeyCode.UpArrow) || bodyMovement.leanBackward())
        {
            vertical -= rotateVSpeed;
        }
        else
        {
			if(vertical > .1) { vertical -= rotateVSpeed; }
			else if (vertical < 0) { vertical += rotateVSpeed; }
        }
        vertical = Mathf.Clamp(vertical, -1f, 1f);
    }

    void GetInputHorizontal()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || bodyMovement.leanLeft())
        {
			horizontal += rotateHSpeed;
			tilt+= rotateHSpeed;
        }
		else if (Input.GetKey(KeyCode.RightArrow) || bodyMovement.leanRight())
        {
			horizontal -= rotateHSpeed;
			tilt -= rotateHSpeed;
        }
        else 
		{
			if(tilt > .1) { tilt -= rotateHSpeed; }
			else if (tilt < 0) { tilt += rotateHSpeed; }
        }
        tilt = Mathf.Clamp(tilt, -1f, 1f);
    }
}
