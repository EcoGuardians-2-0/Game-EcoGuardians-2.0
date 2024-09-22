using UnityEngine;

public class SurfaceDetector : MonoBehaviour
{
    private TerrainChecker terrainChecker;

    private void Start()
    {
        terrainChecker = new TerrainChecker();
    }

    public void CheckSurface()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 3))
        {
            string surfaceType = "";

            if (hit.transform.TryGetComponent<Terrain>(out Terrain terrain))
            {
                surfaceType = terrainChecker.GetLayerName(transform.position, terrain);
            }
            else if (hit.transform.TryGetComponent<SurfaceType>(out SurfaceType surfaceTypeComponent))
            {
                surfaceType = surfaceTypeComponent.footstepCollection.name;
            }

            if (!string.IsNullOrEmpty(surfaceType))
            {
                AudioManager.Instance.UpdateSurfaceType(surfaceType);
            }
        }
    }
}