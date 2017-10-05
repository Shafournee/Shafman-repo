using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {

    GameObject[] Doors;
    //Get the transform of the player
    Transform PlayerTransform;
    //Get the transform of the Camera
    Transform CameraTransform;
    //Find the Exit on the floor
    GameObject Exit;
    //Find the PlayerUI
    GameObject Canvas;
    //Find the minimap
    GameObject Minimap;
    //Gets the heart prefab so you can spawn hearts on room completion
    public GameObject HeartPrefab;
    //Gets the Half Heart
    public GameObject HalfHeartPrefab;
    //Tells the code what the grid number of the room is
    public int RoomCoordinatex = 0;
    public int RoomCoordinatey = 0;
    //Find if the item has spawned in a collision area
    public bool ItemIsColliding;

    public Component[] EnemiesInRoom;

	// Use this for initialization
	void Start () {

        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        CameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        Doors = GameObject.FindGameObjectsWithTag("Door");
        //Sets all doors open initially
        OpenDoorsOnStart();
        //Find the canvas
        Canvas = GameObject.FindGameObjectWithTag("Canvas");
        //Find the minimap
        Minimap = GameObject.FindGameObjectWithTag("Minimap");
        //Close the exit and make it invisible
        Exit = GameObject.FindGameObjectWithTag("Exit");
        Exit.GetComponent<BoxCollider2D>().enabled = false;
        Exit.GetComponent<SpriteRenderer>().enabled = false;
        DisableEnemiesOnStart();
    }
	
	// Update is called once per frame
	void Update () {


        

    }

    private void DisableEnemiesOnStart()
    {
        GameObject[] Enemies;
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < Enemies.Length; i++)
        {
            Enemies[i].GetComponent<SpriteRenderer>().enabled = false;
            Enemies[i].GetComponent<BaseEnemy>().enabled = false;
        }
    }

    //Update Coordinates, Teleport the player, and teleport the camera when the player enters a door trigger
    public void OnDoorEnter(string DoorSide)
    {
        

        //Moves the minimap when you enter a door
        if (DoorSide == "Right")
        {
            RoomCoordinatex++;
            PlayerTransform.position = new Vector3(PlayerTransform.position.x + 5, PlayerTransform.position.y, PlayerTransform.position.z);
            CameraTransform.position = new Vector3(CameraTransform.position.x + 20, CameraTransform.position.y, CameraTransform.position.z);
        }

        else if (DoorSide == "Left")
        {
            RoomCoordinatex--;
            PlayerTransform.position = new Vector3(PlayerTransform.position.x - 5, PlayerTransform.position.y, PlayerTransform.position.z);
            CameraTransform.position = new Vector3(CameraTransform.position.x - 20, CameraTransform.position.y, CameraTransform.position.z);
        }

        else if (DoorSide == "Top")
        {
            RoomCoordinatey++;
            PlayerTransform.position = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y + 5.5f, PlayerTransform.position.z);
            CameraTransform.position = new Vector3(CameraTransform.position.x, CameraTransform.position.y + 12, CameraTransform.position.z);
        }

        else if (DoorSide == "Bottom")
        {
            RoomCoordinatey--;
            PlayerTransform.position = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y - 5.5f, PlayerTransform.position.z);
            CameraTransform.position = new Vector3(CameraTransform.position.x, CameraTransform.position.y - 12, CameraTransform.position.z);
        }
        //Moves the minimap upon entering the new room
        Minimap.GetComponent<Minimap>().MoveTheMinimap(DoorSide, RoomCoordinatex, RoomCoordinatey);
        Minimap.GetComponent<Minimap>().SetRoomsActive();
        //Find the current room coordinates
        GameObject CurrentRoom = GameObject.Find("Room(" + RoomCoordinatex + "," + RoomCoordinatey + ")");

        //If there are enemies or a boss close all the doors on the level and spawn the enemies
        if (CurrentRoom.GetComponentsInChildren<BaseEnemy>(true).Length > 0 || CurrentRoom.GetComponentsInChildren<BaseBoss>(true).Length > 0)
        {
            //Closes all doors
            Doors = GameObject.FindGameObjectsWithTag("Door");
            for (int i = 0; i < Doors.Length; i++)
            {
                Doors[i].GetComponent<BoxCollider2D>().isTrigger = false;
                Doors[i].GetComponent<SpriteRenderer>().enabled = false;
            }
            StartCoroutine(HoldForEnemiesToSpawn(CurrentRoom));
            //Disable the player movement
            PlayerTransform.GetComponent<Player>().PlayerCanShoot = false;
            PlayerTransform.GetComponent<Player>().PlayerCanMove = false;
            PlayerTransform.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, 0f, 1f);
            if(CurrentRoom.GetComponentsInChildren<BaseBoss>(true).Length > 0)
            {
                StartCoroutine(SpawnTheBoss(CurrentRoom));
            }
        }
    }

    public void OpenDoorsOnEnemyDeaths()
    {
        StartCoroutine(HoldFor15FramesToEnsureEnemyDeathThenOpenDoors());
    }

    private IEnumerator HoldForEnemiesToSpawn(GameObject CurrentRoom)
    {
        for (int i = 0; i < 30; i++)
        {
            yield return null;
        }
        //reenable the player movement
        PlayerTransform.GetComponent<Player>().PlayerCanShoot = true;
        PlayerTransform.GetComponent<Player>().PlayerCanMove = true;
        //Activates each enemy in a room
        BaseEnemy[] EnemiesInRoom = CurrentRoom.GetComponentsInChildren<BaseEnemy>(true);
        for (int i = 0; i < EnemiesInRoom.Length; i++)
        {
            EnemiesInRoom[i].gameObject.GetComponent<BaseEnemy>().enabled = true;
            EnemiesInRoom[i].gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    //Spawns the boss (and does the other things) for the current room
    private IEnumerator SpawnTheBoss(GameObject CurrentRoom)
    {
        BaseBoss Boss = CurrentRoom.GetComponentInChildren<BaseBoss>(true);
        GameObject BossObject = Boss.gameObject;
        string BossName = BossObject.GetComponent<BaseBoss>().BossName;
        Sprite BossSprite = BossObject.GetComponent<BaseBoss>().BossSprite;
        //Get the Boss Sprite and the Boss Name to draw on the screen
        Canvas.GetComponent<PlayerUI>().BossFightScreenCoroutine(BossSprite, BossName);
        Canvas.GetComponent<PlayerUI>().ShowTheBossHealthBar();
        yield return new WaitForSeconds(4.1f);
        //Enable the player, and then enable the boss
        PlayerTransform.GetComponent<Player>().PlayerCanShoot = true;
        PlayerTransform.GetComponent<Player>().PlayerCanMove = true;
        Boss.gameObject.GetComponent<BaseBoss>().enabled = true;
    }

    public IEnumerator HoldFor15FramesToEnsureEnemyDeathThenOpenDoors()
    {
        for (int i = 0; i < 15; i++)
        {
            yield return null;
        }
        //Find the current room
        GameObject CurrentRoom = GameObject.Find("Room(" + RoomCoordinatex + "," + RoomCoordinatey + ")");
        //Check to see if the number of enemies in a room is 0 and there are no bosses and no enemies on the floor at all
        if (CurrentRoom.GetComponentsInChildren<BaseEnemy>(true).Length == 0 && CurrentRoom.GetComponentsInChildren<BaseBoss>(true).Length == 0)
        {
            //If the enemies in a room is = 0 open all doors
            for (int i = 0; i < Doors.Length; i++)
            {
                Doors[i].GetComponent<BoxCollider2D>().isTrigger = true;
                Doors[i].GetComponent<SpriteRenderer>().enabled = true;
            }
            SpawnItemsOnRoomClear();
        }
    }
    
    //Open the exit once the boss has died
    public void OpenTheExitOnBossDeath()
    {
        StartCoroutine(HoldForFramesThenOpenExitOnBossDeath());
    }

    private IEnumerator HoldForFramesThenOpenExitOnBossDeath()
    {
        for(int i = 0; i < 5; i++)
        {
            yield return null;
        }
        Exit.GetComponent<BoxCollider2D>().enabled = true;
        Exit.GetComponent<SpriteRenderer>().enabled = true;
    }

    //Open the doors upon loading the level
    private void OpenDoorsOnStart()
    {
        for (int i = 0; i < Doors.Length; i++)
        {
            Doors[i].GetComponent<BoxCollider2D>().isTrigger = true;
            Doors[i].GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public void SpawnItemsOnRoomClear()
    {
        //Spawns a heart with a 30% chance
        float HeartDrop = Random.Range(0f, 1f);
        if (HeartDrop > .5f)
        {
            float FullHeartvsHalf = Random.Range(0f, 1f);
            if (FullHeartvsHalf > .7f)
            {
                GameObject HeartSpawn = Instantiate(HeartPrefab, new Vector3(RoomCoordinatex*20, RoomCoordinatey*12, 1f), transform.rotation).gameObject;
                //StartCoroutine(MoveItemsFromCenter(HeartSpawn));
            }
            //Spawns a half heart with a 50% chance
            else
            {
                GameObject HalfHeartSpawn = Instantiate(HalfHeartPrefab, new Vector3(RoomCoordinatex*20, RoomCoordinatey*12, 1f), transform.rotation).gameObject;
                //StartCoroutine(MoveItemsFromCenter(HalfHeartSpawn));
            }
        }
    }

    private IEnumerator MoveItemsFromCenter(GameObject HeartSpawn)
    {
        HeartPickup Heart = HeartSpawn.GetComponent<HeartPickup>();
        while (Heart.IsColliding)
        {
            HeartSpawn.transform.position = gameObject.transform.position = new Vector3(HeartSpawn.transform.position.x + 1f, HeartSpawn.transform.position.y + 1f, 1f);
            yield return null;
        }
        
    }

    public GameObject CurrentRoomFunction()
    {
        return GameObject.Find("Room(" + RoomCoordinatex + "," + RoomCoordinatey + ")");
    }

}
