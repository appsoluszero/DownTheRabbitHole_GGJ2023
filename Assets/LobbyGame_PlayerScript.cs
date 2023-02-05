using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyGame_PlayerScript : MonoBehaviour
{
    [HideInInspector] public PlayerInput _playerInput;
    [HideInInspector] public InputAction walkAction, interactAction;

    private Rigidbody2D rb;
    public float moveSpeed;

    void OnEnable()
    {
        _playerInput = GetComponent<PlayerInput>();
        walkAction = _playerInput.actions["Move"];
        interactAction = _playerInput.actions["Interact"];
        interactAction.performed += OnInteract;

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var dir = walkAction.ReadValue<Vector2>() * moveSpeed;
        rb.velocity = dir;
    }

    void OnInteract(InputAction.CallbackContext ctx) 
    {
        DTRH_GameManager._instance.EnterRoom();
    }
}
