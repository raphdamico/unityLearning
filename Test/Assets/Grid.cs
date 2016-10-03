using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour {
	
	// public float animTime {get; private set; }
	public float waterSpeed = 0.4f;
	public float waterHeightScale = 0.1f;
	private Mesh mesh;
	public int xSize, zSize;
	// Use this for initialization
	private void Awake() {
		Generate();	
	}
	private float[] waterCells;
	private Vector3[] vertices;
	private void Generate() {
		GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		mesh.name = "Procedural Grid";
		vertices = new Vector3[(xSize+1)*(zSize+1)];
		waterCells = new float[vertices.Length];
		Vector2[] uv = new Vector2[vertices.Length];
		for (int i=0, z=0; z<=zSize; z++) {
			for (int x=0; x<=xSize; x++, i++) {
				waterCells[i] = Random.Range(0f, 1f);
				vertices[i] = new Vector3(x, GetWaterHeightFromCellValue(waterCells[i]), z);
				uv[i] = new Vector2((float)x/xSize, (float)z/zSize);
			}
		}
		mesh.vertices = vertices;
		mesh.uv = uv;
		RecalculateTriangles();

		GetComponent<MeshCollider>().sharedMesh = mesh;
	}
	private void RecalculateTriangles() {
		// vi = vertex index, ti = triangle index 
		int[] triangles = new int[xSize * zSize * 6];
		for (int z=0, vi=0, ti=0; z<zSize; z++, vi++) {
			for (int x=0; x<xSize; vi++, ti+=6, x++) {
				triangles[ti] 						= vi;
				triangles[ti+1] = triangles[ti+4] 	= vi+xSize+1;
				triangles[ti+2] = triangles[ti+3] 	= vi+1;
				triangles[ti+5] 					= vi+xSize+2;
			}
		}
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
	}
	float GetWaterHeightFromCellValue(float waterCellValue) {
		return Mathf.Cos(waterCellValue * Mathf.PI * 2) * waterHeightScale;
	} 
	IEnumerator AnimateWater() {
		yield return new WaitForSeconds(0.0f);

		for (int i = 0; i<mesh.vertices.Length; i++) {
			waterCells[i] += Time.deltaTime * waterSpeed;
			if (waterCells[i] > 1) {
				waterCells[i] -= 1;
			}
			vertices[i].y = GetWaterHeightFromCellValue(waterCells[i]);
		}
		mesh.vertices = vertices;
		GetComponent<MeshCollider>().sharedMesh = mesh;
	}

	private void OnDrawGizmos() {
		if (vertices == null) {
			return;
		}
		Gizmos.color = Color.red;
		for (int i=0; i<vertices.Length; i++) {
			Gizmos.DrawSphere(transform.TransformPoint(vertices[i]), 0.1f);
		}
	}

	private void Update() {
		StartCoroutine(AnimateWater());
	}
}
