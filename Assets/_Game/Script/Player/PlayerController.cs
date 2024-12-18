using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IMovable
{
    #region Variables
    [Header("Movement")]
    [SerializeField] private float acceleration = 75f;
    [SerializeField] private float deceleration = 30f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float velocityStopThreshold = 0.5f;

    //create a separate class
    [Header("Shooting")]
    [SerializeField] private Bullet bullet;
    [SerializeField] private GameObject target;
    [SerializeField] private float attackSpeed = 0.2f; // Time between shots
    private float attackTimer = 0f; // Timer to track elapsed time for firing
    private float attackCooldown;

    private Rigidbody2D rb;
    private bool isFacingRight = true; // Default facing direction
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //TODO: doesnt belong here
    private void Start()
    {
        attackCooldown = 1 / attackSpeed;
    }

    private void Update()
    {
        // Increment the fire timer every frame
        attackTimer += Time.deltaTime;
    }
    

    public void Move(Vector2 moveDirection)
    {
        if (moveDirection != Vector2.zero)
        {
            Flip(moveDirection.x);

            rb.AddForce(moveDirection * acceleration, ForceMode2D.Force);

            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }
        else
        {
            rb.AddForce(rb.velocity.normalized * -deceleration, ForceMode2D.Force);

            if (rb.velocity.magnitude < velocityStopThreshold)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void Flip(float horizontalInput)
    {
        if (horizontalInput > 0 && !isFacingRight)
        {
            // Face right
            isFacingRight = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            // Face left
            isFacingRight = false;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    //TODO: this probably belongs to a gun, and this would be "trigger attack method" or smth
    public void Fire()
    {
        // If enough time has passed since the last fire, fire again
        if (attackTimer >= attackCooldown)
        {
            Vector2 shootDirection = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;

            // Spawn the bullet using object pool
            Bullet bulletObject = ObjectPoolManager.Instance.SpawnObject<Bullet>(bullet.gameObject, transform.position, Quaternion.identity.normalized, PoolType.GameObject);

            if (bulletObject != null)
            {
                // Initialize the bullet with the direction
                bulletObject.OnInit(shootDirection);

                // Calculate the angle to rotate the bullet
                float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;

                // Rotate the bullet to face the target (the bullet's up is aligned to the target direction)
                bulletObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
            }

            attackTimer = 0f; // Reset timer
        }

    }

    //public void Dash()
    //{
    //    // Only perform the dash if it's off cooldown
    //    if (timeSinceDash >= dashCooldown && !isDashing)
    //    {
    //        Vector2 dashDirection = inputHandler.MovementInput != Vector2.zero
    //                                ? inputHandler.MovementInput.normalized
    //                                : lastDirection;

    //        isDashing = true;
    //        rb.AddForce(dashDirection * dashForce, ForceMode2D.Impulse);

    //        // Reset the dash timer
    //        timeSinceDash = 0f;
    //    }
    //some other logic for dash
    //// Update cooldown timer only when not dashing
    //if (!isDashing)
    //{
    //    timeSinceDash += Time.deltaTime; // Only increment if not dashing
    //}

    //// Handle dash state
    //if (isDashing)
    //{
    //    timeSinceDash += Time.deltaTime;
    //    if (timeSinceDash >= dashDuration)
    //    {
    //        isDashing = false;  // Dash ends after dashDuration
    //    }
    //    return; // Skip movement and dash logic while dashing
    //}
    //}

}
