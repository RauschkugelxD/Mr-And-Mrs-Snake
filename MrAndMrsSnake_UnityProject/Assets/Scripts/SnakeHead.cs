using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    public SnakeMovement Snake;
    public SpawnGameObject SO;

    // Variables for data analysis
    [HideInInspector]
    public static int AppleCount = 0;
    [HideInInspector]
    public static float InitialTime = 0.0f;
    [HideInInspector]
    public static double TimeSurvived = 0.0;

    void Start()
    {
        InitialTime = Time.time;
    }

    void OnCollisionEnter(Collision col)
    {
        // Collision with food
        if (col.gameObject.CompareTag("Food")) 
        {
            AppleCount++;
            Snake.AddBodyPart();
            SO.RespawnFood();
        }

        // Collision with wall
        else
        {
            if (col.transform != Snake.BodyParts[1] && Snake.IsAlive) 
            {
                if (Time.time - Snake.TimeFromLastRetry > 2.8)
                {
                    float time = Time.time - InitialTime;
                    TimeSurvived = System.Math.Round(time, 2);
                    Snake.Die();
                }
            }
        }
    }
}
