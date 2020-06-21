using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_DungeonGenerator : MonoBehaviour
{
    public scr_DungeonGenerationData dungeonGenerationData;
    private List<Vector2Int> dungeonRooms;

    private void Start()
    {
        dungeonRooms = scr_DungeonCrawlerController.GenerateDungeon(dungeonGenerationData);
        SpawnRooms(dungeonRooms);
    }

    private void SpawnRooms(IEnumerable<Vector2Int> rooms)
    {
        scr_RoomController.instance.LoadRoom("Start", 0, 0);
        foreach(Vector2Int roomLocation in rooms)
        {
            //RandomString for differend Rooms :D
            scr_RoomController.instance.LoadRoom(scr_RoomController.instance.GetRandomRoomName(), roomLocation.x, roomLocation.y);
        }
    }
}
