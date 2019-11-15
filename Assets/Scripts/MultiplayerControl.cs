using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerControl : MonoBehaviour
{
    [Range(1, 4)]
    public int playerCount = 1;

    [HideInInspector]
    public Vector2 playScreen;

    public bool isTouchControls = false;

    #region Singleton
    public static MultiplayerControl instance = null;
    // Start is called before the first frame update
    void Awake()
    {
        //a singleton to prevent object from being duplicated
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            //prevent this object from being destroyed with scene transition
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        SetupMultiplayerControlScreen();
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// Divides the screen according to the number of players for input areas
    /// </summary>
    public void SetupMultiplayerControlScreen()
    {
        //enabnle multi touch
        Input.multiTouchEnabled = true;

        //adjust the control screen size according to the number of players
        switch (playerCount)
        {
            case 1:
                playScreen = new Vector2(Screen.width, Screen.height);
                break;
            case 2:
                playScreen = new Vector2(Screen.width / 2f, Screen.height);
                break;
            default:
                playScreen = new Vector2(Screen.width / 2f, Screen.height / 2f);
                break;
        }
    }
    
    /// <summary>
    /// Returns the player index of screen. returns -1 if none
    /// </summary>
    /// <param name="touchLoc"></param>
    /// <returns></returns>
    public int TouchedArea(Vector2 touchLoc)
    {
        if (touchLoc.x < Screen.width && touchLoc.y < Screen.height && touchLoc.x > 0 && touchLoc.y > 0)
        {
            if (touchLoc.x < playScreen.x && touchLoc.y < playScreen.y)
            {
                return 1;
            }
            else if (touchLoc.x > playScreen.x && touchLoc.y < playScreen.y)
            {
                return 2;
            }
            else if (touchLoc.x < playScreen.x && touchLoc.y > playScreen.y)
            {
                return 3;
            }
            else if (touchLoc.x > playScreen.x && touchLoc.y > playScreen.y && playerCount == 4)
            {
                return 4;
            }
            else
            {
                return -1;
            }
        }
        else
        {
            return -1;
        }
    }
    
    public void AddPlayer()
    {
        if (playerCount < 4)
        {
            playerCount += 1;
            SetupMultiplayerControlScreen();
        }
        
        Debug.Log("Current player " + playerCount);
    }

    public void ReducePlayer()
    {
        if (playerCount > 1)
        {
            playerCount -= 1;
            SetupMultiplayerControlScreen();
        }
        
        Debug.Log("Current player " + playerCount);
    }

}
