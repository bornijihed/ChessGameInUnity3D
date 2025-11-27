using UnityEngine;

namespace Visuals
{
    public class PieceObject : MonoBehaviour
    {
        private Vector3 targetPosition;
        public float speed = 5.0f;
        public float bounceHeight = 0.5f;
        public float bounceSpeed = 10.0f;

        private bool isMoving = false;
        public int gridX;
        public int gridY;

        public void SetTargetPosition(Vector3 target)
        {
            targetPosition = target;
            isMoving = true;
        }

        public void SetGridPosition(int x, int y)
        {
            gridX = x;
            gridY = y;
        }

        private void Update()
        {
            if (isMoving)
            {
                // Smooth Lerp
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);

                // "Rhythmic" Bounce effect when close to target? 
                // Or maybe just continuous breathing?
                // Let's do a simple move check
                if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
                {
                    transform.position = targetPosition;
                    isMoving = false;
                }
            }
            else
            {
                // Idle "Rhythmic" breathing
                float y = targetPosition.y + Mathf.Sin(Time.time * 2.0f) * 0.05f;
                transform.position = new Vector3(transform.position.x, y, transform.position.z);
            }
        }
    }
}
