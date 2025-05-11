using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 8;
    public int height = 8;
    public Gem[] gemPrefabs;
    public Transform gemParent;
    private Gem[,] grid;
    private Gem selectedGem;
    public GameObject selectedEffect;

    void Start()
    {
        grid = new Gem[width, height];
        InitializeGrid();
    }

    void InitializeGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SpawnGem(new Vector2Int(x, y));
            }
        }
    }

    void SpawnGem(Vector2Int position)
    {
        int randomIndex = Random.Range(0, gemPrefabs.Length);

        Gem gem = Instantiate(gemPrefabs[randomIndex], gemParent);
        grid[position.x, position.y] = gem;
        gem.SetPosition(position);
    }

    bool CheckForMatches()
    {
        bool foundMatch = false;
    
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x < width - 2 && grid[x, y] != null && grid[x + 1, y] != null && grid[x + 2, y] != null)
                {
                    if (grid[x, y].name == grid[x + 1, y].name && grid[x, y].name == grid[x + 2, y].name)
                    {
                        Destroy(grid[x, y].gameObject);
                        Destroy(grid[x + 1, y].gameObject);
                        Destroy(grid[x + 2, y].gameObject);
                        grid[x, y] = null;
                        grid[x + 1, y] = null;
                        grid[x + 2, y] = null;
                        foundMatch = true;
                    }
                }
    
                if (y < height - 2 && grid[x, y] != null && grid[x, y + 1] != null && grid[x, y + 2] != null)
                {
                    if (grid[x, y].name == grid[x, y + 1].name && grid[x, y].name == grid[x, y + 2].name)
                    {
                        Destroy(grid[x, y].gameObject);
                        Destroy(grid[x, y + 1].gameObject);
                        Destroy(grid[x, y + 2].gameObject);
                        grid[x, y] = null;
                        grid[x, y + 1] = null;
                        grid[x, y + 2] = null;
                        foundMatch = true;
                    }
                }
            }
        }
    
        return foundMatch;
    }

    void MakeGemsFall()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == null)
                {
                    for (int aboveY = y + 1; aboveY < height; aboveY++)
                    {
                        if (grid[x, aboveY] != null)
                        {
                            grid[x, y] = grid[x, aboveY];
                            grid[x, aboveY] = null;
                            grid[x, y].SetPosition(new Vector2Int(x, y));
                            break;
                        }
                    }
                }
            }
        }

        SpawnNewGems();
    }

    private void SpawnNewGems()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == null)
                {
                    SpawnGem(new Vector2Int(x, y));
                }
            }
        }
    }

    public bool CheckForPotentialMatch(Gem gem1, Gem gem2)
    {
        SwapGems(gem1, gem2);
        bool hasMatch = CheckForMatches();
        SwapGems(gem1, gem2); // Swap back if no match
        return hasMatch;
    }

    public void SelectGem(Gem gem)
    {
        if (selectedGem == null)
        {
            selectedGem = gem;
            selectedEffect.SetActive(true);
            selectedEffect.transform.position = gem.transform.position;
        }
        else
        {
            if (AreAdjacent(selectedGem, gem))
            {
                if (CheckForPotentialMatch(selectedGem, gem))
                {
                    SwapGems(selectedGem, gem);
                    CheckForMatches();
                    MakeGemsFall();
                }
            }
            selectedGem = null;
            selectedEffect.SetActive(false);
        }
    }

    bool AreAdjacent(Gem gem1, Gem gem2)
    {
        return (Mathf.Abs(gem1.position.x - gem2.position.x) == 1 && gem1.position.y == gem2.position.y) ||
               (Mathf.Abs(gem1.position.y - gem2.position.y) == 1 && gem1.position.x == gem2.position.x);
    }

    void SwapGems(Gem gem1, Gem gem2)
    {
        Vector2Int temporaryPosition = gem1.position;

        gem1.SetPosition(gem2.position);
        gem2.SetPosition(temporaryPosition);

        grid[gem1.position.x, gem1.position.y] = gem1;
        grid[gem2.position.x, gem2.position.y] = gem2;
    }

    public Gem GetGemAt(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return grid[x, y];
        }
        return null;
    }
}