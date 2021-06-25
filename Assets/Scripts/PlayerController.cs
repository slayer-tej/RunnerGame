using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Rigidbody rbd;
    private Vector3 startPos;
    private Animator _anim;
    private bool fingerDown;
    private int currentLane = 1;
    [SerializeField]
    private float laneDistance = 4f;
    private float verticalVelocity = -0.1f;


    [SerializeField]
    private float speed;
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private float horizontalSpeed;
    [SerializeField]
    private int pixelDistToDetect;

    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float gravity;


    // Start is called before the first frame update
    void Start()
    {
        rbd = gameObject.GetComponent<Rigidbody>();
        controller = gameObject.GetComponent<CharacterController>();
        _anim = gameObject.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //Vector3 verticalMovement = Vector3.forward * speed * Time.fixedDeltaTime;
        //rbd.MovePosition(transform.position +  verticalMovement);
    }
    // Update is called once per frame
    void Update()
    {
        SwipeControls();
        TotalDistanceCovered();
    }

    private void SwipeControls()
    {
        if (!fingerDown && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            startPos = Input.touches[0].position;
            fingerDown = true;
        }

        if (fingerDown && Input.touches[0].phase == TouchPhase.Moved)
        {
            if (Input.touches[0].position.y >= startPos.y + pixelDistToDetect)
            {
                verticalVelocity = -0.1f;
                _anim.SetTrigger("isJumped");
                verticalVelocity = jumpForce;
                fingerDown = false;
            }
            else if (Input.touches[0].position.x <= startPos.x - pixelDistToDetect)
            {
                MoveLane(false);
                _anim.SetTrigger("isMovedLeft");
                fingerDown = false;
                Debug.Log("swipeleft");
            }
            else if (Input.touches[0].position.x >= startPos.x + pixelDistToDetect)
            {
                MoveLane(true);
                _anim.SetTrigger("isMovedRight");
                fingerDown = false;
                Debug.Log("swipeRight");
            }
            else if (Input.touches[0].position.y <= startPos.y - pixelDistToDetect)
            {
                _anim.SetTrigger("isSliding");
                fingerDown = false;
            }
        }

        if (fingerDown && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)
        {
            fingerDown = false;
        }

        Vector3 targetPos = transform.position.z * Vector3.forward;
        if (currentLane == 0)
            targetPos += Vector3.left * laneDistance;
        else if (currentLane == 2)
            targetPos += Vector3.right * laneDistance;

        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPos - transform.position).normalized.x * speed;
        moveVector.y = verticalVelocity;
        moveVector.z = speed;


        controller.Move(moveVector * Time.deltaTime);
    }

    private void MoveLane(bool goingRight)
    {
        currentLane += (goingRight) ? 1 : -1;
        currentLane = Mathf.Clamp(currentLane, 0, 2);
    }

    public void CreateGround()
    {
        gameController.GetItemFromPool();
    }
    public void TotalDistanceCovered()
    {
        gameController.playerDistance =  this.transform.position.z;
    }
}
