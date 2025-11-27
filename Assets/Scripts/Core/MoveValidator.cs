using UnityEngine;

namespace Core
{
    public static class MoveValidator
    {
        public static bool IsValidMove(ChessBoard board, int fromX, int fromY, int toX, int toY)
        {
            ChessPiece piece = board.GetPiece(fromX, fromY);
            if (piece == null) return false;

            // Can't move to same position
            if (fromX == toX && fromY == toY) return false;

            // Can't capture your own piece
            ChessPiece targetPiece = board.GetPiece(toX, toY);
            if (targetPiece != null && targetPiece.team == piece.team) return false;

            // Check piece-specific movement
            switch (piece.type)
            {
                case PieceType.Pawn:
                    return IsValidPawnMove(board, piece, fromX, fromY, toX, toY);
                case PieceType.Rook:
                    return IsValidRookMove(board, fromX, fromY, toX, toY);
                case PieceType.Knight:
                    return IsValidKnightMove(fromX, fromY, toX, toY);
                case PieceType.Bishop:
                    return IsValidBishopMove(board, fromX, fromY, toX, toY);
                case PieceType.Queen:
                    return IsValidQueenMove(board, fromX, fromY, toX, toY);
                case PieceType.King:
                    return IsValidKingMove(fromX, fromY, toX, toY);
                default:
                    return false;
            }
        }

        private static bool IsValidPawnMove(ChessBoard board, ChessPiece pawn, int fromX, int fromY, int toX, int toY)
        {
            int direction = pawn.team == TeamColor.White ? 1 : -1;
            int deltaX = toX - fromX;
            int deltaY = toY - fromY;

            // Move forward one square
            if (deltaX == 0 && deltaY == direction)
            {
                return board.GetPiece(toX, toY) == null;
            }

            // Move forward two squares from starting position
            if (deltaX == 0 && deltaY == 2 * direction)
            {
                int startRow = pawn.team == TeamColor.White ? 1 : 6;
                if (fromY == startRow)
                {
                    return board.GetPiece(toX, toY) == null && 
                           board.GetPiece(fromX, fromY + direction) == null;
                }
            }

            // Capture diagonally
            if (Mathf.Abs(deltaX) == 1 && deltaY == direction)
            {
                ChessPiece target = board.GetPiece(toX, toY);
                return target != null && target.team != pawn.team;
            }

            return false;
        }

        private static bool IsValidRookMove(ChessBoard board, int fromX, int fromY, int toX, int toY)
        {
            // Rook moves in straight lines (horizontal or vertical)
            if (fromX != toX && fromY != toY) return false;

            return IsPathClear(board, fromX, fromY, toX, toY);
        }

        private static bool IsValidKnightMove(int fromX, int fromY, int toX, int toY)
        {
            // Knight moves in L-shape: 2 squares in one direction, 1 in perpendicular
            int deltaX = Mathf.Abs(toX - fromX);
            int deltaY = Mathf.Abs(toY - fromY);

            return (deltaX == 2 && deltaY == 1) || (deltaX == 1 && deltaY == 2);
        }

        private static bool IsValidBishopMove(ChessBoard board, int fromX, int fromY, int toX, int toY)
        {
            // Bishop moves diagonally
            int deltaX = Mathf.Abs(toX - fromX);
            int deltaY = Mathf.Abs(toY - fromY);

            if (deltaX != deltaY) return false;

            return IsPathClear(board, fromX, fromY, toX, toY);
        }

        private static bool IsValidQueenMove(ChessBoard board, int fromX, int fromY, int toX, int toY)
        {
            // Queen moves like rook or bishop
            return IsValidRookMove(board, fromX, fromY, toX, toY) ||
                   IsValidBishopMove(board, fromX, fromY, toX, toY);
        }

        private static bool IsValidKingMove(int fromX, int fromY, int toX, int toY)
        {
            // King moves one square in any direction
            int deltaX = Mathf.Abs(toX - fromX);
            int deltaY = Mathf.Abs(toY - fromY);

            return deltaX <= 1 && deltaY <= 1;
        }

        private static bool IsPathClear(ChessBoard board, int fromX, int fromY, int toX, int toY)
        {
            int deltaX = toX - fromX;
            int deltaY = toY - fromY;
            int steps = Mathf.Max(Mathf.Abs(deltaX), Mathf.Abs(deltaY));

            int stepX = deltaX == 0 ? 0 : deltaX / Mathf.Abs(deltaX);
            int stepY = deltaY == 0 ? 0 : deltaY / Mathf.Abs(deltaY);

            for (int i = 1; i < steps; i++)
            {
                int checkX = fromX + (stepX * i);
                int checkY = fromY + (stepY * i);

                if (board.GetPiece(checkX, checkY) != null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
