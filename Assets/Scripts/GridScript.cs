using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    public int gridSizeX = 10;
    public int gridSizeZ = 10;
    public float cellSize = 1f;

     public LayerMask walkableLayer;
     public LayerMask obsticleLayer;
    public float maxRayDistance = 10f;
    public float maxSlopeAngle = 45f;

    // NavMesh Output
   // public NavMeshSurface navMeshSurface; // Assign in Inspector
    private bool[,] walkableGrid;
    private Vector3[,] floorPositions;

    void Start() {
        // Step 2: Raycast to detect walkable surfaces
        DetectWalkableAreas();
        
        // Step 3: Generate triangles and build NavMesh
        BuildNavMesh();
    }

    void DetectWalkableAreas() {
        walkableGrid = new bool[gridSizeX, gridSizeZ];
        floorPositions = new Vector3[gridSizeX, gridSizeZ];

        for (int x = 0; x < gridSizeX; x++) {
            for (int z = 0; z < gridSizeZ; z++) {
                Vector3 rayStart = new Vector3(
                    x * cellSize + cellSize / 2f,
                    maxRayDistance,
                    z * cellSize + cellSize / 2f
                );

                if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, maxRayDistance, walkableLayer)) {
                    float slope = Vector3.Angle(hit.normal, Vector3.up);
                    if (slope <= maxSlopeAngle) {
                        walkableGrid[x, z] = true;
                        floorPositions[x, z] = hit.point;
                    }
                }
            }
        }
    }

    void BuildNavMesh() {
        Mesh navMesh = CreateNavMesh();
        
        GameObject navMeshObj = new GameObject("NavMesh");
        navMeshObj.AddComponent<MeshFilter>().mesh = navMesh;
        navMeshObj.AddComponent<MeshRenderer>();
        MeshRenderer meshRenderer = navMeshObj.GetComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard")) {
    color = new Color(0.8f, 0.9f, 1f, 0.5f), // Pale blue with transparency
    enableInstancing = true
};
        
       
    }

    Mesh CreateNavMesh() {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        for (int x = 0; x < gridSizeX - 1; x++) {
            for (int z = 0; z < gridSizeZ - 1; z++) {
                if (walkableGrid[x, z] && walkableGrid[x + 1, z] && 
                    walkableGrid[x, z + 1] && walkableGrid[x + 1, z + 1]) {
                    int vIndex = vertices.Count;
                    vertices.Add(floorPositions[x, z]);
                    vertices.Add(floorPositions[x + 1, z]);
                    vertices.Add(floorPositions[x, z + 1]);
                    vertices.Add(floorPositions[x + 1, z + 1]);

                    triangles.Add(vIndex+3);
                    triangles.Add(vIndex + 1);
                    triangles.Add(vIndex + 2);
                    triangles.Add(vIndex + 1);
                    triangles.Add(vIndex);
                    triangles.Add(vIndex + 2);
                }
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        return mesh;
    }


    void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        
        // Draw horizontal lines (X-axis)
        for (int x = 0; x <= gridSizeX; x++) {
            Vector3 start = transform.position + new Vector3(x * cellSize, 0, 0);
            Vector3 end = start + new Vector3(0, 0, gridSizeZ * cellSize);
            Gizmos.DrawLine(start, end);
        }

        // Draw vertical lines (Z-axis)
        for (int z = 0; z <= gridSizeZ; z++) {
            Vector3 start = transform.position + new Vector3(0, 0, z * cellSize);
            Vector3 end = start + new Vector3(gridSizeX * cellSize, 0, 0);
            Gizmos.DrawLine(start, end);
        }
    }
}
