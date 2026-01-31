using UnityEngine;

public class CameraFollowRuntime : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 0, -10);
    public float smooth = 10f;

    private Transform target;

    private void Start()
    {
        FindPlayer();
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            FindPlayer();
            return;
        }

        Vector3 desired = target.position + offset;
        transform.position = Vector3.Lerp(
            transform.position,
            desired,
            smooth * Time.deltaTime
        );
    }

    private void FindPlayer()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            target = p.transform;
        }
    }
}
