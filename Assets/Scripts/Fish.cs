using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Fish : MonoBehaviour
{
    private SpriteRenderer myRenderer;

    private Rigidbody2D body;

    [SerializeField]
    private Vector2 swimForce = Vector2.one;

    public Color FishColor
    {
        get
        {
            return myRenderer.color;
        }
        set
        {
            myRenderer.color = value;
        }
    }

    private void Awake()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
    }

    public void StartSwimming()
    {
        StartCoroutine(SwimAround());
    }

    public IEnumerator SwimAround()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            Swim(Random.insideUnitCircle);
        }
    }

    public void Swim(Vector2 direction)
    {
        body.AddForce(direction * swimForce, ForceMode2D.Impulse);
    }

}
