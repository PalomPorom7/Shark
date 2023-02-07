using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Shark : MonoBehaviour
{
    [SerializeField]
    private Vector2 swimForce = Vector2.one;

    [SerializeField]
    private float yBounds = 6.5f;

    private Rigidbody2D body;

    [SerializeField]
    private SpriteRenderer eye;
    private Color eyeColor;

    [SerializeField]
    private CircleCollider2D mouth;

    private Vector3 facingLeft = new Vector3(0, 180, 90),
                    facingRight = new Vector3(0, 0, 90);

    public int badFishCount = 0;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        mouth.enabled = false;
    }
    public void Reset()
    {
        badFishCount = 0;
        transform.position = Vector2.zero;
    }
    private void FixedUpdate()
    {
        if (transform.position.y > yBounds)
        {
            transform.position = new Vector2(transform.position.x, yBounds);
            body.velocity = new Vector2(body.velocity.x, 0);
        }
        else if (transform.position.y < -yBounds)
        {
            transform.position = new Vector2(transform.position.x, -yBounds);
            body.velocity = new Vector2(body.velocity.x, 0);
        }
    }
    public void Swim(Vector2 direction)
    {
        body.AddForce(direction * swimForce, ForceMode2D.Impulse);

        if (direction.x < 0)
            transform.eulerAngles = facingLeft;
        else if (direction.x > 0)
            transform.eulerAngles = facingRight;

    }
    public IEnumerator ChangeEyeColor(Color newColor)
    {
        mouth.enabled = false;

        for(int i = 0; i != 30; ++i)
        {
            eye.color = GameManager.Instance.GetRandomColor();
            yield return null;
        }
        eyeColor = newColor;
        eye.color = eyeColor;

        mouth.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fish")
        {
            Fish toEat = collision.GetComponent<Fish>();

            if(toEat.FishColor == eyeColor)
            {
                GameManager.Instance.AddScore(1);
            }
            else
            {
                if (++badFishCount == 3)
                    GameManager.Instance.GameOver();
                
            }
            FishSpawner.Instance.DespawnFish(toEat);
        }
    }
}
