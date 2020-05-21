using UnityEngine;
using UnityEngine.Tilemaps;

public class  DungeonGenerator : MonoBehaviour {
        
    [SerializeField]
    private Tile groundTile;

    [SerializeField]
    private Tile wallTile;

    [SerializeField]
    private Tile iconGroundTile;

    [SerializeField]
    private Tile iconWallTile;

    [SerializeField]
    private Tilemap groundMap;

    [SerializeField]
    private Tilemap wallMap;

    [SerializeField]
    private Tilemap iconGroundMap;

    [SerializeField]
    private Tilemap iconWallMap;

    [SerializeField]
    private int deviationRate = 10;

    [SerializeField]
    private int roomRate = 15;

    [SerializeField]
    private int maxRouteLength;

    [SerializeField]
    private int maxRoutes = 20;

    [SerializeField]
    private int numberOfSpawners = 8;

    public Transitioner transitioner;

    public GameObject spawner;

    private int routeCount = 0;

    private void Awake() {
        transitioner = GameObject.FindGameObjectWithTag("Transitioner").GetComponent<Transitioner>();
        int x = 0;
        int y = 0;
        int routeLength = 0;
        GenerateSquare(x, y, 1);
        Vector2Int previousPos = new Vector2Int(x, y);
        y += 3;
        GenerateSquare(x, y, 1);
        NewRoute(x, y, routeLength, previousPos);
        FillWalls();
    }

    private void FillWalls() {
        BoundsInt bounds = groundMap.cellBounds;
        for (int xMap = bounds.xMin - 50; xMap <= bounds.xMax + 50; xMap++) {
            for (int yMap = bounds.yMin - 50; yMap <= bounds.yMax + 50; yMap++) {

                Vector3Int pos = new Vector3Int(xMap, yMap, 0);
                Vector3Int posBelow = new Vector3Int(xMap, yMap - 1, 0);
                Vector3Int posAbove = new Vector3Int(xMap, yMap + 1, 0);
                Vector3Int posRight = new Vector3Int(xMap + 1, yMap, 0);
                Vector3Int posLeft = new Vector3Int(xMap - 1, yMap, 0);
                
                TileBase tile = groundMap.GetTile(pos);
                TileBase tileBelow = groundMap.GetTile(posBelow);
                TileBase tileAbove = groundMap.GetTile(posAbove);
                TileBase tileRight = groundMap.GetTile(posRight);
                TileBase tileLeft = groundMap.GetTile(posLeft);
                
                if (tile == null) {
                    if (tileBelow != null) {
                        wallMap.SetTile(pos, wallTile);
                        iconWallMap.SetTile(pos, iconWallTile); 
                    } else if (tileAbove != null) {
                        wallMap.SetTile(pos, wallTile);
                        iconWallMap.SetTile(pos, iconWallTile);
                    } else if (tileRight != null) {
                        wallMap.SetTile(pos, wallTile);
                        iconWallMap.SetTile(pos, iconWallTile); 
                    } else if (tileLeft != null) {
                        wallMap.SetTile(pos, wallTile);
                        iconWallMap.SetTile(pos, iconWallTile); 
                    }
                }
            }
        }
    }

    private void NewRoute(int x, int y, int routeLength, Vector2Int previousPos) {
        if (routeCount < maxRoutes) {
            routeCount++;
            while (++routeLength < maxRouteLength) {
                //Initialize
                bool routeUsed = false;
                int xOffset = x - previousPos.x; //0
                int yOffset = y - previousPos.y; //3
                int roomSize = 1; //Hallway size
                if (Random.Range(1, 100) <= roomRate)
                    roomSize = Random.Range(3, 6);
                previousPos = new Vector2Int(x, y);

                //Go Straight
                if (Random.Range(1, 100) <= deviationRate) {
                    if (routeUsed) {
                        GenerateSquare(previousPos.x + xOffset, previousPos.y + yOffset, roomSize);
                        NewRoute(previousPos.x + xOffset, previousPos.y + yOffset, Random.Range(routeLength, maxRouteLength), previousPos);

                        if (roomSize > 3) {
                            GameObject s = Instantiate(spawner, new Vector2(x, y), Quaternion.identity);
                            transitioner.spawners.Add(s);
                        }
                    } else {
                        x = previousPos.x + xOffset;
                        y = previousPos.y + yOffset;
                        GenerateSquare(x, y, roomSize);
                        routeUsed = true;
                        if (roomSize > 3) {
                            GameObject s = Instantiate(spawner, new Vector2(x, y), Quaternion.identity);
                            transitioner.spawners.Add(s);
                        }
                    }
                }

                //Go left
                if (Random.Range(1, 100) <= deviationRate) {
                    if (routeUsed) {
                        GenerateSquare(previousPos.x - yOffset, previousPos.y + xOffset, roomSize);
                        NewRoute(previousPos.x - yOffset, previousPos.y + xOffset, Random.Range(routeLength, maxRouteLength), previousPos);
                        if (roomSize > 3) {
                            GameObject s = Instantiate(spawner, new Vector2(x, y), Quaternion.identity);
                            transitioner.spawners.Add(s);
                        }
                    } else {
                        y = previousPos.y + xOffset;
                        x = previousPos.x - yOffset;
                        GenerateSquare(x, y, roomSize);
                        routeUsed = true;
                        if (roomSize > 3) {
                            GameObject s = Instantiate(spawner, new Vector2(x, y), Quaternion.identity);
                            transitioner.spawners.Add(s);
                        }
                    }
                }
                //Go right
                if (Random.Range(1, 100) <= deviationRate) {
                    if (routeUsed) {
                        GenerateSquare(previousPos.x + yOffset, previousPos.y - xOffset, roomSize);
                        NewRoute(previousPos.x + yOffset, previousPos.y - xOffset, Random.Range(routeLength, maxRouteLength), previousPos);
                        if (roomSize > 3) {
                            GameObject s = Instantiate(spawner, new Vector2(x, y), Quaternion.identity);
                            transitioner.spawners.Add(s);
                        }
                    } else {
                        y = previousPos.y - xOffset;
                        x = previousPos.x + yOffset;
                        GenerateSquare(x, y, roomSize);
                        routeUsed = true;
                        if (roomSize > 3) {
                            GameObject s = Instantiate(spawner, new Vector2(x, y), Quaternion.identity);
                            transitioner.spawners.Add(s);
                        }
                    }
                }

                if (!routeUsed) {
                    x = previousPos.x + xOffset;
                    y = previousPos.y + yOffset;
                    GenerateSquare(x, y, roomSize);
                    if (roomSize > 3    ) {
                        GameObject s = Instantiate(spawner, new Vector2(x, y), Quaternion.identity);
                        transitioner.spawners.Add(s);
                    }
                }
            }
        }
    }

    private void GenerateSquare(int x, int y, int radius) {
        for (int tileX = x - radius; tileX <= x + radius; tileX++) {
            for (int tileY = y - radius; tileY <= y + radius; tileY++) {
                Vector3Int pos = new Vector3Int(tileX, tileY, 0);
                groundMap.SetTile(pos, groundTile);
                iconGroundMap.SetTile(pos, iconGroundTile);
            }
        }
    }

}
