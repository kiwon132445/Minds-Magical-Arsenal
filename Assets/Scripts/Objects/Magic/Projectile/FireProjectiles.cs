using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectiles : MonoBehaviour
{
    public GameObject barrel;
    public List<GameObject> projectilePrefabs;
    public Camera mainCamera;
    public Dictionary<string, int> nameToIndex = new Dictionary<string, int>
    {
        {"Fire Arrow", 0},
        {"Fire Ball",  1},
        {"Ice Arrow", 2},
        {"Freeze Ball", 3},
        {"Poison Arrow", 4},
        {"Poison Ball", 5},
        {"Earth Arrow", 6},
        {"Earth Ball", 7},
    };

    private GameObject projectileToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        projectileToSpawn = projectilePrefabs[0];
    }

    public void FireProjectile(string name)
    {
        GameObject vfx;
        if (barrel != null)
        {
            projectileToSpawn = projectilePrefabs[nameToIndex[name]];
            vfx = Instantiate(projectileToSpawn, barrel.transform.position, gameObject.transform.rotation);
        }
        else
        {
            Debug.Log("No barrel to launch spell");
        }

    }
}
