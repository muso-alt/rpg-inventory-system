using UnityEngine;

namespace Inventory.Services
{
    public class CameraService : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        
        public Camera Camera => _mainCamera;
    }
}