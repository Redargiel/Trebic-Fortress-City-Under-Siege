using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth = 16;
    public int gridHeight = 8;
    public int minPathLenght = 30;

    public GridCellObject[] pathCellObjects;
    public GridCellObject[] sceneryCellObjects;

    private PathGenerator pathGenerator;

    // Start is called before the first frame update
    void Start()
    {
        pathGenerator = new PathGenerator(gridWidth, gridHeight);

        List<Vector2Int> pathCells = pathGenerator.GeneratePath();
        int pathSize = pathCells.Count;

        while (pathSize < minPathLenght)
        {
            pathCells = pathGenerator.GeneratePath();
            pathSize = pathCells.Count;
        }
       
        StartCoroutine(CreateGrid(pathCells));
    }

    IEnumerator CreateGrid(List<Vector2Int> pathCells)
    {
        yield return LayPathCells(pathCells);
        yield return LaySceneryCells();
    }    

    private IEnumerator LayPathCells(List<Vector2Int> pathCells)
    {
        foreach (Vector2Int pathCell in pathCells)
        {
            int neighbourValue = pathGenerator.getCellNeighbourValue(pathCell.x, pathCell.y) - 1;
            Debug.Log("Title " + pathCell.x + ", " + pathCell.y + " neighbour value = " + neighbourValue);
            GameObject pathTile = pathCellObjects[neighbourValue].cellPrefab;
            GameObject pathTileCell = Instantiate(pathTile, new Vector3(pathCell.x, 0f, pathCell.y), Quaternion.identity);
            pathTileCell.transform.Rotate(0f, pathCellObjects[neighbourValue].yRotation, 0f, Space.Self);

            yield return new WaitForSeconds(0.1f);
        }

        yield return null;
    }

    IEnumerator LaySceneryCells()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (pathGenerator.CellIsEmpty(x,y))
                {
                    int randomSceneryCellIndex = Random.Range(0, sceneryCellObjects.Length);
                    Instantiate(sceneryCellObjects[randomSceneryCellIndex].cellPrefab, new Vector3(x, 0f, y), Quaternion.identity);
                    yield return new WaitForSeconds(0.025f);
                }
            }
        }

        yield return null;
    }
}
