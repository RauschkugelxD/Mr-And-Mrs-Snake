using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class SnakeMovement : MonoBehaviour
{
    // Snake body
    public List<Transform> BodyParts = new List<Transform>();
    public float MinDistance;
    public int BeginSize;
    [HideInInspector]
    public GameObject BodyPrefab;
    public GameObject BodyPrefabGreen;
    public GameObject BodyPrefabBlue;

    // Selectable snake heads
    public GameObject MrSnakeBlue;
    public GameObject MrSnakeGreen;
    public GameObject MrsSnakeBlue;
    public GameObject MrsSnakeGreen;
    public GameObject Player;

    public static bool IsMrSnake = false;
    public static bool IsBlue = false;

    // Body parts of the snake
    private float distance;
    private Transform curBodyPart;
    private Transform preBodyPart;

    // Physics
    public float Speed;
    [HideInInspector]
    public float TimeFromLastRetry;

    // User interface
    public TMP_Text ScoreGame;
    public TMP_Text ScoreDeath;
    public TMP_Text TimeSurvived;
    public GameObject DeathScreen;

    public bool IsAlive;

    // Start is called before the first frame update
    void Start()
    {
        StartLevel();
    }

    public void StartLevel()
    {
        // Set the female or male snake head active, based on the user input
        if (IsBlue)
        {
            if (IsMrSnake) 
                MrSnakeBlue.SetActive(true); 
            else 
                MrsSnakeBlue.SetActive(true); 

            BodyPrefab = BodyPrefabBlue;
        }
        else
        {
            if (IsMrSnake)
                MrSnakeGreen.SetActive(true); 
            else 
                MrsSnakeGreen.SetActive(true); 

            BodyPrefab = BodyPrefabGreen;
        }        

        // Make snake head and first body part do not collide at the beginning (visual collision issue when spawning )
        Player.layer = LayerMask.NameToLayer("Ignore Raycast");

        // Remove body parts of last game (counting backwards)
        for (int i = BodyParts.Count - 1; i > 0; i--)
        {
            Destroy(BodyParts[i].gameObject);
            BodyParts.Remove(BodyParts[i]);
        }

        // Start position of snake head
        BodyParts[0].position = new Vector3(0, -0.04f, -10); 
        BodyParts[0].rotation = Quaternion.Euler(0, 90, 0);  // Quaternion.identity;

        // Time buffer: when new body part is spawning do not touch own body by accident 
        TimeFromLastRetry = Time.time;
        for (int i = 0; i < BeginSize - 1; i++)
            AddBodyPart();

        // Reset analysis data
        SnakeHead.AppleCount = 0;
        SnakeHead.TimeSurvived = 0.0;
        SnakeHead.InitialTime = Time.time;

        // UI elements
        DeathScreen.SetActive(false);
        ScoreGame.gameObject.SetActive(true);

        // Add snake head to layer "Player" after 2s
        StartCoroutine(SetLayer(2));

        // Start game
        IsAlive = true;
        Time.timeScale = 1;
    }

    // FixedUpdate() for better performance of object movement
    void FixedUpdate()
    {
        if (IsAlive)
            Move();
    }

    public void Move()
    {
        float curSpeed = Speed;

        // Movement Snake
        BodyParts[0].Translate(BodyParts[0].forward * curSpeed * Time.smoothDeltaTime, Space.World); 

        for (int i = 1; i<BodyParts.Count; i++)
        {
            curBodyPart = BodyParts[i];
            preBodyPart = BodyParts[i - 1];

            distance = Vector3.Distance(preBodyPart.position, curBodyPart.position);

            Vector3 newPos = preBodyPart.position;
            // Lock the y-position of the body parts
            newPos.y = 0;

            // In case body parts move inside each other
            float T = Time.deltaTime * distance / MinDistance * curSpeed;
            if (T > 0.6f)
                T = 0.6f;

            // Scale, position and rotation of the added body parts
            curBodyPart.position = Vector3.MoveTowards(curBodyPart.position, newPos, T); // alternative: Slerp(_curBodyPart.position, newPos, T); 
            curBodyPart.rotation = Quaternion.Slerp(curBodyPart.rotation, preBodyPart.rotation, T);
        }
    }

    public void AddBodyPart()
    {
        Transform newPart = (Instantiate(BodyPrefab, BodyParts[BodyParts.Count-1].position, BodyParts[BodyParts.Count - 1].rotation) as GameObject).transform;
        newPart.SetParent(transform);
        BodyParts.Add(newPart);
        ScoreGame.text = "SCORE: " + SnakeHead.AppleCount;  
    }

    public void Die()
    {
        Time.timeScale = 0;
        IsAlive = false;
        ScoreDeath.text = "YOUR SCORE: " + SnakeHead.AppleCount;
        TimeSurvived.text = "TIME SURVIVED: " + SnakeHead.TimeSurvived + "s";
        ScoreGame.gameObject.SetActive(false);
        DeathScreen.SetActive(true);
    }

    // Change layer name after some seconds
    IEnumerator SetLayer(int secs)
    {
        yield return new WaitForSeconds(secs);
        Player.layer = LayerMask.NameToLayer("Player");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
