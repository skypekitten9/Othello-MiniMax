using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Judge
{
    public static bool IsTilePlayable(TileState[,] board, IndexPair index, TileState turnTo)
    {
        if(board[index.z, index.x] == TileState.Empty)
        {
            return true;
        }
        return false;
    }

    public static List<IndexPair> GetPlayableTiles(TileState[,] board)
    {
        return new List<IndexPair>();
    }
}
