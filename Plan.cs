using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class NewMonoBehaviourScript : MonoBehaviour
{
    public int width = 1;
    public int height = 1;

    private void OnDrawGizmos()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        int gx = 5, gy = 5;
        var vertices = new List<Vector3>(gx * gy * 4);
        var triangles = new List<int>(gx * gy * 6);

        for (int y = 0; y < gy; y++)
        {
            for (int x = 0; x < gx; x++)
            {
                float ox = x * width;
                float oy = y * height;

                int baseIndex = vertices.Count;

                vertices.Add(new Vector3(ox, oy, 0));
                vertices.Add(new Vector3(ox, oy + height, 0));
                vertices.Add(new Vector3(ox + width, oy, 0));
                vertices.Add(new Vector3(ox + width, oy + height, 0));

                triangles.Add(baseIndex + 0);
                triangles.Add(baseIndex + 1);
                triangles.Add(baseIndex + 3);

                triangles.Add(baseIndex + 3);
                triangles.Add(baseIndex + 2);
                triangles.Add(baseIndex + 0);
            }
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
    }

    void Start()
    {
    }
}
