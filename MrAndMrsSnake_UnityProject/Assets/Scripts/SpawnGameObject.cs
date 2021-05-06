using UnityEngine;

public class SpawnGameObject : MonoBehaviour
{
    public GameObject FoodPrefab;
    private GameObject foodObject;
    public Vector3 Center;
    public Vector3 Size;

    void Start()
    {
        SpawnFood();
    }

    // Instantiate food at random positions
    public void SpawnFood()
    {
        // Position random on every axis
        Vector3 pos = Center + new Vector3(Random.Range(-Size.x/2, Size.x/2), Size.y, Random.Range(-Size.z / 2, Size.z / 2));
        foodObject = Instantiate(FoodPrefab, pos, Quaternion.identity);
    }

    // For better visualization:  red, transparent spawn field 
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f); 
        Gizmos.DrawCube(Center, Size);
    }

    // Destroy the apple on restart and spawn a new one
    public void RespawnFood()
    {
        Destroy(foodObject);
        SpawnFood();
    }
}
