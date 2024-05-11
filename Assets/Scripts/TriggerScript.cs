using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TriggerScript : MonoBehaviour
{
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

    public Transform particlesPool;
    private GameObject[] hitEffects;
    
    private float nextSection_z = 193f;
    private int RIGHT = 1;
    private int LEFT = -1;


    // Start is called before the first frame update
    void Start() {
        nextSection_z = 50f;
        InstantiateRoadSection(50f);

        hitEffects = new GameObject[particlesPool.childCount];	
        for(int i = 0; i < particlesPool.childCount; i++) {
            hitEffects[i] = particlesPool.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other) {
        if (other.tag == "RoadTrigger") {
            Debug.Log("Triggered");
            nextSection_z = 193f;
            InstantiateRoadSection(nextSection_z);
        } else {
            Debug.Log("Triggered obstacle");
            SpawnHit();
        }
    }

    private GameObject SpawnHit()
    {
        GameObject spawnedHit = Instantiate(hitEffects[0]);
        spawnedHit.transform.position = new Vector3(0, 2, -26);
        spawnedHit.transform.localScale = new Vector3(4, 4, 4);
        spawnedHit.transform.LookAt(Camera.main.transform);
        return spawnedHit;
    }

    public void InstantiateRoadSection(float z) {
        GameObject road = Instantiate(roadSection, new Vector3(0, 0, z), Quaternion.identity);
        InstantiateAmbient(road, RIGHT);
        InstantiateAmbient(road, LEFT);
    }

    private void InstantiateAmbient(GameObject roadSection, int side) {
        // Path
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
            GameObject rockInstance = Instantiate(rock, new Vector3(Random.Range(-7.0f, 7.0f), (Random.Range(8.0f, 15.0f)/-10.0f), Random.Range(z_minimum, z_maximum)), Quaternion.identity);
            rockInstance.transform.parent = roadSection.transform;
        }
        
        // Ground
        GameObject[] allGrounds = new GameObject[6];

        bool secondOrFirst = Random.Range(0, 2) == 1;
        GameObject ground = secondOrFirst ? ground_1 : ground_2;

        GameObject groundInstance = Instantiate(ground, new Vector3(side == RIGHT ? 26 : -26, -0.8f, nextSection_z), Quaternion.identity);
        groundInstance.transform.localScale = new Vector3(1.5f, 1f, 2f);
        groundInstance.transform.parent = roadSection.transform;
        allGrounds[0] = groundInstance;

        // Set layer
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

        // Trees
        int numberOfTrees = 90;
        Vector3[] treePositions = new Vector3[numberOfTrees];
        for (int i = 0; i < numberOfTrees; i++) {
            treePositions[i] = getRandomPointGround(90f);
        }

        for(int i = 0; i < allGrounds.Length; i++) {
            // turn off mesh collider
            if(allGrounds[i] != null){
                allGrounds[i].GetComponent<MeshCollider>().enabled = false;
            }
        }

        int maxTreesOnRoad = 6, treesOnRoad = 0;
        for (int i = 0; i < numberOfTrees; i++) {
            if(treePositions[i] == new Vector3(0, 0, 0)) continue;

            bool inRoad = SpawnTree(roadSection, treePositions[i], treesOnRoad < maxTreesOnRoad);
            if(inRoad) treesOnRoad++;
        }

        // Other things
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

        Debug.Log("Section instantiated");
    }

    bool SpawnTree(GameObject roadSection, Vector3 position, bool inRoadEnabled){
        float spawnRadius = 90f; 
        // random tree
        GameObject treeObj = tree_1;
        int randomTree = Random.Range(0, 3);
        if (randomTree == 1) {
            treeObj = tree_2;
        } else if (randomTree == 2) {
            treeObj = tree_3;
        }

        // Check if the tree is in the road
        bool hitRoad = position.x >= -7 && position.x <= 7;
        int roadOffset = 0;
        if (!inRoadEnabled && hitRoad) {
            roadOffset = Random.Range(0, 2) == 1 ? 10 : -10;
        }

        // Instantiate the tree at the hit point
        Vector3 treePosition = new Vector3(position.x + roadOffset, position.y, position.z);
        int height = Random.Range(0, 3);
        GameObject tree = Instantiate(treeObj, treePosition, Quaternion.identity);
        tree.transform.localScale = new Vector3(1 + height, 1 + height, 1 + height);
        // tree.GetComponent<Collider>().isTrigger = true;
        tree.transform.parent = roadSection.transform;

        return hitRoad;
    }

    private Vector3 getRandomPointGround(float spawnRadius){
        Vector3 randomPoint = (transform.position + Random.insideUnitSphere * spawnRadius) + new Vector3(0, 0, nextSection_z);
        RaycastHit hit;
        if (Physics.Raycast(randomPoint, Vector3.down, out hit, Mathf.Infinity, groundLayer, QueryTriggerInteraction.Ignore)){
            return hit.point;
        }

        return new Vector3(0, 0, 0);
    }

}
