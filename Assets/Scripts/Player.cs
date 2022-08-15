using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private Rigidbody rb;
	[SerializeField] private CharacterController characterController;
	[SerializeField] private float speed = 2f;
	[SerializeField] private float turnSmoothTime = 0.1f;
	[SerializeField] private float turnSmoothVelocity;
	[SerializeField] private  Transform camTransform;
	[SerializeField] private  Transform mainRigTransform;

    // Start is called before the first frame update
    void Start()
    {
	    
    }

    // Update is called once per frame
    void Update()
    {
        //bool forwardPressed, leftPressed, rightPressed, backwardPressed;

        //forwardPressed = Input.GetKey(KeyCode.W);
        //leftPressed = Input.GetKey(KeyCode.A);
        //rightPressed = Input.GetKey(KeyCode.D);
        //backwardPressed = Input.GetKey(KeyCode.S);

        Move();
    }


    ////Movement
    //void Move(bool forwardPressed, bool leftPressed, bool rightPressed, bool backwardPressed)
    //{
    //    if (forwardPressed)
	//        rb.velocity = transform.TransformDirection(new Vector3(0f, 0f, Time.deltaTime * 50));
    //    else if(backwardPressed)
	//        rb.velocity = transform.TransformDirection(new Vector3(0f, 0f, Time.deltaTime * -50));
    //    else
	//        rb.velocity = new Vector3(0f, 0f, 0f);
	        
	//    if (leftPressed)
	//	    rb.velocity = transform.TransformDirection(new Vector3(Time.deltaTime * -50, 0f, 0f));
	//    else if(rightPressed)
	//	    rb.velocity = transform.TransformDirection(new Vector3(Time.deltaTime * 50, 0f, 0f));
	//}
    
	private void Move()
	{
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");
		Vector3 direction = transform.TransformDirection(new Vector3(horizontal, 0f, vertical).normalized);
		
		if(direction.magnitude >= 0.1f)
		{
			float targetAngle = camTransform.eulerAngles.y;
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
			transform.rotation = Quaternion.Euler(0f, angle, 0f);
			
			characterController.Move(direction * speed * Time.deltaTime);
		}
	}
}
