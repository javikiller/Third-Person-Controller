using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]

public class CharacterAnimBasedMovement : MonoBehaviour {

    public string turn180Param = "turn180";
    public bool turn180;
    public float degreesToTurn;

    public string mirrorIdleParam = "mirrorIdle";
    public bool mirrorIdle;
    public bool stairs = false;
    public bool contAux = false;
    public bool contPressAux = false;
    public bool buttonPressed = false;
    public bool Button = false;    
    public float jumpAux = 0;
    public float pressAux = 0;

    public float rotationSpeed = 4f;
    public float rotationThreshold = 0.3f;

    public GameObject obstacle1;
    public GameObject obstacle2;
    public GameObject obstacle3;

    [Header("Animator Parameters")]
    public string motionParam = "motion";
    public string stairsParam = "upStairs";
    public string jumpParam = "jump";
    public string buttonParam = "buttonPress";

    [Header("Animation Smoothing")]
    [Range(0, 1f)]
    public float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    public float StopAnimTime = 0.15f;

    private float Speed;
    private Vector3 desiredMoveDirection;
    private CharacterController characterController;
    private Animator animator;

    void Start ()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        obstacle1.SetActive(false);
        obstacle2.SetActive(false);
        obstacle3.SetActive(false);
    }
    private void Update()
    {
        
    }

    public void MoveCharacter(float hInput, float vInput, Camera cam, bool jump, bool dash, bool press)
    {
        Speed = new Vector2(hInput, vInput).normalized.sqrMagnitude;

     

        if(Speed >= Speed - rotationThreshold && dash)
        {
            Speed = 1.5f;

        }

        if(Speed > rotationThreshold)
        {
            animator.SetFloat(motionParam, Speed, StartAnimTime, Time.deltaTime);
            Vector3 forward = cam.transform.forward;
            Vector3 right = cam.transform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed * Time.deltaTime);
            desiredMoveDirection = forward * vInput + right * hInput;
            

            if (Vector3.Angle(transform.forward, desiredMoveDirection) >= degreesToTurn)
            {
                turn180 = true;
            }
            else
            {
                turn180 = false;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed * Time.deltaTime);
            }
            animator.SetBool(turn180Param, turn180);
            animator.SetFloat(motionParam, Speed, StartAnimTime, Time.deltaTime);
        }
        else if(Speed < rotationThreshold)
        {
            animator.SetBool(mirrorIdleParam, mirrorIdle);
            animator.SetFloat(motionParam, Speed, StopAnimTime, Time.deltaTime);
        }

        if (jump)
        {
            animator.SetBool(jumpParam, true);
            contAux = true;            
        }
        if (contAux)
        {
            jumpAux += 10 * Time.deltaTime;
        }
        if(jumpAux >= 7f)
        {
            animator.SetBool(jumpParam, false);
            jumpAux = 0;
            contAux = false;
        }

        if (press && Button)
        {
            animator.SetBool(buttonParam, true);
            contPressAux = true;           
        }
        if (contPressAux)
        {
            pressAux += 4f * Time.deltaTime;
        }
        if (pressAux >= 12f)
        {
            animator.SetBool(buttonParam, false);
            pressAux = 0;
            contPressAux = false;
            obstacle1.SetActive(true);
            obstacle2.SetActive(true);
            obstacle3.SetActive(true);
        }
       


    }
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Stairs")
        {
            stairs = true;
            animator.SetBool(stairsParam, stairs);
        }
        if (coll.tag == "Button")
        {
            Button = true;
        }
        


    }
    private void OnTriggerExit(Collider coll)
    {
        if (coll.tag == "Stairs")
        {
            stairs = false;
            animator.SetBool(stairsParam, stairs);
        }
        if (coll.tag == "Button")
        {
            Button = false;
        }
        

    }

    
    private void OnAnimatorIK(int layerIndex)
    {
        if (Speed < rotationThreshold) return;

        float distancetoLeftFoot = Vector3.Distance(transform.position, animator.GetIKPosition(AvatarIKGoal.LeftFoot));
        float distancetoRightFoot = Vector3.Distance(transform.position, animator.GetIKPosition(AvatarIKGoal.RightFoot));

        if(distancetoRightFoot > distancetoLeftFoot)
        {
            mirrorIdle = true;
        }
        else
        {
            mirrorIdle = false;
        }


    }

    
}
