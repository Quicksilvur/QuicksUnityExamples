using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Teleporter : MonoBehaviour
{
    // The other teleporter
    public GameObject pointB;

    // The effects you're using - remove it if you don't want effects
    public GameObject effects;

    // Checks if the player has either entered/exited
    public bool entered;

    // Sprite renderer of the object, used to get the colour for your effects
    SpriteRenderer spr;
    // MATERIAL VERSION
    // Material mat;
    AudioSource audS;

    // Sound effect you're using - remove it if you don't want sound
    public AudioClip teleport;

    // Gets components - effects and sound will not work without these
    void Start()
    {
        spr = gameObject.GetComponent<SpriteRenderer>();
        audS = gameObject.GetComponent<AudioSource>();
        // mat = gameObject.GetComponent<Renderer>().material;
    }

    // Player and teleporter collision detection
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!entered)
        {
            // Moves the object that entered - change the offset if your player gets stuck inside
            float offset = 0.1f;
            collision.gameObject.transform.position = pointB.transform.position + new Vector3(0f, offset, 0f);
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);

            // Plays the sound - only if it's defined
            audS.clip = teleport;
            audS.Play();

            // Generates effects, feel free to mess with numbers but take caution as too many objects will cause lag
            for (var i = 0; i < 32; i++)
            {
                // Initialises the effect object
                GameObject baseObj = Instantiate(sparks, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-1f, 2f), 0f), transform.rotation);

                // Messes with the scale of the object
                //
                // Initialise a float if you want them all to be perfectly square/cubic
                // float scale = Random.Range(0f, 1f);
                // baseObj.transform.localScale = new Vector3(scale, scale, scale);
                baseObj.transform.localScale = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

                // Makes effects move, feel free to mess with the values
                baseObj.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));

                // Changes the colour of your effects to that of the teleporter, assuming you use a SpriteRenderer or Material with a different colour
                baseObj.GetComponent<SpriteRenderer>().color = new Color(spr.color.r, spr.color.g, spr.color.b, Random.Range(0.5f, 1f));
                // MATERIAL VERSION:
                // baseObj.GetComponent<Renderer>().material.color = new Color(mat.color.r, mat.color.g, mat.color.b, Random.Range(0.5f, 1f));
            }
        }

        // Sets both ends to be classed as entered.
        // Not setting both ends to true will make your character bounce back and forth endlessly
        // Point A has been entered
        entered = true;
        // Point B has been entered
        pointB.GetComponent<TeleporterHandler>().entered = true;
    }

    // The player has left the teleporter
    void OnCollisionExit2D(Collision2D collision)
    {
        // So we set the flag to be false
        // Point B does it automatically as you have left Point B to get to Point A, and vice versa
        entered = false;
    }
}