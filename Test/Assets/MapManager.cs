using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour {

	public MapHex hexPrefab;
	public int mapHeight = 10;
	public int mapWidth = 10;  
	public float cellSize = 1.0f;
	public float verticalOffset;
	// Use this for initialization
	void Start () {
		float offset;
		verticalOffset = cellSize / (2.0f * Mathf.Tan(30 * Mathf.PI/180));
		for (int x=0; x<mapWidth; x++) {
			for (int z=0; z<mapHeight; z++) {
				// Offset odd lines
				offset = (z % 2 == 0) ? cellSize/2 : 0.0f;
				MapHex newHex = Instantiate(hexPrefab, new Vector3(x*cellSize+offset, 0, z*verticalOffset), Quaternion.identity);
				int[] coords = newHex.coords = new int[3] {x,0,z};
				// Debug.Log(newHex.GetType());
				newHex.transform.Find("HexCanvas").Find("HexLabel").GetComponent<Text>().text 
					= coords[0] + "\n" + coords[1] + "\n" + coords[2];
			}
		}
	}

	// MapHex GetAdjacent (direction) {

	// }
	// Update is called once per frame
	void Update () {
		
	}
}
