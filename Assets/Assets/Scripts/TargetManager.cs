using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour {

    // code for random spawn on terrain from http://forum.brackeys.com/thread/random-objects/

    public GameObject targetPrefab;
    public Terrain terrain;
    public int numOfTargets = 30;

    private float terrainWidth,
                    terrainLength,
                    terrainPosX,
                    terrainPosZ;
    private const float targetHeightHalf = 0.4f;

	public static TargetManager inst;

	private float t, timeToReachTarget;
	private bool makeTargetRun = false;
	private GameObject target = null;
	private Vector3 startPosition;
	private Vector3 endPosition;

	// private float xDir

	void Awake () {
		if (!inst)
			inst = this;
	}

	void OnDestroy () {
		if (inst) {
			inst = null;
		}
	}

	// Use this for initialization
	void Start () {
        // terrain size x
        //terrainWidth = (int)terrain.terrainData.size.x;
        terrainWidth = 290;
        // terrain size z
        //terrainLength = (int)terrain.terrainData.size.z;
        terrainLength = 330;
        // terrain x position
        terrainPosX = (int)terrain.transform.position.x + 86;
        // terrain z position
        terrainPosZ = (int)terrain.transform.position.z + 69;
        SpawnPrefabs();
    }
	
	// Update is called once per frame
	void Update () {
		//if (makeTargetRun && target) {
		//	t += Time.deltaTime / timeToReachTarget;
		//	target.transform.position = Vector3.Lerp(startPosition, endPosition, t);
		//}
	}

	// need to add a check to see if it spawns in a tree
	public void SpawnPrefabs() {
		for (int i = 0; i < numOfTargets; i++) {
            // generate random x position
            float posx = Random.Range(terrainPosX, terrainPosX + terrainWidth);
            // generate random z position
            float posz = Random.Range(terrainPosZ, terrainPosZ + terrainLength);
            // get the terrain height at the random position
            float posy = Terrain.activeTerrain.SampleHeight(new Vector3(posx, 0, posz));
            // create new gameObject on random position
            Instantiate(targetPrefab, new Vector3(posx, posy + targetHeightHalf, posz), Quaternion.identity);
		}
	}

	public void MakeTargetRun(GameObject tar, Vector3 hitPos) {
		// see where hit and move away

		//target = tar;
		//makeTargetRun = true;
		//startPosition = target.transform.position;
		//t = 0f;
		//timeToReachTarget = 0.5f;
		//endPosition = startPosition + new Vector3(0.0f, 0.0f, 1f);
	}
}
