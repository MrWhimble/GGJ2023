using UnityEngine;

public class GryoSim : MonoBehaviour
{
    public Transform top;
    public Transform mover;
    public float speed;

    private Quaternion originalRot;
    private Vector3 originalForward;
    private float originalAngle;

    private void Start()
    {
        originalRot = transform.rotation;
        originalForward = originalRot * Vector3.forward;
    }
    
    private void Update()
    {
        Quaternion newRot = transform.rotation;
        newRot = Quaternion.Inverse(originalRot) * newRot;
        Vector3 newForward = newRot * Vector3.forward;
        Vector3 newRight = newRot * Vector3.right;
        
        Debug.DrawRay(Vector3.zero, newForward, Color.green);
        //Debug.DrawRay(Vector3.zero, newRight, Color.red);

        //newForward = Quaternion.Inverse(originalRot) * newForward;
        
        

        //newUp = Quaternion.Inverse(originalRot) * newUp;
        
        //Vector3 input = Vector3.ProjectOnPlane(newUp, Vector3.up);
        //Debug.Log(newUp);
        
        mover.Translate(new Vector3(newForward.x * speed * Time.deltaTime, newForward.y * speed * Time.deltaTime, 0));
    }
}