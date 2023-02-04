using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyGame_PlayerScript : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction walkAction, interactAction;

    private Rigidbody2D rb;

    public float moveSpeed;

    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        walkAction = _playerInput.actions["Move"];
        interactAction = _playerInput.actions["Interact"];

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var dir = walkAction.ReadValue<Vector2>() * moveSpeed;
        rb.velocity = dir;
    }
}
