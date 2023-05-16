using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControler : MonoBehaviour
{

    /// <summary>
    /// A player controler from a previous game
    /// </summary>
    [SerializeField]
    private float speed;

    private Vector2 velocity;
    private Rigidbody Rigid;
    private bool isOnTalking = false;
    private bool paused = false;

    private CapsuleCollider playerColl;

    [Header("Jump atributes")]
    [SerializeField]
    private float groundCheckRadius = 0.3f;

    [SerializeField]
    private LayerMask groundMask = 0;

    private bool grounded;

    [SerializeField]
    private float JumpForce = 10f;

    [Header("Crouching atributes")]
    [SerializeField]
    private float standingHeight = 2;

    [SerializeField]
    private float crouchingHeight = 1;

    [SerializeField]
    private float standingCenter = 1;

    [SerializeField]
    private float crouchingCenter = 1.39f;

    [SerializeField]
    private float crouchingSpeed = 2.5f;

    [SerializeField]
    private float standingSpeed = 5f;

    [SerializeField]
    private float heighCheckRadius = 0.3f;

    private bool somethingUp;

    private void Awake()
    {
        playerColl = GetComponent<CapsuleCollider>();
        Rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        grounded = Physics.CheckSphere(transform.position, groundCheckRadius, groundMask);
        somethingUp = Physics.CheckSphere(transform.position + new Vector3(0, standingHeight, 0), heighCheckRadius, groundMask);
        velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        velocity = Vector2.ClampMagnitude(velocity, 1) * speed;
        Rigid.velocity = transform.forward * velocity.y + transform.right * velocity.x + transform.up * Rigid.velocity.y;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + new Vector3(0, standingHeight, 0), heighCheckRadius);
    }
}