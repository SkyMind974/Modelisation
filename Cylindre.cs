using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CylindreMeridiens : MonoBehaviour
{
    public float radius = 1f;
    public float height = 2f;
    public int meridian = 8;

    void OnDrawGizmos()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        int m = Mathf.Max(3, meridian);
        var vertices = new List<Vector3>(m * 4);
        var triangles = new List<int>(m * 6);
        var bottomRing = new List<int>(m);
        var topRing = new List<int>(m);

        for (int i = 0; i < m; i++)
        {
            float cut = (2 * Mathf.PI / m) * i;
            float nextCut = (2 * Mathf.PI / m) * ((i + 1) % m);

            Vector3 bottomCurrent = new Vector3(Mathf.Cos(cut) * radius, 0f, Mathf.Sin(cut) * radius);

            Vector3 topCurrent = new Vector3(bottomCurrent.x, height, bottomCurrent.z);

            Vector3 bottomNext = new Vector3(Mathf.Cos(nextCut) * radius, 0f, Mathf.Sin(nextCut) * radius);

            Vector3 topNext = new Vector3(bottomNext.x, height, bottomNext.z);

            int i2 = vertices.Count;
            vertices.Add(bottomCurrent);
            vertices.Add(topCurrent);
            vertices.Add(bottomNext);
            vertices.Add(topNext);

            bottomRing.Add(i2 + 0);
            topRing.Add(i2 + 1);

            DrawTriangle(i2 + 0, i2 + 1, i2 + 3);
            DrawTriangle(i2 + 3, i2 + 2, i2 + 0);


        }


        int bottomCenter = vertices.Count; vertices.Add(new Vector3(0f, 0f, 0f));
        for (int i = 0; i < m; i++)
        {
            int c = bottomCenter;
            int v0 = bottomRing[i];
            int v1 = bottomRing[(i + 1) % m];
            DrawTriangle(c, v1, v0);
        }

        int topCenter = vertices.Count; vertices.Add(new Vector3(0f, height, 0f));
        for (int i = 0; i < m; i++)
        {
            int c = topCenter;
            int v0 = topRing[i];
            int v1 = topRing[(i + 1) % m];
            DrawTriangle(c, v0, v1);
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
