using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataFragment : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float accel;
    [SerializeField] float rotSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] List<Sprite> sprites = new List<Sprite>();

    Vector2 desiredVelocity;
    Rigidbody2D rb;
    bool moving = true;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
        rb = GetComponent<Rigidbody2D>();
        float rotZ = Random.Range(0f, 360f);

        desiredVelocity = new Vector3(1, 0, 0);
        desiredVelocity = Quaternion.AngleAxis(rotZ, new Vector3(0, 0, 1)) * desiredVelocity;
        desiredVelocity.Normalize();
    }

    void FixedUpdate()
    {
        if (!moving)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        Vector2 diff = Player.instance.transform.position - transform.position;
        diff.Normalize();
        desiredVelocity = RotateTowards(desiredVelocity, diff, rotSpeed * Mathf.Deg2Rad, 0f);

        rb.velocity = desiredVelocity * moveSpeed;
        moveSpeed += accel;
        moveSpeed = moveSpeed > maxSpeed ? maxSpeed : moveSpeed;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            StartCoroutine(DestroySelf());
        }
    }

    private IEnumerator DestroySelf()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        moving = false;
        Player.instance.GetComponent<PlayerData>().AddData(5);

        yield return new WaitForSeconds(0.2f);
        
        Destroy(this.gameObject);
    }

    public static Vector2 RotateTowards(Vector2 current, Vector2 target, float maxRadiansDelta, float maxMagnitudeDelta)
    {
        if (current.x + current.y == 0)
            return target.normalized * maxMagnitudeDelta;
 
        float signedAngle = Vector2.SignedAngle(current, target);
        float stepAngle = Mathf.MoveTowardsAngle(0, signedAngle, maxRadiansDelta * Mathf.Rad2Deg) * Mathf.Deg2Rad;
        Vector2 rotated = new Vector2(
            current.x * Mathf.Cos(stepAngle) - current.y * Mathf.Sin(stepAngle),
            current.x * Mathf.Sin(stepAngle) + current.y * Mathf.Cos(stepAngle)
        );
        if (maxMagnitudeDelta == 0)
            return rotated;
 
        float magnitude = current.magnitude;
        float targetMagnitude = target.magnitude;
        targetMagnitude = Mathf.MoveTowards(magnitude, targetMagnitude, maxMagnitudeDelta);
        return rotated.normalized * targetMagnitude;
    }
}
