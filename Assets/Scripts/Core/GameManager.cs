using UnityEngine;
using System.Collections.Generic;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public ChessBoard board;
        
        public Visuals.BoardVisualizer visualizer;
        public TeamColor currentTurn = TeamColor.White;
        
        public int whiteScore = 0;
        public int blackScore = 0;
        public int whiteMoves = 0;
        public int blackMoves = 0;
        
        private TextMesh scoreText;
        private GameObject scoreDisplayObject;
        
        // Timer
        private TextMesh timerText;
        private GameObject timerDisplayObject;
        private float whiteTime = 0f;
        private float blackTime = 0f;
        private float currentTurnStartTime = 0f;
        
        // Pawn promotion
        private bool waitingForPromotion = false;
        private int promotionX;
        private int promotionY;
        private TeamColor promotionTeam;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            if (board == null) board = gameObject.AddComponent<ChessBoard>();
            if (visualizer == null) visualizer = gameObject.AddComponent<Visuals.BoardVisualizer>();
            
            // Ensure InputController exists
            if (GetComponent<InputController>() == null) gameObject.AddComponent<InputController>();

            CreateScoreDisplay();
            CreateTimerDisplay();
            
            // Add rhythmic background
            gameObject.AddComponent<Visuals.RhythmicBackground>();
            
            StartGame();
        }

        private void CreateScoreDisplay()
        {
            scoreDisplayObject = new GameObject("ScoreDisplay");
            scoreDisplayObject.transform.parent = transform;
            
            scoreText = scoreDisplayObject.AddComponent<TextMesh>();
            scoreText.text = "White: 0 (0 moves)\nBlack: 0 (0 moves)";
            scoreText.fontSize = 50;
            scoreText.characterSize = 0.1f;
            scoreText.anchor = TextAnchor.MiddleCenter; // Center aligned
            scoreText.color = Color.yellow;
            scoreText.GetComponent<MeshRenderer>().material.color = Color.yellow;
            
            // Set initial position for White's turn
            UpdateScorePosition(TeamColor.White);
        }

        private void UpdateScorePosition(TeamColor currentPlayer)
        {
            if (scoreDisplayObject == null) return;
            
            if (currentPlayer == TeamColor.White)
            {
                // White's turn: score on Black's side (far from White's camera), centered
                scoreDisplayObject.transform.position = new Vector3(3.5f, 2f, 9.5f);
                scoreDisplayObject.transform.rotation = Quaternion.Euler(0, 0, 0); // Face forward (towards White)
            }
            else
            {
                // Black's turn: score on White's side (far from Black's camera), centered
                scoreDisplayObject.transform.position = new Vector3(3.5f, 2f, -2.5f);
                scoreDisplayObject.transform.rotation = Quaternion.Euler(0, 180, 0); // Face backward (towards Black)
            }
        }

        private void CreateTimerDisplay()
        {
            timerDisplayObject = new GameObject("TimerDisplay");
            timerDisplayObject.transform.parent = transform;
            
            timerText = timerDisplayObject.AddComponent<TextMesh>();
            timerText.text = "White: 0:00\nBlack: 0:00";
            timerText.fontSize = 40;
            timerText.characterSize = 0.08f;
            timerText.anchor = TextAnchor.MiddleCenter;
            timerText.color = Color.cyan;
            timerText.GetComponent<MeshRenderer>().material.color = Color.cyan;
            
            // Set initial position for White's turn
            UpdateTimerPosition(TeamColor.White);
        }

        private void UpdateTimerPosition(TeamColor currentPlayer)
        {
            if (timerDisplayObject == null) return;
            
            if (currentPlayer == TeamColor.White)
            {
                // White's turn: timer beside score on Black's side
                timerDisplayObject.transform.position = new Vector3(3.5f, 2.8f, 9.5f);
                timerDisplayObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                // Black's turn: timer beside score on White's side
                timerDisplayObject.transform.position = new Vector3(3.5f, 2.8f, -2.5f);
                timerDisplayObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        private void UpdateTimerDisplay()
        {
            int whiteMinutes = (int)(whiteTime / 60f);
            int whiteSeconds = (int)(whiteTime % 60f);
            int blackMinutes = (int)(blackTime / 60f);
            int blackSeconds = (int)(blackTime % 60f);
            
            timerText.text = $"White: {whiteMinutes}:{whiteSeconds:D2}\nBlack: {blackMinutes}:{blackSeconds:D2}";
        }

        private void Update()
        {
            // Update timer for current player
            if (!waitingForPromotion)
            {
                float elapsed = Time.time - currentTurnStartTime;
                if (currentTurn == TeamColor.White)
                    whiteTime += Time.deltaTime;
                else
                    blackTime += Time.deltaTime;
                
                UpdateTimerDisplay();
            }
            
            // Handle promotion input
            if (waitingForPromotion)
            {
                PieceType? chosenType = null;
                
                if (Input.GetKeyDown(KeyCode.Q))
                    chosenType = PieceType.Queen;
                else if (Input.GetKeyDown(KeyCode.R))
                    chosenType = PieceType.Rook;
                else if (Input.GetKeyDown(KeyCode.B))
                    chosenType = PieceType.Bishop;
                else if (Input.GetKeyDown(KeyCode.N))
                    chosenType = PieceType.Knight;
                
                if (chosenType.HasValue)
                {
                    PromotePawn(chosenType.Value);
                }
            }
        }

        private void UpdateScoreDisplay()
        {
            scoreText.text = $"White: {whiteScore} ({whiteMoves} moves)\nBlack: {blackScore} ({blackMoves} moves)";
        }

        public void StartGame()
        {
            board.SetupBoard();
            visualizer.GenerateBoard();
            SyncVisuals();
            
            // Initialize timer
            currentTurnStartTime = Time.time;
            whiteTime = 0f;
            blackTime = 0f;
            UpdateTimerDisplay();
        }

        public void SyncVisuals()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    ChessPiece cp = board.GetPiece(x, y);
                    if (cp != null)
                    {
                        visualizer.SpawnPiece(x, y, cp.type, cp.team);
                    }
                }
            }
        }

        public List<Vector2Int> GetValidMoves(int fromX, int fromY)
        {
            List<Vector2Int> validMoves = new List<Vector2Int>();
            
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (MoveValidator.IsValidMove(board, fromX, fromY, x, y))
                    {
                        validMoves.Add(new Vector2Int(x, y));
                    }
                }
            }
            
            return validMoves;
        }

        public void HighlightValidMoves(int fromX, int fromY)
        {
            var validMoves = GetValidMoves(fromX, fromY);
            visualizer.HighlightTiles(validMoves);
        }

        public void ClearHighlights()
        {
            visualizer.ClearAllHighlights();
        }

        public bool TryMove(int fromX, int fromY, int toX, int toY)
        {
            // Check if there's a piece at the source
            ChessPiece piece = board.GetPiece(fromX, fromY);
            if (piece == null)
            {
                Debug.Log("No piece at source position");
                return false;
            }

            // Check if it's the correct player's turn
            if (piece.team != currentTurn)
            {
                Debug.Log($"Not {piece.team}'s turn!");
                return false;
            }

            // Check if there's a piece to capture and update score
            ChessPiece targetPiece = board.GetPiece(toX, toY);
            if (targetPiece != null && targetPiece.team != piece.team)
            {
                int capturedValue = PieceValues.GetValue(targetPiece.type);
                if (piece.team == TeamColor.White)
                    whiteScore += capturedValue;
                else
                    blackScore += capturedValue;
                
                UpdateScoreDisplay();
            }

            // Try to move the piece (this validates the move)
            if (board.MovePiece(fromX, fromY, toX, toY))
            {
                visualizer.MovePieceVisual(fromX, fromY, toX, toY);
                
                // Increment move counter for current player
                if (currentTurn == TeamColor.White)
                    whiteMoves++;
                else
                    blackMoves++;
                    
                UpdateScoreDisplay();
                
                // Check for pawn promotion
                ChessPiece movedPiece = board.GetPiece(toX, toY);
                if (movedPiece != null && movedPiece.type == PieceType.Pawn)
                {
                    int lastRank = movedPiece.team == TeamColor.White ? 7 : 0;
                    if (toY == lastRank)
                    {
                        // Trigger promotion
                        waitingForPromotion = true;
                        promotionX = toX;
                        promotionY = toY;
                        promotionTeam = movedPiece.team;
                        ShowPromotionUI();
                        return true; // Don't switch turn yet
                    }
                }
                
                SwitchTurn();
                return true;
            }

            Debug.Log("Invalid move!");
            return false;
        }

        private void ShowPromotionUI()
        {
            Debug.Log($"Pawn promotion available at ({promotionX}, {promotionY})!");
            Debug.Log("Press Q=Queen, R=Rook, B=Bishop, N=Knight");
            // Show visual indicators on the promoted pawn's position
            visualizer.ShowPromotionChoices(promotionX, promotionY);
        }



        private void PromotePawn(PieceType newType)
        {
            // Update logic
            ChessPiece pawn = board.GetPiece(promotionX, promotionY);
            if (pawn != null)
            {
                pawn.type = newType;
                Debug.Log($"Pawn promoted to {newType}!");
            }
            
            // Update visual
            visualizer.ReplacePiece(promotionX, promotionY, newType, promotionTeam);
            visualizer.HidePromotionChoices();
            
            waitingForPromotion = false;
            SwitchTurn();
        }

        public void SwitchTurn()
        {
            currentTurn = (currentTurn == TeamColor.White) ? TeamColor.Black : TeamColor.White;
            Debug.Log($"Turn Switched: {currentTurn}");
            
            // Reset turn start time
            currentTurnStartTime = Time.time;
            
            // Move score display to current player's side
            UpdateScorePosition(currentTurn);
            UpdateTimerPosition(currentTurn);
            
            // Rotate camera to current player's perspective
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                CameraController camController = mainCam.GetComponent<CameraController>();
                if (camController != null)
                {
                    camController.SetViewForTeam(currentTurn);
                }
            }
        }
    }
}
