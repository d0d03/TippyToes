using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 6;
    float screenHalfWidthInWorldUnits;
    public event System.Action OnPlayerDeath;
    public event System.Action OnDoorHacked;
    public Camera camera;
    float hackingTime;
    public Text TimeToSurvive;

    // Start is called before the first frame update
    void Start()
    {
        float halfPlayerWidth = transform.localScale.x / 2f;
        screenHalfWidthInWorldUnits = camera.aspect * camera.orthographicSize + halfPlayerWidth;
        hackingTime = PlayerPrefs.GetFloat("startTime");
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float velocity = inputX * speed;

        transform.Translate(Vector2.right *velocity * Time.deltaTime);
        TimeToSurvive.text = ((int)(10 - (Time.timeSinceLevelLoad - hackingTime))).ToString();

        //aspect ration = širina ekrana(px)/ visina ekrana(px)
        //ortographic size = visina ekrana(wu)/2
        //polovina širine ekrana = aspect ration * ortographic size = screen width(wu)/2
        if (transform.localPosition.x < -screenHalfWidthInWorldUnits)
        {
            transform.localPosition = new Vector3(screenHalfWidthInWorldUnits, transform.localPosition.y,transform.localPosition.z);
        }

        if (transform.localPosition.x > screenHalfWidthInWorldUnits)
        {
            transform.localPosition = new Vector3(-screenHalfWidthInWorldUnits, transform.localPosition.y,transform.localPosition.z);
        }

        if (Time.timeSinceLevelLoad - hackingTime >= 10)
        {
            if (OnDoorHacked != null)
            {
                OnDoorHacked();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D trigerCollider)
    {
        if (trigerCollider.tag == "Falling Block") {
            //FindObjectOfType<GameOver>().OnGameOver(); nema smisla da bude tu bolje subscribe napraviti 
            if (OnPlayerDeath != null) {
                OnPlayerDeath();
            }
            if (gameObject != null) {
                Destroy(gameObject);
            }
        }
    }
}
