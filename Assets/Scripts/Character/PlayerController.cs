using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;
    public LayerMask grassLayer;

    public bool isMoving;
    private Vector2 input;
    private Animator animator;
    private bool questItemFound;
    public QuestItem WaterBottle;
    public QuestItem Flamethrower;
    public bool questActive;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            //Remove diagonal movement
            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);
                
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (isWalkable(targetPos))
                    StartCoroutine(Move(targetPos));
            }
        }

        animator.SetBool("isMoving", isMoving);

        if (Input.GetKeyDown(KeyCode.Z))
            Interact();

        if (!questItemFound && WaterBottle.itemCollected)
        {
            questItemFound = true;
            Destroy(WaterBottle.gameObject);
            questActive = true;
        }
        
        if (!questItemFound && Flamethrower.itemCollected)
        {
            questItemFound = true;
            Destroy(Flamethrower.gameObject);
            questActive = true;
        }

    }

    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat(("moveX")), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;
        
        var collider = Physics2D.OverlapCircle(interactPos, 0.1f, interactableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            var sprint = false;
            if(Input.GetKeyDown((KeyCode.LeftShift)))
            {
                moveSpeed = 16;
                sprint = true;
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                moveSpeed = 4;
            }
            yield return null;
        }
        transform.position = targetPos;
        
        isMoving = false;

        CheckForEncounters();
    }

    private bool isWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.1f, solidObjectsLayer | interactableLayer) != null)
        {
            return false;
        }

        return true;
    }

    private void CheckForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
        {
            int rnd = Random.Range(1, 101);
            if (Random.Range(1, 101) <= 10)
            {
                Debug.Log("Encountered a creature");
            }
        }
    }
}

