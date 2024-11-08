using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life_RadarChart : MonoBehaviour
{
    [SerializeField] private CanvasRenderer meshRenderer;
    [SerializeField] private Material meshMaterial;

    public void UpdateStatsRadar()
    {
        Mesh meshRadar = new Mesh();
        Vector3[] vertices = new Vector3[6];
        Vector2[] uvs = new Vector2[6];
        int[] triangles = new int[15];


        float radarChartSize = 108f;
        float angleParts = 360.0f / 5;

        Vector3 romanticVertex = Quaternion.Euler(0, 0, -angleParts * 0) * Vector3.up * radarChartSize * (VidaPersonalManager.Instance.romanticProgress / 100.0f);
        int romanticVertexIDX = 1;

        Vector3 happinessVertex = Quaternion.Euler(0, 0, -angleParts * 1) * Vector3.up * radarChartSize * (VidaPersonalManager.Instance.happinessProgress / 100.0f);
        int happinessVertexIDX = 2;

        Vector3 personalVertex = Quaternion.Euler(0, 0, -angleParts * 2) * Vector3.up * radarChartSize * (VidaPersonalManager.Instance.developmentProgress / 100.0f);
        int personalVertexIDX = 3;

        Vector3 friendshipVertex = Quaternion.Euler(0, 0, -angleParts * 3) * Vector3.up * radarChartSize * (VidaPersonalManager.Instance.friendshipProgress / 100.0f);
        int friendshipVertexIDX = 4;

        Vector3 restVertex = Quaternion.Euler(0, 0, -angleParts * 4) * Vector3.up * radarChartSize * (VidaPersonalManager.Instance.friendshipProgress / 100.0f);
        int restVertexIDX = 5;


        vertices[0] = Vector3.zero;
        vertices[romanticVertexIDX] = romanticVertex;
        vertices[happinessVertexIDX] = happinessVertex;
        vertices[personalVertexIDX] = personalVertex;
        vertices[friendshipVertexIDX] = friendshipVertex;
        vertices[restVertexIDX] = restVertex;

        triangles[0] = 0;
        triangles[1] = romanticVertexIDX;
        triangles[2] = happinessVertexIDX;

        triangles[3] = 0;
        triangles[4] = happinessVertexIDX;
        triangles[5] = personalVertexIDX;

        triangles[6] = 0;
        triangles[7] = personalVertexIDX;
        triangles[8] = friendshipVertexIDX;

        triangles[9] = 0;
        triangles[10] = friendshipVertexIDX;
        triangles[11] = restVertexIDX;

        triangles[12] = 0;
        triangles[13] = restVertexIDX;
        triangles[14] = romanticVertexIDX;

        meshRadar.vertices = vertices;
        meshRadar.uv = uvs;
        meshRadar.triangles = triangles;

        meshRenderer.SetMesh(meshRadar);
        meshRenderer.SetMaterial(meshMaterial, null);
    }
}
