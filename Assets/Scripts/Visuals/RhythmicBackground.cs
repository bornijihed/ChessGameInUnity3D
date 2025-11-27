using UnityEngine;

namespace Visuals
{
    public class RhythmicBackground : MonoBehaviour
    {
        private Material backgroundMaterial;
        private float time = 0f;
        public float pulseSpeed = 1.5f;
        public Color color1 = new Color(0.1f, 0.05f, 0.2f); // Dark purple
        public Color color2 = new Color(0.15f, 0.1f, 0.25f); // Lighter purple

        private void Start()
        {
            // Create a large plane beneath the board
            GameObject bgPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            bgPlane.transform.parent = transform;
            bgPlane.transform.localPosition = new Vector3(3.5f, -1f, 3.5f);
            bgPlane.transform.localScale = new Vector3(5f, 1f, 5f);
            
            Renderer renderer = bgPlane.GetComponent<Renderer>();
            backgroundMaterial = new Material(Shader.Find("Standard"));
            backgroundMaterial.SetFloat("_Metallic", 0.3f);
            backgroundMaterial.SetFloat("_Glossiness", 0.5f);
            renderer.material = backgroundMaterial;
        }

        private void Update()
        {
            time += Time.deltaTime * pulseSpeed;
            
            // Rhythmic pulsing between two colors
            float pulse = (Mathf.Sin(time) + 1f) * 0.5f; // 0 to 1
            Color currentColor = Color.Lerp(color1, color2, pulse);
            
            if (backgroundMaterial != null)
            {
                backgroundMaterial.color = currentColor;
            }
        }
    }
}
