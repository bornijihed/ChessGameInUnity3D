using UnityEngine;

namespace Core
{
    public enum PieceType { None, Pawn, Rook, Knight, Bishop, Queen, King }
    public enum TeamColor { White, Black }

    public class ChessPiece : MonoBehaviour
    {
        public PieceType type;
        public TeamColor team;
        public Vector2Int currentPosition;

        public virtual void Init(TeamColor team, PieceType type)
        {
            this.team = team;
            this.type = type;
        }
    }
}
