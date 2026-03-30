using Firebase.Database;
using Unity.AI.Navigation;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public MapBlock[] mapGrid;
    [SerializeField] Transform Map;
    public NavMeshSurface navMeshSurface;

    [Header("Proceedural Map Debug")]
    [SerializeField] int mapSize = 5;
    [SerializeField] int mapCenter = 12;
    [SerializeField] int curMapIndex;
    [SerializeField] int branchLengthRemaining;

    [Header("Border Detection Test")]
    [SerializeField] bool up;
    [SerializeField] bool left;
    [SerializeField] bool down;
    [SerializeField] bool right;

    public static MapGenerator Instance { get; internal set; }

    private void OnEnable()
    {
        Instance = this;
    }

    private void Start()
    {
        mapGrid = Map.GetComponentsInChildren<MapBlock>();
        ClearMap();
        GenerateMap();
    }

    public void GenerateMap()
    {
        ClearMap();
        curMapIndex = mapCenter;
        mapGrid[curMapIndex].gameObject.SetActive(true);
        branchLengthRemaining = 3;
        for (int i = branchLengthRemaining; i>0; i--)
        {
            GenerateBranches();
        }
        navMeshSurface.BuildNavMesh();
    }

    public void GenerateBranches()
    {
        up    = false;
        left  = false;
        down  = false;
        right = false;

        int[] upConstraints    = {  0,  1,  2,  3,  4 };
        int[] leftConstraints  = {  0,  5, 10, 15, 20 };
        int[] downConstraints  = { 20, 21, 22, 23, 24 };
        int[] rightConstraints = {  4,  9, 14, 19, 24 };

        foreach (int index in upConstraints)
            if (curMapIndex == index)
            {
                Debug.Log("Up branch blocked by border constraint");
                up = true;
            }

        foreach (int index in leftConstraints)
            if (curMapIndex == index)
            {
                Debug.Log("Left branch blocked by border constraint");
                left = true;
            }

        foreach (int index in downConstraints)
            if (curMapIndex == index)
            {
                Debug.Log("Down branch blocked by border constraint");
                down = true;
            }

        foreach (int index in rightConstraints)
            if (curMapIndex == index)
            {
                Debug.Log("Right branch blocked by border constraint");
                right = true;
            }

        Debug.Log("Current Index: " + curMapIndex);
        if (!up)    up    = mapGrid[curMapIndex - mapSize].gameObject.activeSelf;
        if (!left)  left  = mapGrid[curMapIndex - 1      ].gameObject.activeSelf;
        if (!down)  down  = mapGrid[curMapIndex + mapSize].gameObject.activeSelf;
        if (!right) right = mapGrid[curMapIndex + 1      ].gameObject.activeSelf;



        int maxBranchChance=1;
        if (!up)    maxBranchChance++;
        if (!left)  maxBranchChance++;
        if (!down)  maxBranchChance++;
        if (!right) maxBranchChance++;
        if (maxBranchChance==1)
        {
            Debug.Log("No available branches");
            return;
        }

        bool posUp;
        bool posLeft;
        bool posDown;
        bool posRight;
        int direction;


        int branchChance = Random.Range(1, maxBranchChance);
        int i = branchChance;
        do
        {
            posUp = false;
            posLeft = false;
            posDown = false;
            posRight = false;
            direction = Random.Range(1, 5);
            int ranRoomVariant = Random.Range(0, mapGrid[curMapIndex].transform.childCount);
            switch (direction)
            {
                case 1:
                    if (!up)
                    {
                        mapGrid[curMapIndex - mapSize].gameObject.SetActive(true);
                        mapGrid[curMapIndex - mapSize].VariantIndex=ranRoomVariant;
                        posUp = true;
                        i--;
                    }
                    break;
                case 2:
                    if (!left)
                    {
                        mapGrid[curMapIndex - 1].gameObject.SetActive(true);
                        mapGrid[curMapIndex - 1].VariantIndex = ranRoomVariant;
                        posLeft = true;
                        i--;
                    }
                    break;
                case 3:
                    if (!down)
                    {
                        mapGrid[curMapIndex + mapSize].gameObject.SetActive(true);
                        mapGrid[curMapIndex + mapSize].VariantIndex = ranRoomVariant;
                        posDown = true;
                        i--;
                    }
                    break;
                case 4:
                    if (!right)
                    {
                        mapGrid[curMapIndex + 1].gameObject.SetActive(true);
                        mapGrid[curMapIndex + 1].VariantIndex = ranRoomVariant;
                        posRight = true;
                        i--;
                    }
                    break;
            }
        }
        while (i > 0);

        bool indexMoved = false;
        do
        {
            direction = Random.Range(1, 5);
            switch (direction)
            {
                case 1:
                    if (posUp)
                    {
                        curMapIndex -=mapSize;
                        indexMoved = true;
                    }
                    break;
                case 2:
                    if (posLeft)
                    {
                        curMapIndex--;
                        indexMoved = true;
                    }
                    break;
                case 3:
                    if (posDown)
                    {
                        curMapIndex +=mapSize;
                        indexMoved = true;
                    }
                    break;
                case 4:
                    if (posRight)
                    {
                        curMapIndex++;
                        indexMoved = true;
                    }
                    break;
            }
        }
        while (!indexMoved);
    }


    public void ClearMap()
    {
        curMapIndex = mapCenter;
        foreach (MapBlock block in mapGrid)
        {
            block.gameObject.SetActive(false);
        }
        mapGrid[curMapIndex].gameObject.SetActive(true);
        navMeshSurface.RemoveData();

        foreach (GameObject enemies in GameManager.Instance.EnemiesAlive)
        {
            Destroy(enemies);
        }
        GameManager.Instance.EnemiesAlive.Clear();

        foreach (GameObject items in GameManager.Instance.DroppedItems)
        {
            Destroy(items);
        }
        GameManager.Instance.DroppedItems.Clear();
    }

    public void SpawnEnemies()
    {
        foreach (MapBlock block in mapGrid)
        {
            if (block.isActiveAndEnabled)
            {
                block.SpawnEnemies();
            }
        }
    }
}
