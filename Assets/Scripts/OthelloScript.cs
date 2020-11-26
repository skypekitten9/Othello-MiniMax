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
    TileState currentColor;
    int spacing;
    Vector3 origin;

    private void Awake()
    {
        origin = transform.position;
        currentColor = TileState.Black;
        SpawnBoard(origin, width, height);
    }
    void Start()
    {
        SpawnStartPawns(width, height);
    }

    private void Update()
    {
        IndexPair move = RequestMove(currentColor);
        if (MakeMove(currentColor, move))
        {
            switch (currentColor)
            {
                case TileState.Black:
                    currentColor = TileState.White;
                    break;
                case TileState.White:
                    currentColor = TileState.Black;
                    break;
                default:
                    Debug.LogError("Color is neither black or white.");
                    break;
            }
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            RefreshTiles();
        }
    }

    bool MakeMove(TileState color, IndexPair move)
    {
        if (Judge.IsTilePlayable(board, move, color))
        {
            board[move.z, move.x] = color;
            RefreshTiles();
            return true;
        }
        else return false;
    }

    IndexPair RequestMove(TileState currentColor)
    {
        switch (currentColor)
        {
            case TileState.Black:
                if (playerOneType == PlayerType.Agent) return RequestAgentMove(TileState.Black);
                else return RequestPlayerMove(TileState.Black);
            case TileState.White:
                if (playerOneType == PlayerType.Agent) return RequestAgentMove(TileState.White);
                else return RequestPlayerMove(TileState.White);
            default:
                Debug.LogError("Color is neither black or white.");
                return new IndexPair(width, height);
        }
        
    }

    IndexPair RequestPlayerMove(TileState color)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                return hit.transform.GetComponent<TileScript>().GetIndex();
            }
        }
        return new IndexPair(width, height);
    }

    IndexPair RequestAgentMove(TileState color)
    {
        return new IndexPair(width, height);
    }
    #region Turn Tiles
     void TurnTiles(IndexPair index, TileState turnTo)
    {
        TurnLane(index, turnTo, 0, 1, 1);
        TurnLane(index, turnTo, 1, 1, 1);
        TurnLane(index, turnTo, 1, 0, 1);
        TurnLane(index, turnTo, 1, -1, 1);
        TurnLane(index, turnTo, 0, -1, 1);
        TurnLane(index, turnTo, -1, -1, 1);
        TurnLane(index, turnTo, -1, 0, 1);
        TurnLane(index, turnTo, -1, 1, 1);
    }

    bool TurnLane(IndexPair index, TileState turnTo, int directionZ, int directionX, int depth)
    {
        if (index.z + (directionZ * depth) >= board.GetLength(0)) return false;
        if (index.x + (directionX * depth) >= board.GetLength(1)) return false;
        if (index.z + (directionZ * depth) < 0) return false;
        if (index.x + (directionX * depth) < 0) return false;
        Debug.Log(index.z + (directionZ * depth) + " " + index.x + (directionX * depth));
        switch (turnTo)
        {
            case TileState.Black:
                if (board[index.z + (directionZ * depth), index.x + (directionX * depth)] == TileState.Black && depth > 1) return true;
                else if (board[index.z + (directionZ * depth), index.x + (directionX * depth)] == TileState.White)
                {
                    if (TurnLane(index, turnTo, directionZ, directionX, ++depth))
                    {
                        board[index.z + (directionZ * depth), index.x + (directionX * depth)] = turnTo;
                        tileGameObjects[index.z + (directionZ * depth), index.x + (directionX * depth)].GetComponent<TileScript>().TurnTile(turnTo);
                        return true;
                    }
                    else return false;
                }
                else return false;
            case TileState.White:
                if (board[index.z + (directionZ * depth), index.x + (directionX * depth)] == TileState.White && depth > 1) return true;
                else if (board[index.z + (directionZ * depth), index.x + (directionX * depth)] == TileState.Black)
                {
                    if (TurnLane(index, turnTo, directionZ, directionX, ++depth))
                    {
                        board[index.z + (directionZ * depth), index.x + (directionX * depth)] = turnTo;
                        tileGameObjects[index.z + (directionZ * depth), index.x + (directionX * depth)].GetComponent<TileScript>().TurnTile(turnTo);
                        return true;
                    }
                    else return false;
                }
                else return false;
            case TileState.Empty:
                Debug.LogError("Empty tile should not be turned.");
                break;
            default:
                break;
        }
        return false;
    }
    #endregion
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

    void SpawnStartPawns(int width, int height)
    {
        board[(width / 2) - 1, (height / 2) - 1] = TileState.White;
        board[width / 2, height / 2] = TileState.White;
        board[width / 2, (height / 2) - 1] = TileState.Black;
        board[(width / 2) - 1, height / 2] = TileState.Black;
        RefreshTiles();

    }
    void RefreshTiles()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (tileGameObjects[j, i].transform.GetComponent<TileScript>().GetTileState() == TileState.Empty)
                {
                    tileGameObjects[j, i].transform.GetComponent<TileScript>().PlaceTile(board[j, i]);
                }
                else
                {
                    if (tileGameObjects[j, i].transform.GetComponent<TileScript>().GetTileState() != board[j, i]) tileGameObjects[j, i].transform.GetComponent<TileScript>().TurnTile(board[j, i]);
                }
            }
        }
        
    }
    #endregion
}
