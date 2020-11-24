using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OthelloScript : MonoBehaviour
{
    TileState[,] tileStates;
    GameObject[,] tileGameObjects;
    public GameObject tilePrefab;
    public int width, height;
    int spacing;
    Vector3 origin;

    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
        SpawnBoard(origin, width, height);
        StartCoroutine(RequestPlayerMove());
    }

    void Update()
    {
            
    }

    IEnumerator RequestPlayerMove()
    {
        while(true)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    hit.transform.GetComponent<TileScript>().TurnTile(TileState.Black);
                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    #region Refresh & Spawn Board
    void SpawnBoard(Vector3 origin, int width, int height)
    {
        tileStates = new TileState[width, height];
        tileGameObjects = new GameObject[width, height];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                SpawnTile(i, j);
            }
        }
    }

    void SpawnTile(int xOffset, int zOffset)
    {
        Vector3 offset = new Vector3(xOffset, 0, zOffset);
        tileStates[zOffset, xOffset] = TileState.Empty;
        GameObject tile = Instantiate(tilePrefab, gameObject.transform);
        tile.transform.position += offset;
        tile.GetComponent<TileScript>().SetIndex(zOffset, xOffset);
        tileGameObjects[zOffset, xOffset] = tile;
    }

    void UpdateTiles()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                tileGameObjects[j, i].transform.GetComponent<TileScript>().TurnTile(tileStates[j, i]);
            }
        }
    }
    #endregion
}
