using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator animator;
    float velocityZ = 0.0f;
    float velocityX = 0.0f;

    public float WalkVelocity = 2f;

    public float acceleration = 2.0f;
    public float deceleration = 2.0f;

    //increase perfomance
    int VelocityZHash;
    int VelocityXHash;

    bool forwardPressed, leftPressed, rightPressed, backwardPressed, crouchPressed, runPressed;
    public bool canCrouch = false;

    // Start is called before the first frame update
    void Start()
    {
        //search the gameobject this script is attached to and get the animator component
        animator = GetComponent<Animator>();

        //increase performance
        VelocityZHash = Animator.StringToHash("VelocityZ");
        VelocityXHash = Animator.StringToHash("VelocityX");
    }

    // Update is called once per frame
    void Update()
    {

        UnityEngine.Debug.Log("X: " + velocityX);
        UnityEngine.Debug.Log("Z: " + velocityZ);

        forwardPressed = Input.GetKey(KeyCode.W);
        leftPressed = Input.GetKey(KeyCode.A);
        rightPressed = Input.GetKey(KeyCode.D);
        backwardPressed = Input.GetKey(KeyCode.S);

        if (canCrouch)
        {
            crouchPressed = Input.GetKeyDown(KeyCode.C);

            /*if(crouchPressed)
            {
                animator.SetBool("Crouch", animator.GetBool("Crouch") ? false : true);
            }*/
            if (Input.GetKey(KeyCode.LeftControl))
            {
                animator.SetBool("Crouch", true);
            }
            else
            {
                animator.SetBool("Crouch", false);
            }
        }

        if(backwardPressed)
        {
            forwardPressed = backwardPressed;
            bool temp = leftPressed;
            leftPressed = rightPressed;
            rightPressed = temp;
        }


        //set currentMaxVelocity
        float currentVelocity = WalkVelocity; //runPressed ? maximumRunVelocity : maximumWalkVelocity;

        changeVelocity(forwardPressed, leftPressed, rightPressed, backwardPressed, currentVelocity);
        //lockOrResetVelocity(forwardPressed, leftPressed, rightPressed, backwardPressed, runPressed, currentVelocity);

        if(!forwardPressed && !backwardPressed && !leftPressed && !rightPressed)
        {
            velocityX = 0f;
            velocityZ = 0f;
        }

        //set the parameters to our local variable values
        animator.SetFloat(VelocityZHash, velocityZ);
        animator.SetFloat(VelocityXHash, velocityX);
    }

    //handles acceleration and deceleration
    void changeVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool backwardPressed, float currentMaxVelocity)
    {
        //if player presses forward, increase velocity in z direction
        if (forwardPressed && velocityZ < currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        /*if (backwardPressed && velocityZ > -currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * acceleration;
        }*/

        //increase velocity in left direction
        if (leftPressed && velocityX > -currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
        }

        //increase velocity in right direction
        if (rightPressed && velocityX < currentMaxVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
        }


        //decrease velocityZ
        if (!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }

        //increase velocityZ
        if (!backwardPressed && velocityZ < 0.0f)
        {
            velocityZ += Time.deltaTime * deceleration;
        }

        //increase velocityX if left is not pressed and velocityX < 0
        if (!leftPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }

        //decrease velocityX if right is not pressed and velocityX > 0
        if (!rightPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * deceleration;
        }
    }

    //handles rest and locking of velocity
    void lockOrResetVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool backwardPressed, bool runPressed, float currentMaxVelocity)
    {
        //reset velcityZ
        if (!forwardPressed && velocityZ < 0.0f)    //(!backwardPressed && !forwardPressed && velocityZ != 0f && (velocityZ > -WalkVelocity && velocityZ < WalkVelocity))
        {
            velocityZ = 0f;
        }

        //reset velocityX
        if (!leftPressed && !rightPressed && velocityX != 0f && (velocityX > -WalkVelocity && velocityX < WalkVelocity))
        {
            velocityX = 0f;
        }




        //lock forward
        if (forwardPressed && runPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ = currentMaxVelocity;
        }
        //decelerate to the maximum walk velocity
        else if (forwardPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * deceleration;

            //round to the currentMaxVelocity if within offset
            if (velocityZ > currentMaxVelocity && velocityZ < (currentMaxVelocity + WalkVelocity))
            {
                velocityZ = currentMaxVelocity;
            }
        }
        //round to the currentMaxVelocity if within offset 
        else if (forwardPressed && velocityZ < currentMaxVelocity && velocityZ > (currentMaxVelocity - WalkVelocity))
        {
            velocityZ = currentMaxVelocity;
        }



        //lock left
        if (leftPressed && runPressed && velocityX < -currentMaxVelocity)
        {
            velocityX = -currentMaxVelocity;
        }
        //decelerate to the maximum walk velocity
        else if (leftPressed && velocityX < -currentMaxVelocity)
        {
            velocityX += Time.deltaTime * deceleration;

            //round to the currentMaxVelocity if within offset
            if (velocityX < -currentMaxVelocity && velocityX > (-currentMaxVelocity - WalkVelocity))
            {
                velocityX = -currentMaxVelocity;
            }
        }
        //round to the currentMaxVelocity if within offset 
        else if (leftPressed && velocityX > -currentMaxVelocity && velocityX < (-currentMaxVelocity + WalkVelocity))
        {
            velocityX = -currentMaxVelocity;
        }



        //lock right
        if (rightPressed && runPressed && velocityX > currentMaxVelocity)
        {
            velocityX = currentMaxVelocity;
        }
        //decelerate to the maximum walk velocity
        else if (rightPressed && velocityX > currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * deceleration;

            //round to the currentMaxVelocity if within offset
            if (velocityX > currentMaxVelocity && velocityX < (currentMaxVelocity + WalkVelocity))
            {
                velocityX = currentMaxVelocity;
            }
        }
        //round to the currentMaxVelocity if within offset 
        else if (rightPressed && velocityX < currentMaxVelocity && velocityX > (currentMaxVelocity - WalkVelocity))
        {
            velocityX = currentMaxVelocity;
        }


        /*
        //lock backward
        if (backwardPressed && runPressed && velocityZ < -currentMaxVelocity)
        {
            velocityZ = currentMaxVelocity;
        }
        //decelerate to the maximum walk velocity
        else if (backwardPressed && velocityZ < -currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * deceleration;

            //round to the currentMaxVelocity if within offset
            if (velocityZ < -currentMaxVelocity && velocityZ > (-currentMaxVelocity - WalkVelocity))
            {
                velocityZ = currentMaxVelocity;
            }
        }
        //round to the currentMaxVelocity if within offset 
        else if (backwardPressed && velocityZ > -currentMaxVelocity && velocityZ < (-currentMaxVelocity + WalkVelocity))
        {
            velocityZ = currentMaxVelocity;
        }*/
    }
}
