using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform interactPoint;

    [SerializeField] LayerMask ground;


    private readonly float gravity = -9.81f;
    private readonly float speed = 10f;
    private readonly float jumpHeight = 3f;

    private float friction;
    private Vector3 verticalVelocity = Vector3.zero;
    private bool isGrounded;

    private GameObject selectedObject;

    [SerializeField] private float attackDamage;
    [SerializeField] private float attackCooldownMax;
    private float attackCooldownTimer = 0;

    public event EventHandler OnPlayerHasFallen;

    private bool paused = false;
    private float t = 0;

    private void Awake()
    {
        GameManager.Instance.OnGameOver += GameInputProcessor_OnGamePause;
        GameInputProcessor.Instance.OnGamePause += GameInputProcessor_OnGamePause;
        GameInputProcessor.Instance.OnPlayerJump += GameInputProcessor_OnPlayerJump;
        GameInputProcessor.Instance.OnPlayerAttack += GameInputProcessor_OnPlayerAttack;
        GameInputProcessor.Instance.OnPlayerInteract += GameInputProcessor_OnPlayerInteract;
    }





    private void GameInputProcessor_OnGamePause(object sender, System.EventArgs e)
    {
        paused = !paused;
    }


    private void GameInputProcessor_OnPlayerJump(object sender, System.EventArgs e)
    {
        if (paused) { return; }
        if (isGrounded)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -6*gravity);
        }
    }

    private void GameInputProcessor_OnPlayerInteract(object sender, System.EventArgs e)
    {
        if (paused) { return; }

        if (selectedObject != null && selectedObject.TryGetComponent<IHaveInteract>(out IHaveInteract interactableObject))
        {
            interactableObject.Interact();
        }
    }

    private void GameInputProcessor_OnPlayerAttack(object sender, System.EventArgs e)
    {
        if (attackCooldownTimer < attackCooldownMax) { return; }
        attackCooldownTimer = 0;


        if (selectedObject != null && selectedObject.TryGetComponent<IHaveDamage>(out IHaveDamage damageableObject))
        {
            if (damageableObject.Damage(attackDamage))
            {
                // object is destroyed
                selectedObject = null;

            }
        }
        else
        {
            // if implementing ranged attacks
            // Spawn Attack Projectile Object
            // in attack direction


        }
    }

    // Update is called once per frame
    void Update()
    {
        if (paused) { return; }


        UpdateAttackCooldown();
        CheckIsGrounded();
        GetFriction();
        FindSelectedObject();
        Move();

        if (CheckIsFallen())
        {
            OnPlayerHasFallen?.Invoke(this, null);
        }


    }




    private void CheckIsGrounded()
    {
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, 0.5f, ground);
    }

    private bool CheckIsFallen()
    {
        return interactPoint.position.y < -5;
    }

    public void GetFriction()
    {
        if (isGrounded)
        {
            Physics.Raycast(groundCheck.position, Vector3.down, out RaycastHit hitInfo, 0.5f, ground);

            friction = hitInfo.transform != null ? hitInfo.transform.GetComponent<Floor>().GetFriction() : 0;
        }
        else
        {
            friction = 0;
        }
    }


    private void Move()
    {
        if (t < 1.5)
        {
            t += Time.deltaTime;
            return;
        }

        Vector2 inputVector = GameInputProcessor.Instance.GetInputMovementVectorNormalized();
        Vector3 moveDir = (transform.right * inputVector.x) + (transform.forward * inputVector.y);


        if (!isGrounded)
        {
            verticalVelocity.y += 2* gravity * Time.deltaTime;
        }
        else if (isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = 0f;
        }

        Vector3 moveVector = ((speed - friction) * moveDir) + verticalVelocity;
        characterController.Move(moveVector * Time.deltaTime);
    }



    private void FindSelectedObject()
    {
        selectedObject = Physics.SphereCast(transform.position, 0.1f, interactPoint.forward, out RaycastHit hitInfo, 5f)
            ? hitInfo.transform.gameObject
            : null;
    }

    private void UpdateAttackCooldown()
    {
        if (attackCooldownTimer < attackCooldownMax)
        {
            attackCooldownTimer += Time.deltaTime;
        }
    }



}
