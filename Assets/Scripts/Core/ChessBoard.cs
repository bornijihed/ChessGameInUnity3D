using UnityEngine;

namespace Core
{
    public class ChessBoard : MonoBehaviour
    {
        public const int TILE_COUNT_X = 8;
        public const int TILE_COUNT_Y = 8;
        private ChessPiece[,] chessPieces;

        private void Awake()
        {
            chessPieces = new ChessPiece[TILE_COUNT_X, TILE_COUNT_Y];
        }

        public ChessPiece GetPiece(int x, int y)
        {
            if (x < 0 || x >= TILE_COUNT_X || y < 0 || y >= TILE_COUNT_Y)
                return null;
            return chessPieces[x, y];
        }

        public void SetPiece(ChessPiece piece, int x, int y)
        {
            if (x < 0 || x >= TILE_COUNT_X || y < 0 || y >= TILE_COUNT_Y)
                return;
            
            chessPieces[x, y] = piece;
            if (piece != null)
            {
                piece.currentPosition = new Vector2Int(x, y);
            }
        }

        public bool IsPositionOnBoard(int x, int y)
        {
            return x >= 0 && x < TILE_COUNT_X && y >= 0 && y < TILE_COUNT_Y;
        }

        public bool MovePiece(int fromX, int fromY, int toX, int toY)
        {
            if (!IsPositionOnBoard(fromX, fromY) || !IsPositionOnBoard(toX, toY))
                return false;

            ChessPiece piece = GetPiece(fromX, fromY);
            if (piece == null) return false;

            // Validate move
            if (!MoveValidator.IsValidMove(this, fromX, fromY, toX, toY))
                return false;

            // Capture if target has opponent piece
            ChessPiece targetPiece = GetPiece(toX, toY);
            if (targetPiece != null)
            {
                Destroy(targetPiece.gameObject);
            }

            // Move the piece
            SetPiece(null, fromX, fromY);
            SetPiece(piece, toX, toY);

            return true;
        }

        public void SetupBoard()
        {
            // Clear old
            chessPieces = new ChessPiece[TILE_COUNT_X, TILE_COUNT_Y];
            
            // White
            for(int x=0; x<8; x++) SpawnLogicPiece(x, 1, PieceType.Pawn, TeamColor.White);
            SpawnLogicPiece(0, 0, PieceType.Rook, TeamColor.White);
            SpawnLogicPiece(1, 0, PieceType.Knight, TeamColor.White);
            SpawnLogicPiece(2, 0, PieceType.Bishop, TeamColor.White);
            SpawnLogicPiece(3, 0, PieceType.Queen, TeamColor.White);
            SpawnLogicPiece(4, 0, PieceType.King, TeamColor.White);
            SpawnLogicPiece(5, 0, PieceType.Bishop, TeamColor.White);
            SpawnLogicPiece(6, 0, PieceType.Knight, TeamColor.White);
            SpawnLogicPiece(7, 0, PieceType.Rook, TeamColor.White);

            // Black
            for(int x=0; x<8; x++) SpawnLogicPiece(x, 6, PieceType.Pawn, TeamColor.Black);
            SpawnLogicPiece(0, 7, PieceType.Rook, TeamColor.Black);
            SpawnLogicPiece(1, 7, PieceType.Knight, TeamColor.Black);
            SpawnLogicPiece(2, 7, PieceType.Bishop, TeamColor.Black);
            SpawnLogicPiece(3, 7, PieceType.Queen, TeamColor.Black);
            SpawnLogicPiece(4, 7, PieceType.King, TeamColor.Black);
            SpawnLogicPiece(5, 7, PieceType.Bishop, TeamColor.Black);
            SpawnLogicPiece(6, 7, PieceType.Knight, TeamColor.Black);
            SpawnLogicPiece(7, 7, PieceType.Rook, TeamColor.Black);
        }

        private void SpawnLogicPiece(int x, int y, PieceType type, TeamColor team)
        {
            GameObject go = new GameObject($"Logic_{team}_{type}");
            go.transform.parent = transform;
            ChessPiece cp = go.AddComponent<ChessPiece>();
            cp.Init(team, type);
            SetPiece(cp, x, y);
        }
    }
}
