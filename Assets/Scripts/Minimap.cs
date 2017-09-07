using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour {

    GameObject FloorGenerator;
    GameObject RoomManager;
    //List<Vector2> RoomCoordinates;
    //Stores the coordinates of each room in the minimap
    List<Vector2> MinimapCoordinates;
    //Stores the coordinate of the room the player is currently in
    Vector2 PlayerCurrentRoom;
    //Tells the minimap to keep the sprite as an explored room
    List<bool> RoomWasExplored;
    //The list of the drawn rooms on the minimap
    List<Image> DrawnRooms;
    //The image prefab for unexplored rooms which are default
    public Image UnexploredRoom;
    //The sprite for the player being in the room
    public Sprite CurrentRoom;
    //The sprite for the player having been in a room, but not currently in it
    public Sprite ExploredRoom;
    //Sprite over the item room
    public Image ItemRoomImage;
    //Sprite over the boss room
    public Image BossRoomImage;
    //Array of both the images
    private Image[] RoomIcons;


	// Use this for initialization
	void Start () {
        FloorGenerator = GameObject.FindGameObjectWithTag("FloorGenerator");
        //RoomCoordinates = FloorGenerator.GetComponent<FloorGenerator>().CoordinateList;
        RoomManager = GameObject.FindGameObjectWithTag("RoomManager");
    }
	
	// Update is called once per frame
	void Update () {
	}

    //This spawns each room on the minimap, stores them in a list, and stores their coordinates in a list and then runs a script to disable all the rooms
    public void CreateTheMinimap(List<Vector2> RoomCoordinates)
    {
        RoomWasExplored = new List<bool>();
        DrawnRooms = new List<Image>();
        MinimapCoordinates = new List<Vector2>();
        RoomIcons = new Image[2];
        for (int i = 0; i < RoomCoordinates.Count; i++)
        {
            Image DrawRoom;
            DrawRoom = Instantiate(UnexploredRoom, new Vector3(0f, 0f, 1f), gameObject.transform.rotation);
            DrawRoom.transform.SetParent(gameObject.transform);
            DrawRoom.transform.localPosition = new Vector3(RoomCoordinates[i].x * 48, RoomCoordinates[i].y * 48, 1f);
            MinimapCoordinates.Add(RoomCoordinates[i]);
            DrawnRooms.Add(DrawRoom);
            RoomWasExplored.Add(false);

            if(DrawnRooms.Count == RoomCoordinates.Count - 1)
            {
                Image DrawBossIcon;
                DrawBossIcon = Instantiate(BossRoomImage, new Vector3(0f, 0f, 1f), gameObject.transform.rotation);
                DrawBossIcon.transform.SetParent(DrawnRooms[i].transform);
                DrawBossIcon.transform.localPosition = new Vector3(0f, 0f, 1f);
                RoomIcons[0] = DrawBossIcon;

            }
            else if(DrawnRooms.Count == RoomCoordinates.Count)
            {
                Image DrawItemIcon;
                DrawItemIcon = Instantiate(ItemRoomImage, new Vector3(0f, 0f, 1f), gameObject.transform.rotation);
                DrawItemIcon.transform.SetParent(DrawnRooms[i].transform);
                DrawItemIcon.transform.localPosition = new Vector3(0f, 0f, 1f);
                RoomIcons[1] = DrawItemIcon;
            }
        }
        DisableAllRooms();
    }

    //Disables all rooms at the start so that they can be revealed when the player gets near them and then set the origin rooms active
    private void DisableAllRooms()
    {
        for (int i = 0; i < DrawnRooms.Count; i++)
        {
            DrawnRooms[i].enabled = false;
            RoomIcons[0].enabled = false;
            RoomIcons[1].enabled = false;
        }
        SetRoomsActive();
    }

    //Moves each room on the minimap when the player changes rooms
    public void MoveTheMinimap(string MoveDirection, int PlayerCoordinatex, int PlayerCoordinatey)
    {
        for (int i = 0; i < DrawnRooms.Count; i++)
        {
            if(MoveDirection == "Top")
            {
                DrawnRooms[i].transform.localPosition = new Vector3(DrawnRooms[i].transform.localPosition.x, DrawnRooms[i].transform.localPosition.y - 48, 1f);
            }
            else if (MoveDirection == "Bottom")
            {
                DrawnRooms[i].transform.localPosition = new Vector3(DrawnRooms[i].transform.localPosition.x, DrawnRooms[i].transform.localPosition.y + 48, 1f);
            }
            else if (MoveDirection == "Left")
            {
                DrawnRooms[i].transform.localPosition = new Vector3(DrawnRooms[i].transform.localPosition.x + 48, DrawnRooms[i].transform.localPosition.y, 1f);
            }
            else if (MoveDirection == "Right")
            {
                DrawnRooms[i].transform.localPosition = new Vector3(DrawnRooms[i].transform.localPosition.x - 48, DrawnRooms[i].transform.localPosition.y, 1f);
            }
        }
        PlayerCurrentRoom = new Vector2(PlayerCoordinatex, PlayerCoordinatey);
        SetRoomsActive();
    }

    public void SetRoomsActive()
    {
        for(int i = 0; i < DrawnRooms.Count; i++)
        {
            //Set the room the player is currently in to be enabled and give it the current room sprite, then store it as having been explored
            if (PlayerCurrentRoom == MinimapCoordinates[i])
            {
                DrawnRooms[i].enabled = true;
                DrawnRooms[i].GetComponent<Image>().sprite = CurrentRoom;
                RoomWasExplored[i] = true;
                //If it's the last few rooms in the array enable their icons
                if(DrawnRooms[i] == DrawnRooms[DrawnRooms.Count - 2])
                {
                    RoomIcons[0].enabled = true;
                }
                else if(DrawnRooms[i] == DrawnRooms[DrawnRooms.Count - 1])
                {
                    RoomIcons[1].enabled = true;
                }
            }
            else if (RoomWasExplored[i])
            {
                DrawnRooms[i].GetComponent<Image>().sprite = ExploredRoom;
                if (DrawnRooms[i] == DrawnRooms[DrawnRooms.Count - 2])
                {
                    RoomIcons[0].enabled = true;
                }
                else if (DrawnRooms[i] == DrawnRooms[DrawnRooms.Count - 1])
                {
                    RoomIcons[1].enabled = true;
                }
            }
            //If you're adjacent to the current room become visable and be marked as an unexplored room
            else if (new Vector2 (PlayerCurrentRoom.x + 1, PlayerCurrentRoom.y) == MinimapCoordinates[i])
            {
                DrawnRooms[i].enabled = true;
                if (DrawnRooms[i] == DrawnRooms[DrawnRooms.Count - 2])
                {
                    RoomIcons[0].enabled = true;
                }
                else if (DrawnRooms[i] == DrawnRooms[DrawnRooms.Count - 1])
                {
                    RoomIcons[1].enabled = true;
                }
            }
            else if (new Vector2(PlayerCurrentRoom.x - 1, PlayerCurrentRoom.y) == MinimapCoordinates[i])
            {
                DrawnRooms[i].enabled = true;
                if (DrawnRooms[i] == DrawnRooms[DrawnRooms.Count - 2])
                {
                    RoomIcons[0].enabled = true;
                }
                else if (DrawnRooms[i] == DrawnRooms[DrawnRooms.Count - 1])
                {
                    RoomIcons[1].enabled = true;
                }
            }
            else if (new Vector2(PlayerCurrentRoom.x, PlayerCurrentRoom.y + 1) == MinimapCoordinates[i])
            {
                DrawnRooms[i].enabled = true;
                if (DrawnRooms[i] == DrawnRooms[DrawnRooms.Count - 2])
                {
                    RoomIcons[0].enabled = true;
                }
                else if (DrawnRooms[i] == DrawnRooms[DrawnRooms.Count - 1])
                {
                    RoomIcons[1].enabled = true;
                }
            }
            else if (new Vector2(PlayerCurrentRoom.x, PlayerCurrentRoom.y - 1) == MinimapCoordinates[i])
            {
                DrawnRooms[i].enabled = true;
                if (DrawnRooms[i] == DrawnRooms[DrawnRooms.Count - 2])
                {
                    RoomIcons[0].enabled = true;
                }
                else if (DrawnRooms[i] == DrawnRooms[DrawnRooms.Count - 1])
                {
                    RoomIcons[1].enabled = true;
                }
            }
        }
    }
}
