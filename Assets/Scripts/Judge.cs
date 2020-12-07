using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Judge
{
    public static List<IndexPair> GetPlayableTiles(TileState[,] board, TileState colorCheck)
    {
        List<IndexPair> playableTiles = new List<IndexPair>();
        for (int i = 0; i < board.GetLength(1); i++)
        {
            for (int j = 0; j < board.GetLength(0); j++)
            {
                if (IsTilePlayable(board, new IndexPair(j, i), colorCheck)) playableTiles.Add(new IndexPair(j, i));
            }
        }
        return playableTiles;
    }

    public static bool IsBoardPlayable(TileState[,] board, TileState colorCheck)
    {
        for (int i = 0; i < board.GetLength(1); i++)
        {
            for (int j = 0; j < board.GetLength(0); j++)
            {
                if (IsTilePlayable(board, new IndexPair(j, i), colorCheck)) return true;
            }
        }
        return false;
    }
    public static bool IsTilePlayable(TileState[,] board, IndexPair index, TileState turnTo)
    {
        if (index.z >= board.GetLength(0)) return false;
        if (index.x >= board.GetLength(1)) return false;
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

    public static TileState[,] SimulateTurn(TileState[,] board, IndexPair index, TileState turnTo)
    {
        SimulateLane(board, index, turnTo, 0, 1, 1);
        SimulateLane(board, index, turnTo, 1, 1, 1);
        SimulateLane(board, index, turnTo, 1, 0, 1);
        SimulateLane(board, index, turnTo, 1, -1, 1);
        SimulateLane(board, index, turnTo, 0, -1, 1);
        SimulateLane(board, index, turnTo, -1, -1, 1);
        SimulateLane(board, index, turnTo, -1, 0, 1);
        SimulateLane(board, index, turnTo, -1, 1, 1);
        return board;
    }

    public static bool SimulateLane(TileState[,] board, IndexPair index, TileState turnTo, int directionZ, int directionX, int depth)
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
                if (board[z, x] == TileState.Empty)
                {
                    return false;
                }
                if (board[z, x] == TileState.Black && depth > 1)
                {
                    return true;
                }
                if (board[z, x] == TileState.White)
                {
                    if (SimulateLane(board, index, turnTo, directionZ, directionX, ++depth))
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
                    if (SimulateLane(board, index, turnTo, directionZ, directionX, ++depth))
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
}
