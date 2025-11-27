using UnityEngine;
using Core;
using System.Collections.Generic;

namespace Visuals
{
    public class BoardVisualizer : MonoBehaviour
    {
        public GameObject tilePrefab; // We'll use a Cube primitive if null
        public Material whiteTileMat;
        public Material blackTileMat;
        public float tileSize = 1.0f;
        public float yOffset = 0.5f; // To raise pieces above board

        // Prefabs for pieces (we'll use primitives for now)
        public GameObject[] whitePiecePrefabs; // Index matches PieceType
        public GameObject[] blackPiecePrefabs;

        public GameObject[,] tiles;
        private PieceObject[,] pieceObjects;

        public void GenerateBoard()
        {
            tiles = new GameObject[8, 8];
            pieceObjects = new PieceObject[8, 8];

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    GenerateTile(x, y);
                }
            }
        }

        private void GenerateTile(int x, int y)
        {
            GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tile.transform.parent = transform;
            tile.transform.position = new Vector3(x * tileSize, 0, y * tileSize);
            tile.name = $"Tile_{x}_{y}";

            Renderer r = tile.GetComponent<Renderer>();
            bool isWhite = (x + y) % 2 != 0;
            
            if (isWhite)
                r.material = whiteTileMat != null ? whiteTileMat : CreateMaterial(Color.white);
            else
                r.material = blackTileMat != null ? blackTileMat : CreateMaterial(Color.black);

            tile.AddComponent<TileObject>();
            tiles[x, y] = tile;
        }

        private Material CreateMaterial(Color color)
        {
            Material m = new Material(Shader.Find("Standard"));
            m.color = color;
            return m;
        }

        public void SpawnPiece(int x, int y, PieceType type, TeamColor team)
        {
            if (type == PieceType.None) return;

            GameObject pieceObj = CreateEnhancedPiece(type, team);
            pieceObj.transform.position = GetTileCenter(x, y);
            pieceObj.name = $"{team}_{type}_{x}_{y}";
            
            PieceObject po = pieceObj.AddComponent<PieceObject>();
            po.SetTargetPosition(GetTileCenter(x, y));
            po.SetGridPosition(x, y);
            
            pieceObjects[x, y] = po;
        }

        private GameObject CreateEnhancedPiece(PieceType type, TeamColor team)
        {
            GameObject parent = new GameObject();
            
            // Base colors - White team uses bright colors, Black team uses dark colors
            Color baseColor = team == TeamColor.White ? new Color(0.95f, 0.95f, 1f) : new Color(0.15f, 0.15f, 0.2f);
            
            // Color-code each piece type with accent colors for easy identification
            Color accentColor;
            switch (type)
            {
                case PieceType.Pawn:
                    accentColor = team == TeamColor.White ? new Color(0.7f, 0.9f, 0.7f) : new Color(0.2f, 0.4f, 0.2f); // Green
                    break;
                case PieceType.Rook:
                    accentColor = team == TeamColor.White ? new Color(0.9f, 0.7f, 0.7f) : new Color(0.4f, 0.2f, 0.2f); // Red
                    break;
                case PieceType.Knight:
                    accentColor = team == TeamColor.White ? new Color(0.9f, 0.8f, 0.5f) : new Color(0.4f, 0.3f, 0.15f); // Orange
                    break;
                case PieceType.Bishop:
                    accentColor = team == TeamColor.White ? new Color(0.8f, 0.7f, 0.9f) : new Color(0.3f, 0.2f, 0.4f); // Purple
                    break;
                case PieceType.Queen:
                    accentColor = team == TeamColor.White ? new Color(1f, 0.9f, 0.3f) : new Color(0.6f, 0.5f, 0.1f); // Gold
                    break;
                case PieceType.King:
                    accentColor = team == TeamColor.White ? new Color(0.7f, 0.8f, 1f) : new Color(0.2f, 0.3f, 0.5f); // Blue
                    break;
                default:
                    accentColor = baseColor;
                    break;
            }
            
            switch (type)
            {
                case PieceType.Pawn:
                    CreatePawn(parent, baseColor, accentColor);
                    break;
                case PieceType.Rook:
                    CreateRook(parent, baseColor, accentColor);
                    break;
                case PieceType.Knight:
                    CreateKnight(parent, baseColor, accentColor);
                    break;
                case PieceType.Bishop:
                    CreateBishop(parent, baseColor, accentColor);
                    break;
                case PieceType.Queen:
                    CreateQueen(parent, baseColor, accentColor);
                    break;
                case PieceType.King:
                    CreateKing(parent, baseColor, accentColor);
                    break;
            }
            
            return parent;
        }

        private void CreatePawn(GameObject parent, Color color, Color accentColor)
        {
            // Multi-tier base for stability
            GameObject base1 = CreatePrimitive(PrimitiveType.Cylinder, parent);
            base1.transform.localScale = new Vector3(0.35f, 0.08f, 0.35f);
            base1.transform.localPosition = new Vector3(0, 0.08f, 0);
            ApplyMaterial(base1, color, 0.3f);
            
            GameObject base2 = CreatePrimitive(PrimitiveType.Cylinder, parent);
            base2.transform.localScale = new Vector3(0.32f, 0.06f, 0.32f);
            base2.transform.localPosition = new Vector3(0, 0.19f, 0);
            ApplyMaterial(base2, color, 0.4f);
            
            // Tapered stem
            GameObject stem = CreatePrimitive(PrimitiveType.Cylinder, parent);
            stem.transform.localScale = new Vector3(0.2f, 0.25f, 0.2f);
            stem.transform.localPosition = new Vector3(0, 0.44f, 0);
            ApplyMaterial(stem, color, 0.5f);
            
            // Sphere head - COLOR CODED
            GameObject head = CreatePrimitive(PrimitiveType.Sphere, parent);
            head.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            head.transform.localPosition = new Vector3(0, 0.75f, 0);
            ApplyMaterial(head, accentColor, 0.8f);
        }

        private void CreateRook(GameObject parent, Color color, Color accentColor)
        {
            // Multi-tier base
            GameObject base1 = CreatePrimitive(PrimitiveType.Cylinder, parent);
            base1.transform.localScale = new Vector3(0.4f, 0.08f, 0.4f);
            base1.transform.localPosition = new Vector3(0, 0.08f, 0);
            ApplyMaterial(base1, color, 0.3f);
            
            GameObject base2 = CreatePrimitive(PrimitiveType.Cylinder, parent);
            base2.transform.localScale = new Vector3(0.36f, 0.06f, 0.36f);
            base2.transform.localPosition = new Vector3(0, 0.19f, 0);
            ApplyMaterial(base2, color, 0.4f);
            
            // Lower tower section
            GameObject lower = CreatePrimitive(PrimitiveType.Cylinder, parent);
            lower.transform.localScale = new Vector3(0.28f, 0.2f, 0.28f);
            lower.transform.localPosition = new Vector3(0, 0.39f, 0);
            ApplyMaterial(lower, color, 0.5f);
            
            // Main tower body
            GameObject tower = CreatePrimitive(PrimitiveType.Cube, parent);
            tower.transform.localScale = new Vector3(0.32f, 0.5f, 0.32f);
            tower.transform.localPosition = new Vector3(0, 0.84f, 0);
            ApplyMaterial(tower, color, 0.6f);
            
            // Battlements ring - COLOR CODED
            GameObject battlements = CreatePrimitive(PrimitiveType.Cylinder, parent);
            battlements.transform.localScale = new Vector3(0.38f, 0.12f, 0.38f);
            battlements.transform.localPosition = new Vector3(0, 1.15f, 0);
            ApplyMaterial(battlements, accentColor, 0.9f);
            
            // Top crenellations (4 small cubes)
            for (int i = 0; i < 4; i++)
            {
                GameObject cren = CreatePrimitive(PrimitiveType.Cube, parent);
                float angle = i * 90f * Mathf.Deg2Rad;
                float offset = 0.2f;
                cren.transform.localScale = new Vector3(0.1f, 0.15f, 0.1f);
                cren.transform.localPosition = new Vector3(
                    Mathf.Cos(angle) * offset,
                    1.3f,
                    Mathf.Sin(angle) * offset
                );
                ApplyMaterial(cren, accentColor, 1f);
            }
        }

        private void CreateKnight(GameObject parent, Color color, Color accentColor)
        {
            // Multi-tier base
            GameObject base1 = CreatePrimitive(PrimitiveType.Cylinder, parent);
            base1.transform.localScale = new Vector3(0.38f, 0.08f, 0.38f);
            base1.transform.localPosition = new Vector3(0, 0.08f, 0);
            ApplyMaterial(base1, color, 0.3f);
            
            GameObject base2 = CreatePrimitive(PrimitiveType.Cylinder, parent);
            base2.transform.localScale = new Vector3(0.34f, 0.06f, 0.34f);
            base2.transform.localPosition = new Vector3(0, 0.19f, 0);
            ApplyMaterial(base2, color, 0.4f);
            
            // Pedestal
            GameObject pedestal = CreatePrimitive(PrimitiveType.Cylinder, parent);
            pedestal.transform.localScale = new Vector3(0.26f, 0.15f, 0.26f);
            pedestal.transform.localPosition = new Vector3(0, 0.34f, 0);
            ApplyMaterial(pedestal, color, 0.5f);
            
            // Horse neck (long tilted capsule) - COLOR CODED
            GameObject neck = CreatePrimitive(PrimitiveType.Capsule, parent);
            neck.transform.localScale = new Vector3(0.22f, 0.45f, 0.22f);
            neck.transform.localPosition = new Vector3(0.05f, 0.7f, 0);
            neck.transform.localRotation = Quaternion.Euler(0, 0, 25);
            ApplyMaterial(neck, accentColor, 0.7f);
            
            // Horse head (sphere)
            GameObject head = CreatePrimitive(PrimitiveType.Sphere, parent);
            head.transform.localScale = new Vector3(0.28f, 0.28f, 0.35f);
            head.transform.localPosition = new Vector3(0.15f, 1.05f, 0);
            ApplyMaterial(head, accentColor, 0.8f);
            
            // Ears (2 small cubes)
            GameObject ear1 = CreatePrimitive(PrimitiveType.Cube, parent);
            ear1.transform.localScale = new Vector3(0.08f, 0.15f, 0.06f);
            ear1.transform.localPosition = new Vector3(0.1f, 1.2f, -0.08f);
            ApplyMaterial(ear1, accentColor, 0.9f);
            
            GameObject ear2 = CreatePrimitive(PrimitiveType.Cube, parent);
            ear2.transform.localScale = new Vector3(0.08f, 0.15f, 0.06f);
            ear2.transform.localPosition = new Vector3(0.1f, 1.2f, 0.08f);
            ApplyMaterial(ear2, accentColor, 0.9f);
        }

        private void CreateBishop(GameObject parent, Color color, Color accentColor)
        {
            // Multi-tier base
            GameObject base1 = CreatePrimitive(PrimitiveType.Cylinder, parent);
            base1.transform.localScale = new Vector3(0.38f, 0.08f, 0.38f);
            base1.transform.localPosition = new Vector3(0, 0.08f, 0);
            ApplyMaterial(base1, color, 0.3f);
            
            GameObject base2 = CreatePrimitive(PrimitiveType.Cylinder, parent);
            base2.transform.localScale = new Vector3(0.34f, 0.06f, 0.34f);
            base2.transform.localPosition = new Vector3(0, 0.19f, 0);
            ApplyMaterial(base2, color, 0.4f);
            
            // Wide lower section
            GameObject lower = CreatePrimitive(PrimitiveType.Cylinder, parent);
            lower.transform.localScale = new Vector3(0.3f, 0.15f, 0.3f);
            lower.transform.localPosition = new Vector3(0, 0.34f, 0);
            ApplyMaterial(lower, color, 0.5f);
            
            // Tapered mid section
            GameObject mid = CreatePrimitive(PrimitiveType.Cylinder, parent);
            mid.transform.localScale = new Vector3(0.24f, 0.35f, 0.24f);
            mid.transform.localPosition = new Vector3(0, 0.64f, 0);
            ApplyMaterial(mid, color, 0.6f);
            
            // Upper sphere
            GameObject upper = CreatePrimitive(PrimitiveType.Sphere, parent);
            upper.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            upper.transform.localPosition = new Vector3(0, 0.95f, 0);
            ApplyMaterial(upper, color, 0.65f);
            
            // Mitre point (elongated sphere) - COLOR CODED
            GameObject mitre = CreatePrimitive(PrimitiveType.Sphere, parent);
            mitre.transform.localScale = new Vector3(0.18f, 0.35f, 0.18f);
            mitre.transform.localPosition = new Vector3(0, 1.25f, 0);
            ApplyMaterial(mitre, accentColor, 0.9f);
            
            // Slit in mitre (small sphere cut indicator)
            GameObject slit = CreatePrimitive(PrimitiveType.Sphere, parent);
            slit.transform.localScale = new Vector3(0.06f, 0.08f, 0.12f);
            slit.transform.localPosition = new Vector3(0, 1.35f, 0);
            ApplyMaterial(slit, accentColor, 1f);
        }

        private void CreateQueen(GameObject parent, Color color, Color accentColor)
        {
            // Large multi-tier base
            GameObject base1 = CreatePrimitive(PrimitiveType.Cylinder, parent);
            base1.transform.localScale = new Vector3(0.45f, 0.09f, 0.45f);
            base1.transform.localPosition = new Vector3(0, 0.09f, 0);
            ApplyMaterial(base1, color, 0.3f);
            
            GameObject base2 = CreatePrimitive(PrimitiveType.Cylinder, parent);
            base2.transform.localScale = new Vector3(0.4f, 0.07f, 0.4f);
            base2.transform.localPosition = new Vector3(0, 0.22f, 0);
            ApplyMaterial(base2, color, 0.4f);
            
            // Wide pedestal
            GameObject pedestal = CreatePrimitive(PrimitiveType.Cylinder, parent);
            pedestal.transform.localScale = new Vector3(0.32f, 0.2f, 0.32f);
            pedestal.transform.localPosition = new Vector3(0, 0.42f, 0);
            ApplyMaterial(pedestal, color, 0.5f);
            
            // Lower body section
            GameObject lowerBody = CreatePrimitive(PrimitiveType.Cylinder, parent);
            lowerBody.transform.localScale = new Vector3(0.28f, 0.3f, 0.28f);
            lowerBody.transform.localPosition = new Vector3(0, 0.77f, 0);
            ApplyMaterial(lowerBody, color, 0.6f);
            
            // Upper body (sphere)
            GameObject upperBody = CreatePrimitive(PrimitiveType.Sphere, parent);
            upperBody.transform.localScale = new Vector3(0.35f, 0.4f, 0.35f);
            upperBody.transform.localPosition = new Vector3(0, 1.15f, 0);
            ApplyMaterial(upperBody, color, 0.7f);
            
            // Crown base ring - COLOR CODED
            GameObject crownBase = CreatePrimitive(PrimitiveType.Cylinder, parent);
            crownBase.transform.localScale = new Vector3(0.32f, 0.08f, 0.32f);
            crownBase.transform.localPosition = new Vector3(0, 1.47f, 0);
            ApplyMaterial(crownBase, accentColor, 0.85f);
            
            // Crown spikes (5 spheres arranged in circle)
            for (int i = 0; i < 5; i++)
            {
                GameObject spike = CreatePrimitive(PrimitiveType.Sphere, parent);
                float angle = i * 72f * Mathf.Deg2Rad;
                float radius = 0.18f;
                spike.transform.localScale = new Vector3(0.12f, 0.2f, 0.12f);
                spike.transform.localPosition = new Vector3(
                    Mathf.Cos(angle) * radius,
                    1.65f,
                    Mathf.Sin(angle) * radius
                );
                ApplyMaterial(spike, accentColor, 0.95f);
            }
            
            // Center crown jewel
            GameObject jewel = CreatePrimitive(PrimitiveType.Sphere, parent);
            jewel.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            jewel.transform.localPosition = new Vector3(0, 1.75f, 0);
            ApplyMaterial(jewel, accentColor, 1f);
        }

        private void CreateKing(GameObject parent, Color color, Color accentColor)
        {
            // Largest multi-tier base
            GameObject base1 = CreatePrimitive(PrimitiveType.Cylinder, parent);
            base1.transform.localScale = new Vector3(0.48f, 0.1f, 0.48f);
            base1.transform.localPosition = new Vector3(0, 0.1f, 0);
            ApplyMaterial(base1, color, 0.3f);
            
            GameObject base2 = CreatePrimitive(PrimitiveType.Cylinder, parent);
            base2.transform.localScale = new Vector3(0.43f, 0.08f, 0.43f);
            base2.transform.localPosition = new Vector3(0, 0.24f, 0);
            ApplyMaterial(base2, color, 0.4f);
            
            // Wide pedestal
            GameObject pedestal = CreatePrimitive(PrimitiveType.Cylinder, parent);
            pedestal.transform.localScale = new Vector3(0.34f, 0.22f, 0.34f);
            pedestal.transform.localPosition = new Vector3(0, 0.46f, 0);
            ApplyMaterial(pedestal, color, 0.5f);
            
            // Lower body
            GameObject lowerBody = CreatePrimitive(PrimitiveType.Cylinder, parent);
            lowerBody.transform.localScale = new Vector3(0.3f, 0.35f, 0.3f);
            lowerBody.transform.localPosition = new Vector3(0, 0.83f, 0);
            ApplyMaterial(lowerBody, color, 0.6f);
            
            // Upper body (large sphere)
            GameObject upperBody = CreatePrimitive(PrimitiveType.Sphere, parent);
            upperBody.transform.localScale = new Vector3(0.38f, 0.45f, 0.38f);
            upperBody.transform.localPosition = new Vector3(0, 1.28f, 0);
            ApplyMaterial(upperBody, color, 0.7f);
            
            // Crown base - COLOR CODED
            GameObject crownBase = CreatePrimitive(PrimitiveType.Cylinder, parent);
            crownBase.transform.localScale = new Vector3(0.34f, 0.1f, 0.34f);
            crownBase.transform.localPosition = new Vector3(0, 1.63f, 0);
            ApplyMaterial(crownBase, accentColor, 0.8f);
            
            // Crown ring
            GameObject crownRing = CreatePrimitive(PrimitiveType.Cylinder, parent);
            crownRing.transform.localScale = new Vector3(0.3f, 0.12f, 0.3f);
            crownRing.transform.localPosition = new Vector3(0, 1.8f, 0);
            ApplyMaterial(crownRing, accentColor, 0.85f);
            
            // Cross vertical (larger, prominent)
            GameObject crossV = CreatePrimitive(PrimitiveType.Cube, parent);
            crossV.transform.localScale = new Vector3(0.08f, 0.35f, 0.08f);
            crossV.transform.localPosition = new Vector3(0, 2.05f, 0);
            ApplyMaterial(crossV, accentColor, 0.95f);
            
            // Cross horizontal
            GameObject crossH = CreatePrimitive(PrimitiveType.Cube, parent);
            crossH.transform.localScale = new Vector3(0.28f, 0.08f, 0.08f);
            crossH.transform.localPosition = new Vector3(0, 2.1f, 0);
            ApplyMaterial(crossH, accentColor, 0.95f);
            
            // Orb at cross center
            GameObject orb = CreatePrimitive(PrimitiveType.Sphere, parent);
            orb.transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
            orb.transform.localPosition = new Vector3(0, 2.1f, 0);
            ApplyMaterial(orb, accentColor, 1f);
        }



        private GameObject CreatePrimitive(PrimitiveType type, GameObject parent)
        {
            GameObject obj = GameObject.CreatePrimitive(type);
            obj.transform.parent = parent.transform;
            return obj;
        }

        private void ApplyMaterial(GameObject obj, Color color, float metallic)
        {
            Renderer r = obj.GetComponent<Renderer>();
            Material mat = new Material(Shader.Find("Standard"));
            mat.color = color;
            mat.SetFloat("_Metallic", metallic);
            mat.SetFloat("_Glossiness", 0.7f);
            r.material = mat;
        }

        public void MovePieceVisual(int oldX, int oldY, int newX, int newY)
        {
            // Destroy captured piece if any
            if (pieceObjects[newX, newY] != null)
            {
                Destroy(pieceObjects[newX, newY].gameObject);
                pieceObjects[newX, newY] = null;
            }

            PieceObject p = pieceObjects[oldX, oldY];
            if (p != null)
            {
                pieceObjects[newX, newY] = p;
                pieceObjects[oldX, oldY] = null;
                
                p.SetGridPosition(newX, newY);
                p.SetTargetPosition(GetTileCenter(newX, newY));
            }
        }

        public PieceObject GetPieceObject(int x, int y)
        {
            if (x < 0 || x >= 8 || y < 0 || y >= 8) return null;
            return pieceObjects[x, y];
        }

        public Vector3 GetTileCenter(int x, int y)
        {
            return new Vector3(x * tileSize, yOffset, y * tileSize);
        }

        public void HighlightTiles(List<Vector2Int> positions)
        {
            foreach (var pos in positions)
            {
                if (pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8)
                {
                    TileObject tile = tiles[pos.x, pos.y].GetComponent<TileObject>();
                    if (tile != null)
                    {
                        tile.SetValidMove(true);
                    }
                }
            }
        }

        public void ClearAllHighlights()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    TileObject tile = tiles[x, y].GetComponent<TileObject>();
                    if (tile != null)
                    {
                        tile.SetValidMove(false);
                    }
                }
            }
        }

        private GameObject promotionUIParent;

        public void ShowPromotionChoices(int x, int y)
        {
            if (promotionUIParent != null)
                Destroy(promotionUIParent);
                
            promotionUIParent = new GameObject("PromotionUI");
            promotionUIParent.transform.parent = transform;
            
            // Create floating text above the promoted pawn
            GameObject textObj = new GameObject("PromotionText");
            textObj.transform.parent = promotionUIParent.transform;
            textObj.transform.position = new Vector3(x * tileSize, yOffset + 2, y * tileSize);
            textObj.transform.rotation = Quaternion.Euler(0, 0, 0); // Face forward (readable from angled view)
            
            TextMesh textMesh = textObj.AddComponent<TextMesh>();
            textMesh.text = "Q=Queen  R=Rook\nB=Bishop  N=Knight";
            textMesh.fontSize = 40;
            textMesh.characterSize = 0.08f;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.color = Color.cyan;
            textMesh.GetComponent<MeshRenderer>().material.color = Color.cyan;
        }

        public void HidePromotionChoices()
        {
            if (promotionUIParent != null)
            {
                Destroy(promotionUIParent);
                promotionUIParent = null;
            }
        }

        public void ReplacePiece(int x, int y, PieceType newType, TeamColor team)
        {
            // Destroy old piece visually
            if (pieceObjects[x, y] != null)
            {
                Destroy(pieceObjects[x, y].gameObject);
                pieceObjects[x, y] = null;
            }
            
            // Spawn new piece
            SpawnPiece(x, y, newType, team);
        }

        private PrimitiveType GetPrimitiveType(PieceType type)
        {
            switch (type)
            {
                case PieceType.Pawn: return PrimitiveType.Sphere;
                case PieceType.Rook: return PrimitiveType.Cube;
                case PieceType.Knight: return PrimitiveType.Capsule; // Horse-ish?
                case PieceType.Bishop: return PrimitiveType.Cylinder;
                case PieceType.Queen: return PrimitiveType.Sphere; // Bigger sphere?
                case PieceType.King: return PrimitiveType.Cube; // Bigger cube?
                default: return PrimitiveType.Cube;
            }
        }
    }
}
