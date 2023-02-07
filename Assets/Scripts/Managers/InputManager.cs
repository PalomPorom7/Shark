using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Shark player;

    private Vector2 inputDirection;

    private void Update()
    {
        inputDirection.x = Input.GetAxis("Horizontal");
        inputDirection.y = Input.GetAxis("Vertical");
    }
    private void FixedUpdate()
    {
        player.Swim(inputDirection);
    }
}
