using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{

    CharacterController controller;
    Animator animator;

    public float rotationSpeed;
    public float movementSpeed;
    public float RunSpeed;
    public float AttackSpeed;
    public float JumpPower;
    public float gravity = 20;
    Vector3 movementVector = Vector3.zero;
    private float desiredRotationAngle = 0;



    bool isRun = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        movementVector.y -= gravity;
        if(controller.isGrounded)
        {
           
            Jump();
            RunCheck();
        }
        else
            RotatePlayer_Rotation(false);


       
        controller.Move(movementVector * Time.deltaTime);
    }
    void Jump()
    {
        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            movementVector.y = JumpPower;
        }
    }
    void RunCheck()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && isRun == false)
        {
            isRun = true;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift) && isRun == true )
        {
            isRun = false;
            StartCoroutine(RunToWalk_Speed());
        }
    }
    IEnumerator RunToWalk_Speed()
    {
        float FadeOut_Time = 0f;
        float FadeOut_TimeCheck = 0.5f;
        float ChangeMovementSpeed = movementSpeed;
        movementSpeed = RunSpeed;
        while (movementSpeed > ChangeMovementSpeed)
        {
            FadeOut_Time += Time.deltaTime / FadeOut_TimeCheck;
            movementSpeed = Mathf.Lerp(RunSpeed, ChangeMovementSpeed, FadeOut_Time);
            yield return null;
        }
        movementSpeed = ChangeMovementSpeed;
        yield return null;
    }
    public void HandleMovement(Vector2 input, bool isAttack)
    {
        if (controller.isGrounded)
        {
            if (isAttack)
            {
                RotatePlayer_Rotation(isAttack);

            }
            if (input.y == 0 && input.x == 0)
            {//Stop
                movementVector = Vector3.zero;
                animator.SetFloat("Battle_Walk", 0);
                animator.SetBool("IsRun", false);
            }
            else
            {
                RotatePlayer_Rotation(isAttack);
                if (isAttack)
                {
                    movementVector = transform.forward * AttackSpeed;

                    animator.SetFloat("Battle_Walk", Mathf.Max(Mathf.Abs(input.x), Mathf.Abs(input.y)));
                    return;
                }
                if (isRun)
                {
                    //movementVector = GetComponent<ISkill>().IsAttack() ? transform.forward * AttackSpeed : transform.forward * RunSpeed;
                    movementVector = transform.forward * RunSpeed;
                    animator.SetBool("IsRun", true);
                }
                else
                {
                    //movementVector = GetComponent<ISkill>().IsAttack() ? transform.forward * AttackSpeed : transform.forward * movementSpeed;
                    movementVector = transform.forward * movementSpeed;

                    animator.SetBool("IsRun", false);
                    
                    animator.SetFloat("Battle_Walk", Mathf.Max(Mathf.Abs(input.x), Mathf.Abs(input.y)));
                }
            }
        }
    }

    void Movement(Vector3 vec3, float aniSpeed)
    {
        movementVector = Vector3.zero;
        animator.SetFloat("Battle_Walk", 0);
    }

    public void HandleMovementDirection(Vector3 direction)
    {
        desiredRotationAngle = Vector3.Angle(transform.forward, direction);
        var crossProduct = Vector3.Cross(transform.forward, direction).y;
        if (crossProduct < 0)
        {
            desiredRotationAngle *= -1;
        }

    }

    void RotatePlayer_Rotation(bool isAttack) //������ ���� �÷��̾� ȸ��
    {
        //if (desiredRotationAngle_Front > 10 || desiredRotationAngle_Front < -10)
        {
            transform.Rotate(Vector3.up * desiredRotationAngle * rotationSpeed * Time.deltaTime);
        }
    }




}
