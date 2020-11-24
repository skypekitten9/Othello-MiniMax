using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType { Human, Agent };
public class OthelloScript : MonoBehaviour
{
    TileState[,] board;
    GameObject[,] tileGameObjects;
    public GameObject tilePrefab;
    public PlayerType playerOneType, playerTwoType;
    public int width, height;
    bool mousePress;
    int spacing;
    Vector3 origin;

    private void Awake()
    {
        origin = transform.position;
        mousePress = false;
        SpawnBoard(origin, width, height);
    }
    void Start()
    {
        RequestMove(TileState.White);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) mousePress = true;
    }

    bool MakeMove(TileState color, IndexPair move)
    {
        if (Judge.IsTilePlayable(board, move, color))
        {
            tileGameObjects[move.z, move.x].GetComponent<TileScript>().TurnTile(color);
            board[move.z, move.x] = color;
            return true;
        }
        else return false;
    }

    void RequestMove(TileState previousColor)
    {
        switch (previousColor)
        {
            case TileState.Black:
                if (playerOneType == PlayerType.Agent) StartCoroutine(RequestAgentMove(TileState.White));
                else StartCoroutine(RequestPlayerMove(TileState.White));
                break;
            case TileState.White:
                if (playerTwoType == PlayerType.Agent) StartCoroutine(RequestAgentMove(TileState.Black));
                else StartCoroutine(RequestPlayerMove(TileState.Black));
                break;
            default:
                Debug.LogError("Color is neither black or white.");
                break;
        }
    }

    IEnumerator RequestPlayerMove(TileState color)
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            if (mousePress)
            {
                mousePress = false;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (MakeMove(color, hit.transform.GetComponent<TileScript>().GetIndex()))
                    {
                        RequestMove(color);
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator RequestAgentMove(TileState color)
    {
        Debug.Log("Requesting agent move.");
        do
        {

        } while (false);
        mousePress = false;
        yield return new WaitForEndOfFrame();
    }

    #region Player Settings
    public void PlayerVsPlayer()
    {
        playerOneType = PlayerType.Human;
        playerTwoType = PlayerType.Human;
    }

    public void PlayerVsAgent()
    {
        playerOneType = PlayerType.Agent;
        playerTwoType = PlayerType.Human;
    }

    public void AgentVsAgent()
    {
        playerOneType = PlayerType.Agent;
        playerTwoType = PlayerType.Agent;
    }
    #endregion
    #region Refresh & Spawn Board
    void SpawnBoard(Vector3 origin, int width, int height)
    {
        board = new TileState[width, height];
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
        board[zOffset, xOffset] = TileState.Empty;
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
                tileGameObjects[j, i].transform.GetComponent<TileScript>().TurnTile(board[j, i]);
            }
        }
    }
    #endregion
}
