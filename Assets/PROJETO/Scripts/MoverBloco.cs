using UnityEngine;

public class MoverBloco : MonoBehaviour
{
    public float pushpower;
    public float targetmass;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
        {
            return;
        }
        if (hit.moveDirection.y < -0.3f)
        {
            return;
        }
        targetmass = body.mass;

        Vector3 pushDir = new Vector3(hit.moveDirection.x,0,hit.moveDirection.z);
        body.angularVelocity = pushDir * pushpower / targetmass;
    }
}
