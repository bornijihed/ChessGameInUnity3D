using UnityEngine;

namespace Visuals
{
    public class TileObject : MonoBehaviour
    {
        private Material originalMaterial;
        private Color originalColor;
        private Renderer rend;
        private bool isHighlighted = false;
        private bool isValidMove = false;

        private void Start()
        {
            rend = GetComponent<Renderer>();
            originalMaterial = rend.material;
            originalColor = originalMaterial.color;
        }

        public void SetHighlight(bool highlight)
        {
            if (isHighlighted == highlight) return;
            isHighlighted = highlight;

            UpdateVisuals();
        }

        public void SetValidMove(bool valid)
        {
            if (isValidMove == valid) return;
            isValidMove = valid;
            
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            if (isValidMove)
            {
                // Green for valid moves
                rend.material.color = Color.Lerp(originalColor, Color.green, 0.6f);
            }
            else if (isHighlighted)
            {
                // Yellow for hover
                rend.material.color = Color.Lerp(originalColor, Color.yellow, 0.5f);
            }
            else
            {
                rend.material.color = originalColor;
            }
        }

        private void Update()
        {
            if (isValidMove)
            {
                // Rhythmic pulsing for valid moves
                float emission = Mathf.PingPong(Time.time * 3.0f, 0.4f);
                Color finalColor = Color.Lerp(originalColor, Color.green, 0.6f + emission);
                rend.material.color = finalColor;
            }
            else if (isHighlighted)
            {
                // Rhythmic pulsing for hover
                float emission = Mathf.PingPong(Time.time * 2.0f, 0.3f);
                Color finalColor = Color.Lerp(originalColor, Color.yellow, 0.5f + emission);
                rend.material.color = finalColor;
            }
        }
    }
}
