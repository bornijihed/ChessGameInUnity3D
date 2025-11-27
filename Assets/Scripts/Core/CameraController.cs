using UnityEngine;

namespace Core
{
    public class CameraController : MonoBehaviour
    {
        private Camera cam;
        private Vector3 whiteViewPosition = new Vector3(3.5f, 8, -3);
        private Vector3 blackViewPosition = new Vector3(3.5f, 8, 10.5f);
        private Vector3 boardCenter = new Vector3(3.5f, 0, 3.5f);
        
        private Vector3 targetPosition;
        private Quaternion targetRotation;
        private bool isRotating = false;
        public float rotationSpeed = 2.0f;

        private void Start()
        {
            cam = GetComponent<Camera>();
            if (cam == null)
            {
                Debug.LogError("CameraController requires a Camera component!");
                return;
            }
            
            // Start with white's view
            SetViewForTeam(TeamColor.White, immediate: true);
        }

        private void Update()
        {
            if (isRotating)
            {
                // Smoothly interpolate position and rotation
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * rotationSpeed);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                
                // Check if we're close enough to stop
                if (Vector3.Distance(transform.position, targetPosition) < 0.01f &&
                    Quaternion.Angle(transform.rotation, targetRotation) < 0.5f)
                {
                    transform.position = targetPosition;
                    transform.rotation = targetRotation;
                    isRotating = false;
                }
            }
        }

        public void SetViewForTeam(TeamColor team, bool immediate = false)
        {
            if (team == TeamColor.White)
            {
                targetPosition = whiteViewPosition;
            }
            else
            {
                targetPosition = blackViewPosition;
            }
            
            // Always look at board center
            targetRotation = Quaternion.LookRotation(boardCenter - targetPosition);
            
            if (immediate)
            {
                transform.position = targetPosition;
                transform.rotation = targetRotation;
                isRotating = false;
            }
            else
            {
                isRotating = true;
            }
        }
    }
}
