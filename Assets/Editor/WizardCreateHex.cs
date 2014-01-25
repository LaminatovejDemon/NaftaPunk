using UnityEngine;
using UnityEditor;
using System.Collections;


public class WizardCreateHex : ScriptableWizard
{
    public string optionalName;
	public float radius;
    public bool createAtOrigin = true;
	public bool addCollider = false;

    static Camera cam;
    static Camera lastUsedCam;
	
    [MenuItem("GameObject/Create Other/Custom Hex...")]
    static void CreateWizard()
    {
        cam = Camera.current;
        // Hack because camera.current doesn't return editor camera if scene view doesn't have focus
        if (!cam)
            cam = lastUsedCam;
        else
            lastUsedCam = cam;
        ScriptableWizard.DisplayWizard("Create Hex",typeof(WizardCreateHex));
    }
      
    void OnWizardCreate()
    {
        GameObject hex = new GameObject();
       
        if (!string.IsNullOrEmpty(optionalName))
            hex.name = optionalName;
        else
            hex.name = "Hex";
       
        if (!createAtOrigin && cam)
            hex.transform.position = cam.transform.position + cam.transform.forward*5.0f;
        else
            hex.transform.position = Vector3.zero;
       
        MeshFilter meshFilter = (MeshFilter)hex.AddComponent(typeof(MeshFilter));
        hex.AddComponent(typeof(MeshRenderer));

        string hexAssetName = hex.name + "_r" + radius.ToString() + ".asset";
        Mesh m = (Mesh)AssetDatabase.LoadAssetAtPath("Assets/Meshes/" + hexAssetName,typeof(Mesh));
 
        if (m == null)
        {
            m = new Mesh();
            m.name = hex.name;
       
            int numTriangles = 6*3;
            int numVertices = 7;
       
            Vector3[] vertices = new Vector3[numVertices];
            Vector2[] uvs = new Vector2[numVertices];
            int[] triangles = new int[numTriangles];
       
			// vertices
			vertices[0] = new Vector3(0, 0, 0);
			uvs[0] = new Vector2(0.5f, 0.5f);
			for( int i = 1; i < 7; ++i )
			{
				float alpha = i*60f*Mathf.PI/180f;
				float x = Mathf.Cos(alpha);
				float y = Mathf.Sin(alpha);
				vertices[i] = new Vector3(radius*x, 0, radius*y);
				uvs[i] = new Vector3((x+1f)/2f, (y+1f)/2f, 0);
			}

			// triangles
			for( int i = 0; i < 6; ++i )
			{
				triangles[i*3] = 0;
				triangles[i*3+1] = 1+i;
				triangles[i*3+2] = 1+(i+1)%6;
			}
       
            m.vertices = vertices;
            m.uv = uvs;
            m.triangles = triangles;
            m.RecalculateNormals();
           
            AssetDatabase.CreateAsset(m, "Assets/Editor/" + hexAssetName);
            AssetDatabase.SaveAssets();
        }
       
        meshFilter.sharedMesh = m;
        m.RecalculateBounds();
       
        if (addCollider)
            hex.AddComponent(typeof(BoxCollider));
       
        Selection.activeObject = hex;
    }
}