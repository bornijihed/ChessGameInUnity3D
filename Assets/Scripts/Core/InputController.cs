using UnityEngine;
using Visuals;

namespace Core
{
    public class InputController : MonoBehaviour
    {
        private Camera mainCamera;
        private PieceObject selectedPiece;
        private int selectedPieceX;
        private int selectedPieceY;

        private void Start()
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                // Fallback if no main camera
                GameObject camObj = new GameObject("MainCamera");
                mainCamera = camObj.AddComponent<Camera>();
                camObj.tag = "MainCamera";
                
                // Add CameraController for automatic rotation
                CameraController camController = camObj.AddComponent<CameraController>();
            }
            else
            {
                // Ensure existing camera has CameraController
                if (mainCamera.GetComponent<CameraController>() == null)
                {
                    mainCamera.gameObject.AddComponent<CameraController>();
                }
            }
        }

        private TileObject hoveredTile;

        private void Update()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Get the tile we're hovering over
                Vector3 point = hit.point;
                int hoverX = Mathf.RoundToInt(point.x);
                int hoverY = Mathf.RoundToInt(point.z);
                
                // Handle Hover on tiles
                if (hoverX >= 0 && hoverX < 8 && hoverY >= 0 && hoverY < 8)
                {
                    TileObject tile = GameManager.Instance.visualizer.tiles[hoverX, hoverY].GetComponent<TileObject>();
                    if (tile != null && tile != hoveredTile)
                    {
                        if (hoveredTile != null) hoveredTile.SetHighlight(false);
                        hoveredTile = tile;
                        hoveredTile.SetHighlight(true);
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    // Calculate grid position from hit point
                    int clickX = Mathf.RoundToInt(point.x);
                    int clickY = Mathf.RoundToInt(point.z);

                    if (clickX < 0 || clickX >= 8 || clickY < 0 || clickY >= 8)
                        return;

                    // Check if there's a piece at this grid position
                    ChessPiece clickedPiece = GameManager.Instance.board.GetPiece(clickX, clickY);
                    
                    if (selectedPiece == null)
                    {
                        // No piece selected yet, try to select one
                        if (clickedPiece != null)
                        {
                            // Select this piece
                            selectedPiece = GameManager.Instance.visualizer.GetPieceObject(clickX, clickY);
                            selectedPieceX = clickX;
                            selectedPieceY = clickY;
                            Debug.Log($"Selected Piece at: ({clickX}, {clickY})");
                            
                            // Highlight valid moves
                            GameManager.Instance.HighlightValidMoves(clickX, clickY);
                        }
                    }
                    else
                    {
                        // Already have a piece selected, try to move it
                        bool moved = GameManager.Instance.TryMove(selectedPieceX, selectedPieceY, clickX, clickY);
                        
                        // Clear selection and highlights
                        selectedPiece = null;
                        GameManager.Instance.ClearHighlights();
                    }
                }
            }
            else
            {
                if (hoveredTile != null)
                {
                    hoveredTile.SetHighlight(false);
                    hoveredTile = null;
                }
            }
        }
    }
}
