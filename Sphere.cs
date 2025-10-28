using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SphereParallelesMeridiens : MonoBehaviour
{
    [Min(0f)] public float radius = 1f;
    public int parallels = 8; 
    public int meridian = 16;  

    void OnDrawGizmos()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Sphere";
        GetComponent<MeshFilter>().mesh = mesh;

        int m = Mathf.Max(3, meridian);
        int p = Mathf.Max(2, parallels); 

        float r = Mathf.Max(0f, radius);

        var vertices = new List<Vector3>(2 + (p - 1) * m);
        var triangles = new List<int>(m * 6 + (p - 2) * m * 12);

        var ringStart = new List<int>(Mathf.Max(0, p - 1));

        // Poles
        int topIndex = vertices.Count;
        vertices.Add(new Vector3(0f, r, 0f)); 

        // Anneaux
        for (int i = 1; i <= p - 1; i++)
        {
            float phi = Mathf.PI * i / p;
            float y = r * Mathf.Cos(phi);
            float ringRadius = r * Mathf.Sin(phi);

            ringStart.Add(vertices.Count);

            for (int j = 0; j < m; j++)
            {
                float theta = (2f * Mathf.PI / m) * j;
                float x = ringRadius * Mathf.Cos(theta);
                float z = ringRadius * Mathf.Sin(theta);
                vertices.Add(new Vector3(x, y, z));
            }
        }

        int bottomIndex = vertices.Count;
        vertices.Add(new Vector3(0f, -r, 0f));

        void DrawTriangle(int a, int b, int c)
        {
            triangles.Add(a);
            triangles.Add(b);
            triangles.Add(c);

            triangles.Add(c);
            triangles.Add(b);
            triangles.Add(a);
        }

        // haut
        if (p - 1 > 0)
        {
            int firstRing = ringStart[0];
            for (int j = 0; j < m; j++)
            {
                int v0 = firstRing + j;
                int v1 = firstRing + ((j + 1) % m);
                DrawTriangle(topIndex, v0, v1);
            }
        }

        // carrés entre anneaux
        for (int ri = 0; ri < (p - 2); ri++)
        {
            int cur = ringStart[ri];
            int nxt = ringStart[ri + 1];

            for (int j = 0; j < m; j++)
            {
                int jn = (j + 1) % m;

                int v00 = cur + j;
                int v01 = cur + jn;
                int v10 = nxt + j;
                int v11 = nxt + jn;

                DrawTriangle(v00, v10, v11);
                DrawTriangle(v11, v01, v00);
            }
        }

        // bas
        if (p - 1 > 0)
        {
            int lastRing = ringStart[ringStart.Count - 1];
            for (int j = 0; j < m; j++)
            {
                int v0 = lastRing + j;
                int v1 = lastRing + ((j + 1) % m);
                DrawTriangle(bottomIndex, v1, v0);
            }
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
    }
}