using Mirror;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;
    Vector2 velocity;

    public NetworkIdentity identity;
    private void Start()
    {
        if(!identity.isLocalPlayer)
        {
            enabled = false;
        }
    }

    void Update()
    {
        velocity.y = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        velocity.x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(velocity.x, 0, velocity.y);
    }
}
