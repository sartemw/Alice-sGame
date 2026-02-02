using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace _Project.CodeBase.Services
{
    public class AutoScaleCameraService : MonoBehaviour
    { 
        [SerializeField] private float _padding = 1f;

        private Camera _camera;
        private Tilemap _largestTilemap;
    
        private void Awake()
        {
            _camera = GetComponent<Camera>();
            ScaleCameraToFitLargestTilemap();
        }
        
        private void ScaleCameraToFitLargestTilemap()
        {
            Tilemap[] tilemaps = FindObjectsOfType<Tilemap>();
    
            if (tilemaps.Length == 0)
            {
                Debug.LogWarning("No Tilemaps found in scene");
                return;
            }
    
            // Найти самый большой Tilemap
            _largestTilemap = FindSkyTilemap(tilemaps);
    
            if (_largestTilemap == null)
            {
                Debug.LogWarning("No valid Tilemaps found");
                return;
            }
    
            // Получить границы Tilemap
            BoundsInt bounds = _largestTilemap.cellBounds;
            Vector3Int size = bounds.size;
    
            // Получить координаты углов
            Vector3 minCorner = _largestTilemap.CellToWorld(new Vector3Int(bounds.x, bounds.y, 0));
            Vector3 maxCorner = _largestTilemap.CellToWorld(new Vector3Int(bounds.x + size.x, bounds.y + size.y, 0));
    
            // Рассчитать размеры и позицию камеры
            Vector3 center = (minCorner + maxCorner) / 2;
            Vector3 sizeVector = maxCorner - minCorner;
    
            // Установить размер камеры с отступом
            _camera.orthographicSize = (sizeVector.y - _padding) / 2;
    
            // Получаем размеры камеры в мировых координатах
            float cameraHeight = _camera.orthographicSize * 2;
            float cameraWidth = cameraHeight * _camera.aspect;
    
            // Центрируем камеру с учетом границ Tilemap
            float minX = minCorner.x + cameraWidth / 2;
            float maxX = maxCorner.x - cameraWidth / 2;
            float minY = minCorner.y + cameraHeight / 2;
            float maxY = maxCorner.y - cameraHeight / 2;
    
            // Ограничиваем позицию камеры
            float clampedX = Mathf.Clamp(center.x, minX, maxX);
            float clampedY = Mathf.Clamp(center.y, minY, maxY);
    
            // Центрируем камеру
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }

        
        private Tilemap FindSkyTilemap(Tilemap[] tilemaps)
        {
            Tilemap largest = null;
            
            foreach (Tilemap tilemap in tilemaps)
            {
                if (tilemap == null) continue;
                
                if (tilemap.name == "Sky")
                {
                    largest = tilemap;
                }
            }
            
            return largest;
        }
    }
}