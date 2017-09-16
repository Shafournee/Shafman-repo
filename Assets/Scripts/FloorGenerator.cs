using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator : MonoBehaviour {

    int RoomNumber;
    int RoomMax;
    //The actual coordinates of the room
    int Roomx;
    int Roomy;
    //Tells the game if it can create a room there
    public GameObject RoomPrefab;
    public GameObject ExitRoomPrefab;
    public GameObject ItemRoomPrefab;
    public GameObject[] PrefabRoomsList;
    GameObject GameManager;
    GameObject MinimapBackground;
    //Tells the game which type of room to spawn (normal, exit, item)
    private GameObject InstantiatedRoom;
    //Tells the game which room it's using as a reference to choose where to spawn the next room
    GameObject SpawnReferenceRoom;
    public List<GameObject> RoomList;
    public List<Vector2> CoordinateList;
    //Tell the game if it's spawning the exit room so you can change the sprite of the door
    bool ItemRoomSpawning;
    bool ExitRoomSpawning;
    public Sprite NormalDoorTopBottom;
    public Sprite NormalDoorRightLeft;
    public Sprite ItemDoorTopBottom;
    public Sprite ItemDoorRightLeft;
    public Sprite ExitDoorTopBottom;
    public Sprite ExitDoorRightLeft;

	// Use this for initialization
	void Awake () {
        GameManager = GameObject.FindGameObjectWithTag("GameManager");
        MinimapBackground = GameObject.Find("MinimapBackground");
        RoomMax = GameManager.GetComponent<GameManager>().FloorLevel + 6;
        RoomMax = 4;
        Roomx = 0;
        Roomy = 0;
        RoomList = new List<GameObject>();
        CoordinateList = new List<Vector2>();
        FloorGeneratorAlgorithm();
        ItemRoomSpawning = false;
        ExitRoomSpawning = false;
        MinimapBackground.GetComponent<Minimap>().CreateTheMinimap(CoordinateList);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FloorGeneratorAlgorithm ()
    {
        //Create the origin room and add it to the room list
        GameObject OriginRoom = Instantiate(RoomPrefab, new Vector3(Roomx, Roomy, 1f), transform.rotation).gameObject;
        OriginRoom.name = "Room(0,0)";
        RoomList.Add(OriginRoom);
        CoordinateList.Add(new Vector2(0, 0));

        while (RoomNumber <= RoomMax)
        {
            //Pull a random room from the list of rooms
            GameObject RandomRoom = RoomList[Random.Range(0, RoomList.Count)];
            //Choose a Random direction to spawn rooms
            int RoomSpawnDirection = Random.Range(0, 4);
            if (RoomNumber == RoomMax)
            {
                InstantiatedRoom = ItemRoomPrefab;
                SpawnReferenceRoom = RoomList[GenerateTheItemRoom()];
                //Ensures the right door will spawn for the item room
                ItemRoomSpawning = true;
            }
            else if (RoomNumber == RoomMax - 1)
            {
                //The prefab for what room to use to create
                InstantiatedRoom = ExitRoomPrefab;
                SpawnReferenceRoom = RoomList[GenerateTheExitRoom()];
                //Ensures the right door will spawn for the exit room
                ExitRoomSpawning = true;
            }
            
            else
            {
                InstantiatedRoom = PrefabRoomsList[Random.Range(0, PrefabRoomsList.Length)];
                SpawnReferenceRoom = RandomRoom;
            }

            //Spawn a room below
            if(RoomSpawnDirection == 0)
            {
                Vector3 PossiblePosition = new Vector3(SpawnReferenceRoom.transform.position.x, SpawnReferenceRoom.transform.position.y - 12f, 1f);
                Vector3 RoomBelow = new Vector3(SpawnReferenceRoom.transform.position.x, SpawnReferenceRoom.transform.position.y - 24f, 1f);
                Vector3 RoomLeft = new Vector3(SpawnReferenceRoom.transform.position.x - 20f, SpawnReferenceRoom.transform.position.y - 12f, 1f);
                Vector3 RoomRight = new Vector3(SpawnReferenceRoom.transform.position.x + 20f, SpawnReferenceRoom.transform.position.y - 12f, 1f);
                //Ensure the room is not currently occupied, if it is loop again
                if (CheckIfRoomSpaceIsntOccupied(PossiblePosition, RoomBelow, RoomLeft, RoomRight))
                {
                    //Find the Coordinates of the current room using the index of the random room and set the new coordinates to the created room coordinates
                    Vector2 TemporaryCoordinates = CoordinateList[RoomList.IndexOf(SpawnReferenceRoom)];
                    Vector2 RoomCoordinates = new Vector2(TemporaryCoordinates.x, TemporaryCoordinates.y - 1);
                    //Create a room in the new position and name it correctly
                    GameObject CreatedRoom = Instantiate(InstantiatedRoom, new Vector3(SpawnReferenceRoom.transform.position.x, SpawnReferenceRoom.transform.position.y - 12, 1f), transform.rotation).gameObject;
                    CreatedRoom.name = "Room(" + RoomCoordinates.x + "," + RoomCoordinates.y + ")";
                    //Add the new room and new coordinates to an array
                    RoomList.Add(CreatedRoom);
                    CoordinateList.Add(RoomCoordinates);
                    //Send the rooms to the open doors script
                    TagDoors("Down", SpawnReferenceRoom, CreatedRoom);
                    RoomNumber++;
                }
            }
            //Spawn a room above
            else if (RoomSpawnDirection == 1)
            {
                Vector3 PossiblePosition = new Vector3(SpawnReferenceRoom.transform.position.x, SpawnReferenceRoom.transform.position.y + 12f, 1f);
                Vector3 RoomAbove = new Vector3(SpawnReferenceRoom.transform.position.x, SpawnReferenceRoom.transform.position.y + 24f, 1f);
                Vector3 RoomLeft = new Vector3(SpawnReferenceRoom.transform.position.x - 20f, SpawnReferenceRoom.transform.position.y + 12f, 1f);
                Vector3 RoomRight = new Vector3(SpawnReferenceRoom.transform.position.x + 20f, SpawnReferenceRoom.transform.position.y + 12f, 1f);
                if (CheckIfRoomSpaceIsntOccupied(PossiblePosition, RoomAbove, RoomLeft, RoomRight))
                {
                    Vector2 TemporaryCoordinates = CoordinateList[RoomList.IndexOf(SpawnReferenceRoom)];
                    Vector2 RoomCoordinates = new Vector2(TemporaryCoordinates.x, TemporaryCoordinates.y + 1);
                    GameObject CreatedRoom = Instantiate(InstantiatedRoom, new Vector3(SpawnReferenceRoom.transform.position.x, SpawnReferenceRoom.transform.position.y + 12, 1f), transform.rotation).gameObject;
                    CreatedRoom.name = "Room(" + RoomCoordinates.x + "," + RoomCoordinates.y + ")";
                    RoomList.Add(CreatedRoom);
                    CoordinateList.Add(RoomCoordinates);
                    TagDoors("Up", SpawnReferenceRoom, CreatedRoom);
                    RoomNumber++;
                }
            }
            //Spawn a room left
            else if (RoomSpawnDirection == 2)
            {
                Vector3 PossiblePosition = new Vector3(SpawnReferenceRoom.transform.position.x - 20f, SpawnReferenceRoom.transform.position.y, 1f);
                Vector3 RoomLeft = new Vector3(SpawnReferenceRoom.transform.position.x - 40f, SpawnReferenceRoom.transform.position.y, 1f);
                Vector3 RoomAbove = new Vector3(SpawnReferenceRoom.transform.position.x - 20f, SpawnReferenceRoom.transform.position.y + 12f, 1f);
                Vector3 RoomBelow = new Vector3(SpawnReferenceRoom.transform.position.x - 20f, SpawnReferenceRoom.transform.position.y - 12f, 1f);
                if (CheckIfRoomSpaceIsntOccupied(PossiblePosition, RoomLeft, RoomAbove, RoomBelow))
                {
                    Vector2 TemporaryCoordinates = CoordinateList[RoomList.IndexOf(SpawnReferenceRoom)];
                    Vector2 RoomCoordinates = new Vector2(TemporaryCoordinates.x - 1, TemporaryCoordinates.y);
                    GameObject CreatedRoom = Instantiate(InstantiatedRoom, new Vector3(SpawnReferenceRoom.transform.position.x - 20, SpawnReferenceRoom.transform.position.y, 1f), transform.rotation).gameObject;
                    CreatedRoom.name = "Room(" + RoomCoordinates.x + "," + RoomCoordinates.y + ")";
                    RoomList.Add(CreatedRoom);
                    CoordinateList.Add(RoomCoordinates);
                    TagDoors("Left", SpawnReferenceRoom, CreatedRoom);
                    RoomNumber++;
                }
            }
            //Spawn a room right
            else if (RoomSpawnDirection == 3)
            {
                Vector3 PossiblePosition = new Vector3(SpawnReferenceRoom.transform.position.x + 20f, SpawnReferenceRoom.transform.position.y, 1f);
                Vector3 RoomRight = new Vector3(SpawnReferenceRoom.transform.position.x + 40f, SpawnReferenceRoom.transform.position.y, 1f);
                Vector3 RoomAbove = new Vector3(SpawnReferenceRoom.transform.position.x + 20f, SpawnReferenceRoom.transform.position.y + 12f, 1f);
                Vector3 RoomBelow = new Vector3(SpawnReferenceRoom.transform.position.x + 20f, SpawnReferenceRoom.transform.position.y - 12f, 1f);
                if (CheckIfRoomSpaceIsntOccupied(PossiblePosition, RoomRight, RoomAbove, RoomBelow))
                {
                    Vector2 TemporaryCoordinates = CoordinateList[RoomList.IndexOf(SpawnReferenceRoom)];
                    Vector2 RoomCoordinates = new Vector2(TemporaryCoordinates.x + 1, TemporaryCoordinates.y);
                    GameObject CreatedRoom = Instantiate(InstantiatedRoom, new Vector3(SpawnReferenceRoom.transform.position.x + 20, SpawnReferenceRoom.transform.position.y, 1f), transform.rotation).gameObject;
                    CreatedRoom.name = "Room(" + RoomCoordinates.x + "," + RoomCoordinates.y + ")";
                    RoomList.Add(CreatedRoom);
                    CoordinateList.Add(RoomCoordinates);
                    TagDoors("Right", SpawnReferenceRoom, CreatedRoom);
                    RoomNumber++;
                }
            }
        }

    }

    private bool CheckIfRoomSpaceIsntOccupied(Vector3 PossibleRoomPosition, Vector3 RoomAround1, Vector3 RoomAround2, Vector3 RoomAround3)
    {
        //Use a for loop to check if each room in the array is already occupied by the possible spawn position of a new room
        for(int i = 0; i < RoomList.Count; i++)
        {
            if(RoomList[i].transform.position == PossibleRoomPosition)
            {
                return false;
            }
            else if(RoomList[i].transform.position == RoomAround1)
            {
                return false;
            }
            else if (RoomList[i].transform.position == RoomAround2)
            {
                return false;
            }
            else if (RoomList[i].transform.position == RoomAround3)
            {
                return false;
            }
        }
        return true;
    }

    private void TagDoors (string DoorSide, GameObject OriginRoom, GameObject NewRoom)
    {
        if (DoorSide == "Down")
        {
            //Tag the correct doors as doors so they open when enemies die
            Transform OriginDoor = OriginRoom.transform.Find("BottomWallDoor");
            OriginDoor.tag = "Door";
            NewRoom.transform.Find("TopWallDoor").tag = "Door";
            //Assign the correct sprites to the exit rooms and item rooms
            if (ExitRoomSpawning)
            {
                OriginDoor.GetComponent<SpriteRenderer>().sprite = ExitDoorTopBottom;
                OriginDoor.GetComponent<SpriteRenderer>().flipY = true;
                ExitRoomSpawning = false;
            }
            else if (ItemRoomSpawning)
            {
                OriginDoor.GetComponent<SpriteRenderer>().sprite = ItemDoorTopBottom;
                OriginDoor.GetComponent<SpriteRenderer>().flipY = true;
                ItemRoomSpawning = false;
            }
            //If neither the item or exit room assign the normal room sprite
            else
            {
                OriginDoor.GetComponent<SpriteRenderer>().sprite = NormalDoorTopBottom;
                OriginDoor.GetComponent<SpriteRenderer>().flipY = true;
            }
        }
        else if (DoorSide == "Up")
        {
            Transform OriginDoor = OriginRoom.transform.Find("TopWallDoor");
            OriginDoor.tag = "Door";
            NewRoom.transform.Find("BottomWallDoor").tag = "Door";
            if (ExitRoomSpawning)
            {
                OriginDoor.GetComponent<SpriteRenderer>().sprite = ExitDoorTopBottom;
                ExitRoomSpawning = false;
            }
            else if (ItemRoomSpawning)
            {
                OriginDoor.GetComponent<SpriteRenderer>().sprite = ItemDoorTopBottom;
                ItemRoomSpawning = false;
            }
            else
            {
                OriginDoor.GetComponent<SpriteRenderer>().sprite = NormalDoorTopBottom;
            }
        }
        else if (DoorSide == "Left")
        {
            Transform OriginDoor = OriginRoom.transform.Find("LeftWallDoor");
            OriginDoor.tag = "Door";
            NewRoom.transform.Find("RightWallDoor").tag = "Door";
            if (ExitRoomSpawning)
            {
                OriginDoor.GetComponent<SpriteRenderer>().sprite = ExitDoorRightLeft;
                OriginDoor.GetComponent<SpriteRenderer>().flipX = true;
                ExitRoomSpawning = false;
            }
            else if (ItemRoomSpawning)
            {
                OriginDoor.GetComponent<SpriteRenderer>().sprite = ItemDoorRightLeft;
                OriginDoor.GetComponent<SpriteRenderer>().flipX = true;
                ItemRoomSpawning = false;
            }
            else
            {
                OriginDoor.GetComponent<SpriteRenderer>().sprite = NormalDoorRightLeft;
                OriginDoor.GetComponent<SpriteRenderer>().flipY = true;
            }
        }
        else if (DoorSide == "Right")
        {
            Transform OriginDoor = OriginRoom.transform.Find("RightWallDoor");
            OriginDoor.tag = "Door";
            NewRoom.transform.Find("LeftWallDoor").tag = "Door";
            if (ExitRoomSpawning)
            {
                OriginDoor.GetComponent<SpriteRenderer>().sprite = ExitDoorRightLeft;
                ExitRoomSpawning = false;
            }
            else if (ItemRoomSpawning)
            {
                OriginDoor.GetComponent<SpriteRenderer>().sprite = ItemDoorRightLeft;
                ItemRoomSpawning = false;
            }
            else
            {
                OriginDoor.GetComponent<SpriteRenderer>().sprite = NormalDoorRightLeft;
            }
        }
    }

    private int GenerateTheItemRoom()
    {
        //Figure out how to place the item room as far away from the exit room as you can
        float BiggestDistance = 0f;
        int CoordinateListPositionItemRoom = 0;
        //Get the coordinates of the exit room, which will be the last value entered at this point (The second to last once the item room is placed)
        Vector2 ExitRoom = CoordinateList[RoomList.Count - 1];
        for (int i = 0; i < RoomList.Count; i++)
        {
            Vector2 ComparisonVector = CoordinateList[i];
            float Distance = Vector2.Distance(ComparisonVector, ExitRoom);
            if (Distance > BiggestDistance)
            {
                BiggestDistance = Distance;
                CoordinateListPositionItemRoom = i;
            }
        }
        return CoordinateListPositionItemRoom;
    }

    private int GenerateTheExitRoom()
    {
        float BiggestMagnitude = 0f;
        int CoordinateListPositionExitRoom = 0;
        for (int i = 0; i < RoomList.Count; i++)
        {
            Vector2 ComparisonVector = CoordinateList[i];
            if(ComparisonVector.magnitude > BiggestMagnitude)
            {
                BiggestMagnitude = ComparisonVector.magnitude;
                CoordinateListPositionExitRoom = i;
            }
        }
        return CoordinateListPositionExitRoom;
    }
    
}
