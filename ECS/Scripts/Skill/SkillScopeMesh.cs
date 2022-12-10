using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class SkillScopeMesh : MonoBehaviour
{
    public float Radius = 0.3f;                //外圈的半径
    public float Thickness = 0.05f;             //厚度，外圈半径减去内圈半径
    public float Depth = 1.0f;                  //厚度
    public float NumberOfSides = 60.0f;         //由多少个面组成
    public float DrawArchDegrees = 360.0f;       //要绘画多长
    public Material archMaterial = null;
    public int n;

    private List<Vector3> vertexList = new List<Vector3>();
    private List<int> triangleList = new List<int>();
    private List<Vector2> uvList = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {

        GenerateVertex();
    }

    void GenerateVertex()
    {

        //顶点坐标
        vertexList.Clear();
        float incrementAngle = DrawArchDegrees / NumberOfSides;
        //小于等于是因为n+1条线才能组成n个面
        for (int i = 0; i <= NumberOfSides; i++)
        {
            float angle = 180 - i * incrementAngle;
            float innerX = (Radius - Thickness) * Mathf.Cos(angle * Mathf.Deg2Rad);
            float innerZ = (Radius - Thickness) * Mathf.Sin(angle * Mathf.Deg2Rad);
            vertexList.Add(new Vector3(innerX, 0, innerZ));
            float outsideX = Radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float outsideZ = Radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            vertexList.Add(new Vector3(outsideX, 0, outsideZ));
        }

        //三角形索引
        triangleList.Clear();
        int direction = 1;
        for (int i = 0; i < NumberOfSides * 2; i++)
        {
            int[] triangleIndexs = getTriangleIndexs(i, direction);
            direction *= -1;
            for (int j = 0; j < triangleIndexs.Length; j++)
            {
                triangleList.Add(triangleIndexs[j]);
            }
        }

        //UV索引
        uvList.Clear();
        for (int i = 0; i <= NumberOfSides; i++)
        {
            float angle = 180 - i * incrementAngle;
            float littleX = (1.0f / NumberOfSides) * i;
            uvList.Add(new Vector2(littleX, 0));
            float bigX = (1.0f / NumberOfSides) * i;
            uvList.Add(new Vector2(bigX, 1));
        }
        Mesh mesh = new Mesh()
        {
            vertices = vertexList.ToArray(),
            uv = uvList.ToArray(),
            triangles = triangleList.ToArray(),
        };

        mesh.RecalculateNormals();
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshRenderer>().material = archMaterial;
    }

    int[] getTriangleIndexs(int index, int direction)
    {
        int[] triangleIndexs = new int[3] { 0, 1, 2 };
        for (int i = 0; i < triangleIndexs.Length; i++)
        {
            triangleIndexs[i] += index;
        }
        if (direction == -1)
        {
            int temp = triangleIndexs[0];
            triangleIndexs[0] = triangleIndexs[2];
            triangleIndexs[2] = temp;
        }
        return triangleIndexs;
    }

    //绘制扇形
    public static GameObject go;
    public static MeshFilter mf;
    public static MeshRenderer mr;
    public static Shader shader;


    //绘制圆环


    void Update()
    {
        ToDrawSectorSolid(transform, transform.localPosition, n, Radius);
        
    }




    //绘制实心扇形
    public static void ToDrawSectorSolid(Transform t, Vector3 center, float angle, float radius)

    {

        int pointAmount = 100;//点的数目，值越大曲线越平滑   

        float eachAngle = angle / pointAmount;

        Vector3 forward = t.forward;

        List<Vector3> vertices = new List<Vector3>();

        vertices.Add(center);

        for (int i = 1; i < pointAmount - 1; i++)

        {

            Vector3 pos = Quaternion.Euler(0f, -angle / 2 + eachAngle * (i - 1), 0f) * forward * radius + center;

            vertices.Add(pos);

        }

        CreateMesh(vertices);

    }

    private static GameObject CreateMesh(List<Vector3> vertices)

    {

        int[] triangles;

        Mesh mesh = new Mesh();

        int triangleAmount = vertices.Count - 2;

        triangles = new int[3 * triangleAmount];

        //根据三角形的个数，来计算绘制三角形的顶点顺序（索引）   

        //顺序必须为顺时针或者逆时针      

        for (int i = 0; i < triangleAmount; i++)

        {

            triangles[3 * i] = 0;//固定第一个点      

            triangles[3 * i + 1] = i + 1;

            triangles[3 * i + 2] = i + 2;

        }

        if (go == null)

        {

            go = new GameObject("mesh");

            go.transform.position = new Vector3(0, 0.03f, 0);//让绘制的图形上升一点，防止被地面遮挡  

            mf = go.AddComponent<MeshFilter>();

            mr = go.AddComponent<MeshRenderer>();
            go.transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
            go.transform.localPosition = Vector3.zero+ new Vector3(0, 0.03f, 0);
            go.transform.localEulerAngles = Vector3.zero;
            //shader = Shader.Find("Standard"); 
            shader = Shader.Find("Unlit/Color");

        }
        //接受投影
        //mr.receiveShadows = true;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles;
        mf.mesh = mesh;
        mr.material.shader = shader;
        mr.material.color = Color.red;

        return go;
    }

}
