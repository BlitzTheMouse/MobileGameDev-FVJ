using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour
{
    [Header("Swipe Properties")]
    [Tooltip("How far will the player move upon swiping")]
    public float swipeMove = 2f;

    [Tooltip("How far must the player swipe before we will execute the action (in inches)")]
    public float minSwipeDistance = 0.25f;

    private float minSwipeDistancePixels;

    [Header("Scaling Properties")]
    [Tooltip("The minimum size (in Unity units) that the player should be")]
    public float minScale = 0.5f;

    [Tooltip("The maximum size (in Unity units) that the player should be")]
    public float maxScale = 3.0f;

    private float currentScale = 1;

    private Vector2 touchStart;

    private Rigidbody rb;
    public float dodgeSpeed = 5;
    [Range(0, 10)]
    public float rollSpeed = 5;

    public enum MobileHorizMovement
    {
        Accelerometer,
        ScreenTouch
    }
    [Tooltip("What horizontal movement type should be used")]
    public MobileHorizMovement horizMovement = MobileHorizMovement.Accelerometer;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        minSwipeDistancePixels = minSwipeDistance * Screen.dpi;
    }

    private void Update()
    {
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            TouchObjects(screenPos);
        }
#elif UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            TouchObjects(touch.position);
            SwipeTeleport(touch);
            ScalePlayer();
        }
#endif
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            var screenPos = Input.mousePosition;
            horizontalSpeed = CalculateMovement(screenPos);
        }
#elif UNITY_IOS || UNITY_ANDROID

        if(horizMovement == MobileHorizMovement.Accelerometer)
        {
            horizontalSpeed = Input.acceleration.x * dodgeSpeed;
        }

        if (Input.touchCount > 0)
        {
            if (horizMovement == MobileHorizMovement.ScreenTouch)
            {
                Touch touch = Input.touches[0];
                horizontalSpeed = CalculateMovement(touch.position);
            }
        }
#endif

        rb.AddForce(horizontalSpeed, 0, rollSpeed);
    }

    private void SwipeTeleport(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            touchStart = touch.position;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            Vector2 touchEnd = touch.position;
            
            float x = touchEnd.x - touchStart.x;
            if (Mathf.Abs(x) < minSwipeDistancePixels)
            {
                return;
            }
            Vector3 moveDirection;

            if (x < 0)
            {
                moveDirection = Vector3.left;
            }
            else
            {
                moveDirection = Vector3.right;
            }
            RaycastHit hit;

            if (!rb.SweepTest(moveDirection, out hit,swipeMove))
            {
                var movement = moveDirection * swipeMove;
                var newPos = rb.position + movement;
                rb.MovePosition(newPos);
            }
        }
    }
    private float CalculateMovement(Vector3 screenPos)
    {
        var cam = Camera.main;

        var viewPos = cam.ScreenToViewportPoint(screenPos);

        float xMove = 0;

        if (viewPos.x < 0.5f)
        {
            xMove = -1;
        }
        else
        {
            xMove = 1;
        }
        return xMove * dodgeSpeed;
    }
    private void ScalePlayer()
    {
        if (Input.touchCount != 2)
        {
            return;
        }
        else
        {
            Touch touch0 = Input.touches[0];
            Touch touch1 = Input.touches[1];
            Vector2 t0Pos = touch0.position;
            Vector2 t0Delta = touch0.deltaPosition;
            Vector2 t1Pos = touch1.position;
            Vector2 t1Delta = touch1.deltaPosition;

            Vector2 t0Prev = t0Pos - t0Delta;
            Vector2 t1Prev = t1Pos - t1Delta;

            float prevTDeltaMag = (t0Prev - t1Prev).magnitude;
            float tDeltaMag = (t0Pos - t1Pos).magnitude;
           
            float deltaMagDiff = prevTDeltaMag - tDeltaMag;

            float newScale = currentScale;
            newScale -= (deltaMagDiff * Time.deltaTime);
            newScale = Mathf.Clamp(newScale, minScale, maxScale);
            transform.localScale = Vector3.one * newScale;
            currentScale = newScale;
        }
    }
    public static void TouchObjects(Vector2 screenPos)
    {
        Ray touchRay = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        int layerMask = ~0;

        if (Physics.Raycast(touchRay, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
        {
            hit.transform.SendMessage("PlayerTouch", SendMessageOptions.DontRequireReceiver);
        }
    }
}
