using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStatus))]
public class PlayerControls : MonoBehaviour
{
    public ParticleSystem dashEffect;

    public ParticleSystem attackEffect;

    //used to access the multi-touch logic
    private GameObject gameManager;
    
    //these are used for the swipe attack logic, they detrmine the speed and direction of the attack
    [HideInInspector]
    public float swipeTimeStart, swipeTimeFinish, timeInterval;
    [HideInInspector]
    public Vector2 startTouchPos, endTouchPos;

    //used to make sure the player moves apon swipe
    private int? LockedFingerID { get; set; }

    //how long the attack should last
    //todo: change this value to be dynamic according to the attack vector
    public float startAttackTime = 0.25f;
    private float attackTime;
    
    private Rigidbody2D rb2d;

    private PlayerStatus playerStat;

    //indicate which player this is
    public int PlayerIndex { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        //get component will get the component of this object, and not any others
        rb2d = GetComponent<Rigidbody2D>();
        playerStat = GetComponent<PlayerStatus>();
        
        //initialize the starting attack cool down time
        attackTime = startAttackTime;
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        
        
    }

    // Update is called once per frame
    void Update()
    {
        //check if attack time is over
        if (attackTime <= 0)
        {
            //reset attack cooldown
            attackTime = startAttackTime;
            playerStat.isAttacking = false;
        }
        else
        {
            if (gameManager.GetComponent<MultiplayerControl>().isTouchControls)
            {
                SwipeDetection();
            }
            else
            {
                ClickDetection();
            }
            //countdown cooldown
            attackTime -= Time.deltaTime;
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerStat.isAttacking == true)
        {
            //check for hitting object
            if (collision.gameObject.tag == "BreakableObject")
            {
                //damage to object
                Debug.Log("Hitting breakable object");
                var hittingObject = collision.gameObject.GetComponent<ItemBox>();
                hittingObject.health -= playerStat.damage;
            }
        }
    }

    private void LateUpdate()
    {
        //change the player sprite color when the player is attacking
        if (playerStat.isAttacking == true)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(100, 0, 0);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        }
    }

    /// <summary>
    /// The main swipe to attack logic. This method will detect when the finger touches the screen and calculate
    /// the speed and length of the swipe for the acttack power, and will call the AttackForce method
    /// </summary>
    void SwipeDetection()
    {
        if (Input.touchCount > 0)
        {
            foreach (var touch in Input.touches)
            {
                //which character's control area the player is touching now
                var thisAreaPlayer = gameManager.GetComponent<MultiplayerControl>().TouchedArea(touch.position);
                //only run if the player is touching the right area
                if (touch.phase == TouchPhase.Began && PlayerIndex == thisAreaPlayer)
                {

                    if (LockedFingerID == null)
                    {
                        //assign finger ID to prevent duplicate input
                        LockedFingerID = touch.fingerId;
                        //assign the time when the player touches down
                        swipeTimeStart = Time.time;
                        //assign the position the player touched
                        startTouchPos = Camera.main.ScreenToWorldPoint(touch.position);
                    }

                }
                //todo: add swipe boundaries with maximum force
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled || PlayerIndex != thisAreaPlayer)
                {
                    //only execute if the touch ID matches
                    if (LockedFingerID == touch.fingerId)
                    {
                        //reset touch ID
                        LockedFingerID = null;
                        //assign the time when the player lets go
                        swipeTimeFinish = Time.time;
                        //assign the final position of the swipe
                        endTouchPos = Camera.main.ScreenToWorldPoint(touch.position);

                        //calculate how long it took the player to swipe
                        timeInterval = swipeTimeFinish - swipeTimeStart;

                        //add force to the character according to these values
                        AttackForce(startTouchPos, endTouchPos, timeInterval);
                    }
                }
            }
        }
    }

    #region Mouse Controls
    /// <summary>
    /// Simulate a swipe of a screen but with a mouse. This is used for debugging purpose only, not for the actual game
    /// </summary>
    void ClickDetection()
    {
        //which character's control area the player is touching now
        var thisAreaPlayer = gameManager.GetComponent<MultiplayerControl>().TouchedArea(Input.mousePosition);
        
        //only run if the player is touching the right area
        if (Input.GetMouseButtonDown(0) && PlayerIndex == thisAreaPlayer)
        {
            if (LockedFingerID == null)
            {
                LockedFingerID = PlayerIndex;
                swipeTimeStart = Time.time;
                startTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        if (Input.GetMouseButtonUp(0) && LockedFingerID == PlayerIndex)
        {
            LockedFingerID = null;
            swipeTimeFinish = Time.time;
            endTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            timeInterval = swipeTimeFinish - swipeTimeStart;

            //add force to the character according to these values
            AttackForce(startTouchPos, endTouchPos, timeInterval);
        }
    }

    #endregion

    /// <summary>
    /// Add attack force to the character and show the dash effect as well
    /// </summary>
    /// <param name="startLoc">Where touch starts</param>
    /// <param name="endLoc">Where touch ends</param>
    /// <param name="timeInterval">How long it took touch to finish</param>
    public void AttackForce(Vector2 startLoc, Vector2 endLoc, float timeInterval)
    {
        //define attack direction through two points
        Vector2 attackDirection = startLoc - endLoc;
        
        
        //the deadzone for the attack (minimum swiping length)
        float deadzone = 5f;
        float swipeLength = Mathf.Abs(attackDirection.x) + Mathf.Abs(attackDirection.y);
        Debug.Log($"Swipe length: {swipeLength} Swipe direction: {attackDirection}");
        
        //make the attack vector. faster the swipe speed, faster the attack
        Vector2 attackVector = (-attackDirection / timeInterval) * playerStat.strength;
        Debug.Log("Attack direction vector: " + attackVector);

        //only attack if the magnitude is more than 0
        if (attackVector.sqrMagnitude > 0f && swipeLength > deadzone)
        {
            if (dashEffect != null)
            {
                //make jump particles
                Instantiate(dashEffect, transform.position, Quaternion.identity);
                
            }
            if (attackEffect != null)
            {
                //make attacking particles
                Instantiate(attackEffect, transform.position, Quaternion.identity);
            }

            //calculate damage according to the vector magnitude
            playerStat.damage = Mathf.RoundToInt(attackVector.sqrMagnitude / 1000000);
            Debug.Log($"Damage 1: {playerStat.damage} Damage 2: {attackVector.sqrMagnitude}");
            
            if (playerStat.damage > 2)
            {
                //only do damage when it is greater than 2
                playerStat.isAttacking = true;
                startAttackTime = playerStat.damage / 10f;
                Debug.Log($"Attack speed: {startAttackTime} seconds");
            }
            
            //make the character move according to the vector
            rb2d.AddForce(attackVector);
            
        }

    }
}
