using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Judge
{
    public static bool IsTilePlayable(TileState[,] tileStates, int z, int x, TileState turnTo)
    {
        if(tileStates[z, x] == TileState.Empty)
        {
            return true;
        }
        return false;
    }
}
