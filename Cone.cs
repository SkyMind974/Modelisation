using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ConeMeridiens : MonoBehaviour
{
    [Min(0f)] public float baseRadius = 1f;
    [Min(0f)] public float height = 2f;
    [Min(0f)] public float truncationHeight = 2f;
    public int meridian = 8;

    void OnDrawGizmos()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        int m = Mathf.Max(3, meridian);

        float h = Mathf.Max(0f, height);
        float ht = Mathf.Clamp(truncationHeight, 0f, h);
        float r0 = Mathf.Max(0f, baseRadius);
        const float EPS = 1e-5f;

        float rTop = h > EPS ? Mathf.Max(0f, r0 * (1f - (ht / h))) : r0;

        var vertices = new List<Vector3>(m * 4 + 4);
        var triangles = new List<int>(m * 6 + m * 6);
        var bottomRing = new List<int>(m);
        var topRing = new List<int>(m);

        bool isTruncated = rTop > EPS;

        if (isTruncated)
        {
            for (int i = 0; i < m; i++)
            {
                float a0 = (2f * Mathf.PI / m) * i;
                float a1 = (2f * Mathf.PI / m) * ((i + 1) % m);

                Vector3 bottomCurrent = new Vector3(Mathf.Cos(a0) * r0, 0f, Mathf.Sin(a0) * r0);
                Vector3 topCurrent = new Vector3(Mathf.Cos(a0) * rTop, ht, Mathf.Sin(a0) * rTop);

                Vector3 bottomNext = new Vector3(Mathf.Cos(a1) * r0, 0f, Mathf.Sin(a1) * r0);
                Vector3 topNext = new Vector3(Mathf.Cos(a1) * rTop, ht, Mathf.Sin(a1) * rTop);

                int i2 = vertices.Count;
                vertices.Add(bottomCurrent);
                vertices.Add(topCurrent);
                vertices.Add(bottomNext);
                vertices.Add(topNext);

                bottomRing.Add(i2 + 0);
                topRing.Add(i2 + 1);

                // Cotés
                DrawTriangle(i2 + 0, i2 + 1, i2 + 3);
                DrawTriangle(i2 + 3, i2 + 2, i2 + 0);
            }

            // Bas
            int bottomCenter = vertices.Count;
            vertices.Add(new Vector3(0f, 0f, 0f));
            for (int i = 0; i < m; i++)
            {
                int c = bottomCenter;
                int v0 = bottomRing[i];
                int v1 = bottomRing[(i + 1) % m];
                DrawTriangle(c, v1, v0);
            }

            // Haut
            int topCenter = vertices.Count;
            vertices.Add(new Vector3(0f, ht, 0f));
            for (int i = 0; i < m; i++)
            {
                int c = topCenter;
                int v0 = topRing[i];
                int v1 = topRing[(i + 1) % m];
                DrawTriangle(c, v0, v1);
            }
        }
        else
        {
            int apexIndex = vertices.Count;
            vertices.Add(new Vector3(0f, h, 0f));

            for (int i = 0; i < m; i++)
            {
                float a0 = (2f * Mathf.PI / m) * i;
                Vector3 bottom = new Vector3(Mathf.Cos(a0) * r0, 0f, Mathf.Sin(a0) * r0);
                bottomRing.Add(vertices.Count);
                vertices.Add(bottom);
            }

            for (int i = 0; i < m; i++)
            {
                int v0 = bottomRing[i];
                int v1 = bottomRing[(i + 1) % m];
                DrawTriangle(v0, apexIndex, v1);
            }

            int bottomCenter = vertices.Count;
            vertices.Add(new Vector3(0f, 0f, 0f));
            for (int i = 0; i < m; i++)
            {
                int c = bottomCenter;
                int v0 = bottomRing[i];
                int v1 = bottomRing[(i + 1) % m];
                DrawTriangle(c, v1, v0);
            }
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();

        void DrawTriangle(int a, int b, int c)
        {
            triangles.Add(a);
            triangles.Add(b);
            triangles.Add(c);

            triangles.Add(c);
            triangles.Add(b);
            triangles.Add(a);
        }
    }
}
