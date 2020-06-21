using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class RoomInfo
{
    public string name;
    public int X;
    public int Y;
}
public class scr_RoomController : MonoBehaviour
{
    public static scr_RoomController instance;
    string currentWorldName = "Overworld";
    RoomInfo currentLoadRoomData;
    scr_Room currRoom;

    Queue<RoomInfo> loadRoomQue = new Queue<RoomInfo>();
    public List<scr_Room> loadedRooms = new List<scr_Room>();

    bool isLoadingRoom = false;
    bool spawnedBossRoom = false;
    bool updatedRooms = false;

    void Awake()
    {
        instance = this;   
    }
    void Start()
    {
        //LoadRoom("Start",0,0);
        //LoadRoom("Empty", 1, 0);
       //LoadRoom("Empty", -1, 0);
        //LoadRoom("Empty", 0, 1);
        //LoadRoom("Empty", -1, 1);
    }

    void Update()
    {
        UpdateRoomQueue();   
    }

    void UpdateRoomQueue()
    {
        //If already loading a room, Exit out
        if (isLoadingRoom)
        {
            return;
        }
        //Als de Queue leeg is, Exit out
        if (loadRoomQue.Count == 0)
        {
            if (!spawnedBossRoom)
            {
                StartCoroutine(SpawnBossRoom());
            } else if (spawnedBossRoom && !updatedRooms)
            {
                foreach(scr_Room room in loadedRooms)
                {
                    room.RemoveUnconnectedDoors();
                }
                updateRooms();
                updatedRooms = true;
            }
            return;
        }
        //Load a room
        currentLoadRoomData = loadRoomQue.Dequeue();
        isLoadingRoom = true;
        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }
    IEnumerator SpawnBossRoom()
    {
        spawnedBossRoom = true;
        yield return new WaitForSeconds(.5f);
        if(loadRoomQue.Count == 0)
        {
            scr_Room bossRoom = loadedRooms[loadedRooms.Count - 1];
            scr_Room tempRoom = new scr_Room(bossRoom.X, bossRoom.Y);
            Destroy(bossRoom.gameObject);
            var roomToRemove = loadedRooms.Single(r => r.X == tempRoom.X && r.Y == tempRoom.Y);
            loadedRooms.Remove(roomToRemove);
            LoadRoom("End", tempRoom.X, tempRoom.Y);
        }
    }
    public void LoadRoom(string name, int x, int y)
    {
        //Check of the room al bestaad
        if (DoesRoomExist(x, y))
        {
            return;
        }
        //Nieuwe Room Maken
        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.X = x;
        newRoomData.Y = y;

        loadRoomQue.Enqueue(newRoomData);
    }

    IEnumerator LoadRoomRoutine(RoomInfo info)
    {
        string roomName = currentWorldName+"_" + info.name +"Room";
        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while(loadRoom.isDone == false)
        {
            yield return null;
        }
    }

    public void RegisterRoom(scr_Room room)
    {
        //Check if room already exists here
        if(!DoesRoomExist(currentLoadRoomData.X, currentLoadRoomData.Y)) {
            room.transform.position = new Vector3(currentLoadRoomData.X*room.Width,currentLoadRoomData.Y*room.Height,0);
            room.X = currentLoadRoomData.X;
            room.Y = currentLoadRoomData.Y;
            room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.X + "," + room.Y;
            room.transform.parent = transform;

            isLoadingRoom = false;

            if(loadedRooms.Count == 0)
            {
                scr_CameraController.instance.currRoom = room;
            }

            loadedRooms.Add(room);
        }
        else
        {
            //Room already exists, remove it
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }

    public bool DoesRoomExist(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y) != null;
    }

    public scr_Room FindRoom(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y);
    }

    public string GetRandomRoomName()
    {
        string[] possibleRooms = new string[]
        {
            "Empty",
            "Basic"
        };

        return possibleRooms[Random.Range(0, possibleRooms.Length)];
    }

    public void OnPlayerEnterRoom(scr_Room room)
    {
        scr_CameraController.instance.currRoom = room;
        currRoom = room;

        updateRooms();
    }

    //"Disable" enemys that arent in the same room
    private void updateRooms()
    {
        foreach(scr_Room room in loadedRooms)
        {
            if (currRoom != room)
            {
                scr_EnemyController[] enemies = room.GetComponentsInChildren<scr_EnemyController>();
                if (enemies != null)
                {
                    foreach (scr_EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = true;
                    }
                }
            }
            else
            {
                scr_EnemyController[] enemies = room.GetComponentsInChildren<scr_EnemyController>();
                if (enemies != null)
                {
                    foreach (scr_EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = false;
                    }
                }
            }
        }
    }
}
