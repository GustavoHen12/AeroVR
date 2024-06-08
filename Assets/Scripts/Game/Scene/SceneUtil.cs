using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneUtil : MonoBehaviour {
    // Game Objects
    public GameObject roadSection;
    public GameObject montain_1;
    public GameObject montain_2;
    public GameObject ground_1;
    public GameObject ground_2;

    public GameObject tree_1;
    public GameObject tree_2;
    public GameObject tree_3;
    
    public GameObject rock;
    public GameObject fence_1;
    public GameObject fence_2;
    public GameObject bush_1;
    public GameObject bush_2;
    public GameObject flower_1;
    public GameObject grass_1;

    int scene_objects_size = 4;
        
    public LayerMask groundLayer;

    private float nextSection_z = 193f;
    
    private int RIGHT = 1;
    private int LEFT = -1;

    public int forestDensityLevel = 5;
    // Constants
    public static float LEFT_LIMIT = -7;
    public static float RIGHT_LIMIT = 7;

    // Start is called before the first frame update
    void Start() { }

    public void setForestDensityLevel(int density){
        forestDensityLevel = density;
    }
    // Update is called once per frame
    void Update() { }

    public void InstantiateRoadSection(float z) {
        nextSection_z = z;
        GameObject road = Instantiate(roadSection, new Vector3(0, 0, z), Quaternion.identity);
        InstantiateAmbient(road, RIGHT);
        InstantiateAmbient(road, LEFT);
    }

    private void InstantiateAmbient(GameObject roadSection, int side) {
        InstantiatePath(roadSection, side);
        GameObject[] grounds = InstantiateGround(roadSection, side);
        InstantiateTrees(roadSection, side);
        InstantiateDecorativeObjects(roadSection, side);
        TurnOffMeshCollider(grounds);

        Debug.Log("Section instantiated");
    }

    private void InstantiatePath(GameObject roadSection, int side){
        float z_minimum = nextSection_z - 40;
        float z_maximum = nextSection_z + 90;
        
        int numberOfFences = 15;
        for (int i = 0; i < numberOfFences; i++) {
            GameObject fence = Random.Range(0, 2) == 1 ? fence_1 : fence_2;
            GameObject fenceInstance = Instantiate(fence, new Vector3(side == RIGHT ? 8 : -8, -0.6f, Random.Range(z_minimum, z_maximum)), Quaternion.identity);
            fenceInstance.transform.rotation = Quaternion.Euler(0, 90, 0);
            fenceInstance.transform.parent = roadSection.transform;
        }

        int numberOfRocks = 50;
        for (int i = 0; i < numberOfRocks; i++) {
            GameObject rockInstance = Instantiate(rock, new Vector3(Random.Range(LEFT_LIMIT, RIGHT_LIMIT), (Random.Range(8.0f, 15.0f)/-10.0f), Random.Range(z_minimum, z_maximum)), Quaternion.identity);
            rockInstance.transform.parent = roadSection.transform;
        }
    }

    private GameObject[] InstantiateGround(GameObject roadSection, int side) {
        GameObject[] allGrounds = new GameObject[6];

        bool secondOrFirst = Random.Range(0, 2) == 1;
        GameObject ground = secondOrFirst ? ground_1 : ground_2;

        GameObject groundInstance = Instantiate(ground, new Vector3(side == RIGHT ? 26 : -26, -0.8f, nextSection_z), Quaternion.identity);
        groundInstance.transform.localScale = new Vector3(1.5f, 1f, 2f);
        groundInstance.transform.parent = roadSection.transform;
        allGrounds[0] = groundInstance;

        // Montains
        float[] montains_z = new float[] {-10, 20, 40, 60};
        for (int i = 0; i < montains_z.Length; i++) {
            GameObject montain = secondOrFirst ? montain_1 : montain_2;
            montain.layer = 7;
            int height = Random.Range(0, 5);
            int x_offset = Random.Range(0, 3);
            int z_offset = Random.Range(0, 3);
            GameObject montainInstance = Instantiate(montain, new Vector3(side == RIGHT ? 50 + x_offset : -50 - x_offset, -2, nextSection_z + montains_z[i] + z_offset), Quaternion.identity);
            montainInstance.transform.localScale = new Vector3(1, 1 + height, 2);
            montainInstance.transform.parent = roadSection.transform;
            allGrounds[i + 1] = montainInstance;
        }

        return allGrounds;
    }

    private void TurnOffMeshCollider(GameObject[] objects){
        for(int i = 0; i < objects.Length; i++) {
            if(objects[i] != null){
                objects[i].GetComponent<MeshCollider>().enabled = false;
            }
        }
    }

    private void InstantiateDecorativeObjects(GameObject roadSection, int side) {
        int numberOfThings = 50;
        for(int i = 0; i < numberOfThings; i++) {
            Vector3 point = getRandomPointGround(40f);
            if(point == new Vector3(0, 0, 0)) continue;

            int randomObject = Random.Range(0, scene_objects_size);
            GameObject game_obj = bush_1;
            if (randomObject == 1) {
                game_obj = bush_2;
            } else if (randomObject == 2) {
                game_obj = flower_1;
            } else if (randomObject == 3) {
                game_obj = grass_1;
            }

            int scale = Random.Range(0, 3);
            GameObject obj = Instantiate(game_obj, point, Quaternion.identity);
            obj.transform.localScale = new Vector3(1 + scale, 1 + scale, 1 + scale);
            obj.transform.parent = roadSection.transform;
        }    
    }

    private void InstantiateTrees(GameObject roadSection, int side) {
        // Trees
        int numberOfTrees = forestDensityLevel*10, maxTreesOnRoad = forestDensityLevel;
        Vector3[] treePositions = new Vector3[numberOfTrees];
        int i = 0, treesOnRoad = 0;
        while (i < numberOfTrees) {
            Vector3 treePosition = getRandomPointGround(90f);
            if(treePosition == new Vector3(0, 0, 0)) continue;

            if(inRoadPosition(treePosition)){
                if(treesOnRoad < maxTreesOnRoad){
                    treesOnRoad++;
                } else {
                    int roadOffset = Random.Range(0, 2) == 1 ? 10 : -10;
                    treePosition = new Vector3(treePosition.x + roadOffset, treePosition.y, treePosition.z);
                }
            }

            treePositions[i] = treePosition;
            i++;
        }

        if(treesOnRoad < maxTreesOnRoad){
            int treesToAdd = maxTreesOnRoad - treesOnRoad;
            while (treesToAdd > 0) {
                int position = Random.Range(0, numberOfTrees);
                if(!inRoadPosition(treePositions[position])){
                    float roadPosition = Random.Range(LEFT_LIMIT, RIGHT_LIMIT);
                    treePositions[position] = new Vector3(roadPosition, 0, treePositions[position].z);
                    treesToAdd--;
                }
            }
        }
        
        for (int j = 0; j < numberOfTrees; j++) {
            SpawnTree(roadSection, treePositions[j]);
        }
    }

    private static bool inRoadPosition (Vector3 position) {
        return position.x >= (LEFT_LIMIT - 2) && position.x <= (RIGHT_LIMIT + 2);
    }

    private void SpawnTree(GameObject roadSection, Vector3 position){        
        // random tree
        GameObject treeObj = tree_1;
        int randomTree = Random.Range(0, 3);
        if (randomTree == 1) {
            treeObj = tree_2;
        } else if (randomTree == 2) {
            treeObj = tree_3;
        }

        // Instantiate the tree at the hit point
        Vector3 treePosition = new Vector3(position.x, position.y, position.z);
        int height = Random.Range(0, 3);
        GameObject tree = Instantiate(treeObj, treePosition, Quaternion.identity);
        tree.transform.localScale = new Vector3(1 + height, 1 + height, 1 + height);
        tree.transform.parent = roadSection.transform;
    }

    private Vector3 getRandomPointGround(float spawnRadius){
        Vector3 randomPoint = (transform.position + Random.insideUnitSphere * spawnRadius) + new Vector3(0, 0, nextSection_z);
        RaycastHit hit;
        if (Physics.Raycast(randomPoint, Vector3.down, out hit, Mathf.Infinity, groundLayer, QueryTriggerInteraction.Ignore)) {
            return hit.point;
        }

        return new Vector3(0, 0, 0);
    }
}
