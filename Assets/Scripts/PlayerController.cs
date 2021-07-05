using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float jumpPower = 7f;
    [SerializeField]
    private float xValue = 1.5f;
    [SerializeField]
    private int speed;
    [SerializeField]
    private int pixelDistToDetect;
    [SerializeField]
    private int life = 3;
    [SerializeField]
    private HealthController healthController;

    private CharacterController controller;
    private Vector3 startPos;
    private Animator _anim;
    private Lane lane;
    private bool fingerDown;
    private bool swipeDown, swipeUp;
    private bool inSlide;
    private float newXPos;
    private float x;
    private float y;
    private float colliderHeight;
    private float colliderCenterY;
    private float rollCounter;
    private int temp;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        colliderHeight = controller.height;
        colliderCenterY = controller.center.y;
        _anim = gameObject.GetComponent<Animator>();
        transform.position = Vector3.zero;
        lane = Lane.Mid;
    }

    void Update()
    {
        SwipeControls();
        TotalDistanceCovered();
    }

    #region Slide Controls

    private void SwipeControls()
    {
        if (!fingerDown && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            startPos = Input.touches[0].position;
            fingerDown = true;
        }

        if (fingerDown && Input.touches[0].phase == TouchPhase.Moved)
        {
            if (Input.touches[0].position.y >= startPos.y + pixelDistToDetect && !inSlide)
            {
                swipeUp = true;
            }

            else if (Input.touches[0].position.x <= startPos.x - pixelDistToDetect && !inSlide)
            {
                if (lane == Lane.Mid)
                {
                    _anim.SetTrigger("isMovedLeft");
                    newXPos = -xValue;
                    fingerDown = false;
                    lane = Lane.Left;
                    Debug.Log("swipeleft");
                }
                else if (lane == Lane.Right)
                {
                    _anim.SetTrigger("isMovedLeft");
                    newXPos = 0;
                    fingerDown = false;
                    lane = Lane.Mid;
                }
            }
            else if (Input.touches[0].position.x >= startPos.x + pixelDistToDetect && !inSlide)
            {
                if (lane == Lane.Mid)
                {
                    _anim.SetTrigger("isMovedRight");
                    newXPos = xValue;
                    fingerDown = false;
                    lane = Lane.Right;
                    Debug.Log("swipeRight");
                }
                else if (lane == Lane.Left)
                {
                    _anim.SetTrigger("isMovedRight");
                    newXPos = 0;
                    fingerDown = false;
                    lane = Lane.Mid;
                }
            }
            else if (Input.touches[0].position.y <= startPos.y - pixelDistToDetect)
            {
                swipeDown = true;
                fingerDown = false;
                Debug.Log("swipe down");
            }
        }

        if (fingerDown && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)
        {
            fingerDown = false;

        }

        Vector3 moveVector = new Vector3(x - transform.position.x, y * Time.deltaTime, speed * Time.deltaTime);
        x = Mathf.Lerp(x, newXPos, Time.deltaTime * speed);
        controller.Move(moveVector);
        Jump();
        Slide();
    }

    private void Jump()
    {
        if (controller.isGrounded)
        {
            if (swipeUp)
            {
                y = jumpPower;
                _anim.SetTrigger("isJumped");
                fingerDown = false;
                swipeUp = false;
            }
        }
        else
        {
            y -= jumpPower * 2 * Time.deltaTime;
        }
    }

    private void Slide()
    {
        rollCounter -= Time.deltaTime;
        if (rollCounter <= 0f)
        {
            rollCounter = 0f;
            controller.center = new Vector3(0, colliderCenterY, 0);
            controller.height = colliderHeight;
            inSlide = false;
        }
        if (swipeDown)
        {
            _anim.SetTrigger("isSliding");
            controller.center = new Vector3(0, colliderCenterY / 2f, 0);
            controller.height = colliderHeight / 2f;
            rollCounter = 2f;
            y = -10f;
            inSlide = true;
            swipeDown = false;
        }
    }

    #endregion

    public void CreateGround()
    {
        GameManager.Instance.GetItemFromPool();
    }

    public void TotalDistanceCovered()
    {
        int dist = Convert.ToInt32(this.transform.position.z);
        if((dist%200) == 0)
        {
            speed += 1;
        }
        GameManager.Instance.playerDistance = dist;
    }

    internal void UpdateLives(int h)
    {
        life -= h;
        healthController.DisplayHearts(life);
        StartCoroutine(PlayerIdle());
        if (life <= 0)
        {
            this.enabled = false;
            GameManager.Instance.GameOver();
        }
    }

    private IEnumerator PlayerIdle()
    {
        StopPlayer();
        yield return new WaitForSeconds(2f);
        StartAgain();
    }

    private void StartAgain()
    {
        speed = temp;
    }

    private void StopPlayer()
    {
        temp = speed;
        speed = 0;
    }
}

public enum Lane
{
    Left,
    Mid,
    Right
}
