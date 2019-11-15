using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MultiplayerControl))]
public class GameManager : MonoBehaviour
{
    //the frame of the player for spawning
    public GameObject playerPrefab;

    //tell if the game is currently in the menu screen, or in the game screen
    public bool inMenu = true;
    
    #region Singleton
    public static GameManager instance = null;
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

    void Start()
    {
        /*
        if (playerPrefab != null && !inMenu)
        {
            SpawnPlayers();
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        //get how many players are to be spawned
        var playerSpawnCount = MultiplayerControl.instance.playerCount;

        //get all the players on screen
        var currentPlayersOnScreen = GameObject.FindGameObjectsWithTag("Player");

        //if the game manager obejct is not in a menu scene, spawn players
        if (!inMenu && playerSpawnCount > currentPlayersOnScreen.Length)
        {
            SpawnPlayers();
        }
        //remove all players if the object returns to a menu scene
        if (inMenu && currentPlayersOnScreen.Length > 0)
        {
            foreach (var i in currentPlayersOnScreen)
            {
                Destroy(i);
            }
        }
    }

    public void TurnOnMenu()
    {
        inMenu = true;
    }

    public void TurnOffMenu()
    {
        inMenu = false;
        Debug.Log("turning off menu");
    }

    public void SpawnPlayers()
    {
        for (int i = 1; i <= MultiplayerControl.instance.playerCount; i++)
        {
            //spawn the player from the top of the screen
            Vector2 playerSpawnPoint = Camera.main.ScreenToWorldPoint(new Vector2((MultiplayerControl.instance.playScreen.x / 2.5f) * i, Screen.height));

            //spawn empty player frame object
            var thisPlayer = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);

            //assign the player index
            thisPlayer.GetComponent<PlayerControls>().PlayerIndex = i;

            //initialize the player stats
            thisPlayer.GetComponent<PlayerStatus>().strength = 15;
            thisPlayer.GetComponent<PlayerStatus>().defence = 0;
            thisPlayer.GetComponent<PlayerStatus>().manaPoint = 5;
            thisPlayer.GetComponent<PlayerStatus>().weight = 5;

            //add the euqipments the player choose
            //thisPlayer.GetComponent<PlayerStatus>().equipments.Add();

            Debug.Log("Spawning player " + i);
        }
    }

    
}
