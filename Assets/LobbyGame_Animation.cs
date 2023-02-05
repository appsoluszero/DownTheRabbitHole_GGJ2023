using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyGame_Animation : MonoBehaviour
{
    LobbyGame_PlayerScript playerScript;
    Animator animator;
    Vector2 dir, lastDir;

    public string idleUp, idleDown, walkUp, walkDown;
    private int lookState = -1; //-1 down 1 up
    

    void Start()
    {
        animator = GetComponent<Animator>();
        playerScript = GetComponentInParent<LobbyGame_PlayerScript>();

        playerScript.walkAction.performed += ctx => {
            dir = ctx.ReadValue<Vector2>();
            if(dir.y > 0) {
                animator.Play(walkUp);
                lookState = 1;
            }
            else if(dir.y < 0) {
                animator.Play(walkDown);
                lookState = -1;
            }
            else {
                if(lookState == 1) animator.Play(walkUp);
                else animator.Play(walkDown);
            }
            if(dir.x > 0) transform.localScale = new Vector3(-1, 1, 0);
            else if(dir.x < 0) transform.localScale = new Vector3(1, 1, 0);
        };

        playerScript.walkAction.canceled += ctx => {
            if(dir.y > 0) animator.Play(idleUp);
            else if(dir.y < 0) animator.Play(idleDown);
            else {
                if(lookState == 1) animator.Play(idleUp);
                else animator.Play(idleDown);
            }
        };
    }

    void OnWalk(InputAction.CallbackContext ctx) 
    {
        
    }
} 
