using UnityEngine;

namespace Core
{
    public static class PieceValues
    {
        public static int GetValue(PieceType type)
        {
            switch (type)
            {
                case PieceType.Pawn: return 1;
                case PieceType.Knight: return 3;
                case PieceType.Bishop: return 3;
                case PieceType.Rook: return 5;
                case PieceType.Queen: return 9;
                case PieceType.King: return 0; // King is priceless
                default: return 0;
            }
        }
    }
}
