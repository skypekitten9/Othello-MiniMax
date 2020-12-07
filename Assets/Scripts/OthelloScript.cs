using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerType { Human, Agent };
public class OthelloScript : MonoBehaviour
{
    TileState[,] board;
    GameObject[,] tileGameObjects;
    public GameObject tilePrefab;
    public PlayerType playerOneType, playerTwoType;
    public int width, height;
    public float agentDelay;
    float agentTimer;
    bool reset, win;
    TileState currentColor;
    Vector3 origin;

    private void Awake()
    {
        agentTimer = agentDelay;
        reset = false;
        win = false;
        origin = transform.position;
        currentColor = TileState.Black;
        SpawnBoard(origin, width, height);
        SpawnRefferencesUI(width, height);
    }
    void Start()
    {
        SpawnStartPawns(width, height);
    }

    private void Update()
    {
        if (DetermineWin() && !win)
        {
            HandleWin();
        }
        if (reset)
        {
            ResetBoard();
        }
        if (!win)
        {
            if (!Judge.IsBoardPlayable(board, currentColor))
            {
                Debug.Log("Skipped!");
                SwitchColor(currentColor);
            }
            IndexPair move = RequestMove(currentColor);
            if (MakeMove(currentColor, move))
            {
                SwitchColor(currentColor);
            }
        }
    }
    #region Debug
    void PrintBoard()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Debug.Log(board[j, i]);
            }
        }
    }
    #endregion
    #region Conditions
    void HandleWin()
    {
        win = true;
        int blackCount = 0;
        int whiteCount = 0;
        SFXManager.Instance.PlayWin();
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (board[j, i] == TileState.Black) blackCount++;
                if (board[j, i] == TileState.White) whiteCount++;
            }
        }

        if (blackCount > whiteCount)
        {
            UI_Script.Instance.DisplayWin(TileState.Black);
        }
        else if(whiteCount > blackCount)
        {
            UI_Script.Instance.DisplayWin(TileState.White);
        }
        else
        {
            UI_Script.Instance.DisplayWin(TileState.Empty);
        }
    }
    bool DetermineWin()
    {
        if (!Judge.IsBoardPlayable(board, TileState.Black) && !Judge.IsBoardPlayable(board, TileState.White))
        {
            return true;
        }
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (board[j, i] == TileState.Empty) return false;
            }
        }
        return true;
    }
    #endregion
    #region Make & Request Moves
    bool MakeMove(TileState color, IndexPair move)
    {
        if (Judge.IsTilePlayable(board, move, color))
        {
            board[move.z, move.x] = color;
            tileGameObjects[move.z, move.x].transform.GetComponent<TileScript>().PlaceTile(color, true);
            TurnTiles(move, color);
            RefreshTiles();
            agentTimer = agentDelay;
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
                if (playerTwoType == PlayerType.Agent) return RequestAgentMove(TileState.White);
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
        agentTimer -= Time.deltaTime;
        if (agentTimer < 0)
        {
            return Agent.CalculateMove(board, color, 10);
        }
        else return new IndexPair(width, height);
    }

    void ChangeColor(TileState changeToColor)
    {
        UI_Script.Instance.DisplayTurn(changeToColor);
        currentColor = changeToColor;
    }

    void SwitchColor(TileState switchFromColor)
    {
        switch (switchFromColor)
        {
            case TileState.Black:
                ChangeColor(TileState.White);
                break;
            case TileState.White:
                ChangeColor(TileState.Black);
                break;
            default:
                Debug.LogError("Color is neither black or white.");
                break;
        }
    }
    #endregion
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
        
        int z = index.z + (directionZ * depth);
        int x = index.x + (directionX * depth);
        if (z >= board.GetLength(0)) return false;
        if (x >= board.GetLength(1)) return false;
        if (z < 0) return false;
        if (x < 0) return false;
        switch (turnTo)
        {
            case TileState.Black:
                if(board[z, x] == TileState.Empty)
                {
                    return false;
                }
                if(board[z, x] == TileState.Black && depth > 1)
                {
                    return true;
                }
                if (board[z, x] == TileState.White)
                {
                    if(TurnLane(index, turnTo, directionZ, directionX, ++depth))
                    {
                        board[z, x] = TileState.Black;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                break;
            case TileState.White:
                if (board[z, x] == TileState.Empty)
                {
                    return false;
                }
                if (board[z, x] == TileState.White && depth > 1)
                {
                    return true;
                }
                if (board[z, x] == TileState.Black)
                {
                    if (TurnLane(index, turnTo, directionZ, directionX, ++depth))
                    {
                        board[z, x] = TileState.White;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                break;
            case TileState.Empty:
                Debug.LogError("Empty tile should not be tested as playable.");
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
                    tileGameObjects[j, i].transform.GetComponent<TileScript>().PlaceTile(board[j, i], false);
                }
                else
                {
                    if (tileGameObjects[j, i].transform.GetComponent<TileScript>().GetTileState() != board[j, i]) tileGameObjects[j, i].transform.GetComponent<TileScript>().TurnTile(board[j, i]);
                }
            }
        }
    }

    void SpawnRefferencesUI(int width, int height)
    {
        Vector3 top = new Vector3(-1, 0, (width/2) - 0.5f);
        Vector3 bottom = new Vector3(height, 0, (width / 2) - 0.5f);
        Vector3 left = new Vector3((height/2) - 0.5f, 0, -1f);
        Vector3 right = new Vector3((height / 2) - 0.5f, 0, width);
        UI_Script.Instance.SetRefferences(top, bottom, left, right);
    }

    void ResetBoard()
    {
        ChangeColor(TileState.Black);
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                board[j, i] = TileState.Empty;
            }
        }
        agentTimer = agentDelay;
        reset = false;
        win = false;
        SpawnStartPawns(width, height);
    }

    public void RequestReset()
    {
        reset = true;
    }
    #endregion
}
