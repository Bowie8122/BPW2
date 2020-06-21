using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    up = 0,
    left = 1,
    down = 2,
    right = 3
};

public class scr_DungeonCrawlerController : MonoBehaviour
{
    //Dit allows to convert a Vector to a Int
    public static List<Vector2Int> positionsVisited = new List<Vector2Int>();
    private static readonly Dictionary<Direction, Vector2Int> directionMovementMap = new Dictionary<Direction, Vector2Int>
    {
        {Direction.up, Vector2Int.up },
        {Direction.left, Vector2Int.left },
        {Direction.down, Vector2Int.down },
        {Direction.right, Vector2Int.right }
    };

    public static List<Vector2Int> GenerateDungeon(scr_DungeonGenerationData dungeonData)
    {
        List<scr_DungeonCrawler> dungeonCrawlers = new List<scr_DungeonCrawler>();

        for (int i = 0; i < dungeonData.numberOfCrawlers; i++)
        {
            dungeonCrawlers.Add(new scr_DungeonCrawler(Vector2Int.zero));
        }

        //Set iterations
        int iteractions = Random.Range(dungeonData.iterationMin, dungeonData.iterationMax);

        for (int i = 0; i < iteractions; i++)
        {
            foreach(scr_DungeonCrawler dungeonCrawler in dungeonCrawlers)
            {
                Vector2Int newPos = dungeonCrawler.Move(directionMovementMap);
                positionsVisited.Add(newPos);
            }
        }

        return positionsVisited;
    }
}
