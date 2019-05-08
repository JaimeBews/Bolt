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
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        float verticalMovement = Input.GetAxis("Vertical");
        float horizontalMovement = Input.GetAxis("Horizontal");
        anim.SetBool("Moving", verticalMovement != 0);
 
        anim.SetFloat("Speed", verticalMovement);

        if (Input.GetKeyDown(KeyCode.Space)) {
            anim.SetBool("Attacking", true);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Frisbee Throw")) {
            anim.SetBool("Attacking", false);
        }
            //this.transform.position += transform.forward * verticalMovement * m_speed;
            //  this.transform.position += transform.right * horizontalMovement * m_speed;


             yaw += Input.GetAxis("Mouse X");
            // pitch -= speedV * Input.GetAxis("Mouse Y"); //need to put this stuff on camera

             CameraPivot.transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
             faceCamera();
        }
    void faceCamera() {
        transform.eulerAngles = new Vector3(0,Mathf.Lerp(transform.eulerAngles.x, yaw, 1), 0);
    }
    void Release() {
        Frisbee.transform.parent = null;
        Frisbee.GetComponent<Rigidbody>().isKinematic = false;
        Frisbee.GetComponent<Rigidbody>().AddForce((this.transform.forward+(this.transform.up/3)) * 1000);
      
    }
}
