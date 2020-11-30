using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Agent
{
    public IndexPair CalculateMove(TileState[,] board, TileState color, int depth)
    {
        List<IndexPair> availableMoves = Judge.GetPlayableTiles(board, color);
        if (availableMoves.Count == 0)
        {
            return new IndexPair(board.GetLength(0), board.GetLength(1));
        }

        return availableMoves[UnityEngine.Random.Range(0, availableMoves.Count)];
    }

    

    public int MinMaxMove(TileState[,] board, TileState color, int depth, IndexPair move)
    {
        return 1;
    }
}

