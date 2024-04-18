using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();
    public List<Vector2> uv = new List<Vector2>();

    public List<Vector3> colliderVertices = new List<Vector3>();
    public List<int> colliderTriangles = new List<int>();

    public MeshData waterMesh;

    public MeshData(bool isMainMesh)
    {
        if (isMainMesh)
        {
            waterMesh = new MeshData(false);
        }
    }
    public void AddVertex(Vector3 vertex, bool vertexGeneratesCollider)
    {
        vertices.Add(vertex);
        if (vertexGeneratesCollider)
        {
            colliderVertices.Add(vertex);
        }
    }
    public void AddQuadTriangles(bool quadGeneratesCollider)
    {
        // Counting the amount of vertices given, and adding triangles from them
        // For example, we have 4 vertices, so if we minus that by 4, we end up with 0, same logic applies for 3 and 2, which in return give us 1 and 2, therefore drawing a triangle between those 3 points
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);

        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);
        if (quadGeneratesCollider)
        {
            colliderTriangles.Add(colliderVertices.Count - 4);
            colliderTriangles.Add(colliderVertices.Count - 3);
            colliderTriangles.Add(colliderVertices.Count - 2);
            colliderTriangles.Add(colliderVertices.Count - 4);
            colliderTriangles.Add(colliderVertices.Count - 2);
            colliderTriangles.Add(colliderVertices.Count - 1);
        }
    }

}
