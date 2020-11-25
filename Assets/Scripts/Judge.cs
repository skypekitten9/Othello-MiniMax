using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Judge
{
    public static bool IsTilePlayable(TileState[,] board, IndexPair index, TileState turnTo)
    {
        if (board[index.z, index.x] != TileState.Empty) return false;
        if (IsTilePlayable(board, index, turnTo, 0, 1, 1)) return true;
        if (IsTilePlayable(board, index, turnTo, 1, 1, 1)) return true;
        if (IsTilePlayable(board, index, turnTo, 1, 0, 1)) return true;
        if (IsTilePlayable(board, index, turnTo, 1, -1, 1)) return true;
        if (IsTilePlayable(board, index, turnTo, 0, -1, 1)) return true;
        if (IsTilePlayable(board, index, turnTo, -1, -1, 1)) return true;
        if (IsTilePlayable(board, index, turnTo, -1, 0, 1)) return true;
        if (IsTilePlayable(board, index, turnTo, -1, 1, 1)) return true;
        return false;
    }

    public static bool IsTilePlayable(TileState[,] board, IndexPair index, TileState turnTo, int directionZ, int directionX, int depth)
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
                    return IsTilePlayable(board, index, turnTo, directionZ, directionX, ++depth);
                }
                else return false;
            case TileState.White:
                if (board[index.z + (directionZ * depth), index.x + (directionX * depth)] == TileState.White && depth > 1) return true;
                else if (board[index.z + (directionZ * depth), index.x + (directionX * depth)] == TileState.Black)
                {
                    return IsTilePlayable(board, index, turnTo, directionZ, directionX, ++depth);
                }
                else return false;
            case TileState.Empty:
                Debug.LogError("Empty tile should not be tested as playable.");
                break;
            default:
                break;
        }
        return false;
    }

    public static List<IndexPair> GetPlayableTiles(TileState[,] board)
    {
        return new List<IndexPair>();
    }
}
