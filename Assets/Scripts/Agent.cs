using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Agent
{
    public static IndexPair CalculateMove(TileState[,] board, TileState color, int depth)
    {
        List<IndexPair> availableMoves = Judge.GetPlayableTiles(board, color);
        if (availableMoves.Count == 0)
        {
            return new IndexPair(board.GetLength(0), board.GetLength(1));
        }
        return availableMoves[UnityEngine.Random.Range(0, availableMoves.Count)];
    }

    public static float EvaluateBoard(TileState[,] board)
    {
        return 1f;
    }
}
