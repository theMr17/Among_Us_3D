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
	//[SerializeField] private GameObject camera;
	
	private Vector2 fingerDown;
	private Vector2 fingerUp;
	public bool detectSwipeOnlyAfterRelease = false;

	public float SWIPE_THRESHOLD = 0f;

    // Start is called before the first frame update
    void Start()
    {
	    Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
	    Move();
	    transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
    }
    
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
