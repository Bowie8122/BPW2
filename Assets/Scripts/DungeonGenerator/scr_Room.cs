using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Room : MonoBehaviour
{

    public int Width;
    public int Height;
    public int X;
    public int Y;

    private bool UpdatedDoors = false;
    public scr_Room(int x, int y)
    {
        X = x;
        Y = y;
    }

    public scr_Door LeftDoor;
    public scr_Door RightDoor;
    public scr_Door TopDoor;
    public scr_Door BottomDoor;

    public List<scr_Door> doors = new List<scr_Door>();

    // Start is called before the first frame update
    void Start()
    {
        if(scr_RoomController.instance == null)
        {
            Debug.Log("You pressed play in the wrong scene!");
            return;
        }

        //Get all the doors
        scr_Door[] ds = GetComponentsInChildren<scr_Door>();
        foreach(scr_Door d in ds)
        {
            doors.Add(d);
            switch(d.doorType)
            {
                case scr_Door.DoorType.right:
                    RightDoor = d;
                    break;
                case scr_Door.DoorType.left:
                    LeftDoor = d;
                    break;
                case scr_Door.DoorType.top:
                    TopDoor = d;
                    break;
                case scr_Door.DoorType.bottom:
                    BottomDoor = d;
                    break;
            }
        }

        scr_RoomController.instance.RegisterRoom(this);
    }

    void Update()
    {
        if(name.Contains("End") && !UpdatedDoors)
        {
            RemoveUnconnectedDoors();
            UpdatedDoors = true;
        }  
    }

    public void RemoveUnconnectedDoors()
    {
        foreach(scr_Door door in doors)
        {
            door.gameObject.SetActive(false);
            switch (door.doorType)
            {
                case scr_Door.DoorType.right:
                    if (GetRight() == null)
                        door.gameObject.SetActive(true);
                    break;
                case scr_Door.DoorType.left:
                    if (GetLeft() == null)
                        door.gameObject.SetActive(true);
                    break;
                case scr_Door.DoorType.top:
                    if (GetTop() == null)
                        door.gameObject.SetActive(true);
                    break;
                case scr_Door.DoorType.bottom:
                    if (GetBottom() == null)
                        door.gameObject.SetActive(true);
                    break;
            }
        }
    }

    public scr_Room GetRight()
    {
        if (scr_RoomController.instance.DoesRoomExist(X + 1, Y))
        {
            return scr_RoomController.instance.FindRoom(X + 1, Y);
        }
        return null;
    }
    public scr_Room GetLeft()
    {
        if (scr_RoomController.instance.DoesRoomExist(X - 1, Y))
        {
            return scr_RoomController.instance.FindRoom(X - 1, Y);
        }
        return null;
    }
    public scr_Room GetTop()
    {
        if (scr_RoomController.instance.DoesRoomExist(X, Y+1))
        {
            return scr_RoomController.instance.FindRoom(X, Y+1);
        }
        return null;
    }
    public scr_Room GetBottom()
    {
        if (scr_RoomController.instance.DoesRoomExist(X, Y-1))
        {
            return scr_RoomController.instance.FindRoom(X, Y-1);
        }
        return null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, 0));
    }
    public Vector3 GetRoomCenter()
    {
        return new Vector3(X * Width, Y * Height);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);
        if(other.tag == "Player")
        {
            scr_RoomController.instance.OnPlayerEnterRoom(this);
        }    
    }
}
