using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterScript : MonoBehaviour
{
    private float m_speed = 1;
    Animator anim;
    public GameObject Frisbee;
    float yaw = 0;
    public GameObject CameraPivot;
    public Transform returnTransform;
    private Vector3 frisbeeLocalOffset;
    private Vector3 frisbeeRot;
    bool returningFrisbee = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        frisbeeLocalOffset = Frisbee.transform.localPosition;
        frisbeeRot = Frisbee.transform.localEulerAngles;
        
    }

    // Update is called once per frame
    void Update()
    {
       
        float verticalMovement = Input.GetAxis("Vertical");
        float horizontalMovement = Input.GetAxis("Horizontal");
        anim.SetBool("Moving", verticalMovement != 0);
 
        anim.SetFloat("Speed", verticalMovement);

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (Frisbee.transform.parent != null) {
                anim.SetBool("Attacking", true);
            } else {
                returningFrisbee = true;
            }
        }
        if (returningFrisbee) {
            ReturnFrisbee();
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Frisbee Throw")) {
            anim.SetBool("Attacking", false);
           
        }
             yaw += Input.GetAxis("Mouse X");
           
             CameraPivot.transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
             faceCamera();
        }
    void faceCamera() {//mouse controlled
        transform.eulerAngles = new Vector3(0,Mathf.Lerp(transform.eulerAngles.x, yaw, 1), 0);
    }
    void Release() {//called by frisbeetoss animation
        Frisbee.GetComponent<TrailRenderer>().emitting = true;
        Frisbee.transform.parent = null;
        Frisbee.GetComponent<Rigidbody>().isKinematic = false;
        Frisbee.GetComponent<Rigidbody>().AddForce((this.transform.forward+(this.transform.up/3)) * 1500);
      
    }
    void ReturnFrisbee() {
        Frisbee.GetComponent<Rigidbody>().useGravity = false;
        Debug.Log("returnFrisbee");
        float step = 40 * Time.deltaTime;//40 = speed = unexposed/not ideal
        Frisbee.transform.position = Vector3.MoveTowards(Frisbee.transform.position, returnTransform.position, step);
        Vector3 newDir = Vector3.RotateTowards(Frisbee.transform.forward, returnTransform.up, step, 0.0f);
        Frisbee.transform.rotation = Quaternion.LookRotation(newDir);
        if (Frisbee.transform.position == returnTransform.position) {//TODO: No check for failure to return could time it out with teleported return
            Debug.Log("Frisbee arrived");
            returningFrisbee = false;
            Frisbee.GetComponent<Rigidbody>().isKinematic = true;
            Frisbee.GetComponent<Rigidbody>().useGravity = true;
            Frisbee.transform.parent = returnTransform;
            Frisbee.transform.localPosition = frisbeeLocalOffset;
            Frisbee.transform.localEulerAngles = frisbeeRot;
            Frisbee.GetComponent<TrailRenderer>().emitting = false;
        }
    }
}
