using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Agent
{
    public static IndexPair CalculateMove(TileState[,] board, TileState color, int depth)
    {
        return new IndexPair(0, 0);
    }

    public static float EvaluateBoard(TileState[,] board)
    {
        return 1f;
    }
}
