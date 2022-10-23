using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;
public class Player : MonoBehaviour
{
    [SerializeField] private FloatingJoystick joyStick;
    [SerializeField]private float moveSpeed;
    [SerializeField]private float rotateSpeed;
    private Animator animator;
    private Rigidbody rigidBody;
    private Vector3 moveVector;
    public List<Transform> handCuffStack = new List<Transform>();
    public CinemachineVirtualCamera camNormal;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        move();
    }

    private void move()
    {
        moveVector = Vector3.zero;
        moveVector.x = joyStick.Horizontal * moveSpeed * Time.deltaTime;
        moveVector.z = joyStick.Vertical * moveSpeed * Time.deltaTime;

        if (joyStick.Horizontal!=0||joyStick.Vertical!=0)
        {
            Vector3 direction = Vector3.RotateTowards(transform.forward, moveVector, rotateSpeed * Time.deltaTime,0.0f);
            transform.rotation = Quaternion.LookRotation(direction);
            animator.SetBool("isRunning", true);
        }
        else if (joyStick.Horizontal==0&&joyStick.Vertical==0)
        {
            animator.SetBool("isRunning", false);
        }
        rigidBody.MovePosition(rigidBody.position + moveVector);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="handCuff")
        {
            Destroy(other.GetComponent<BoxCollider>());
            handCuffStack.Add(other.gameObject.transform);
            other.transform.parent = transform;
            handcuff hc=other.GetComponent<handcuff>();
            hc.index =handCuffStack.Count==3? 1: handCuffStack.Count - 1;
            hc.player = this;
            hc.enabled = true;
        }
        else if (other.tag=="agent"&&handCuffStack.Count>2)
        {
            //attack anim
            //kelepceyi at
            changeCameraToAttack();
            Invoke("changeCameraToNormal", 0.5f);
            Destroy(other.GetComponent<CapsuleCollider>());
            handcuff hc=handCuffStack.ElementAt(handCuffStack.Count - 1).GetComponent<handcuff>();
            hc.enabled = true;
            hc.reach = false;
            hc.goingToCriminal=true;
            hc.criminal = other.transform;
            handCuffStack.RemoveAt(handCuffStack.Count - 1);
            if (bots.botList.Count==0)
            {
                bots.botList.Add(transform);
            }
            other.gameObject.GetComponent<bots>().enabled = true;
            bots.botList.Add(other.transform);
            other.GetComponent<bots>().index = bots.botList.Count - 1;
        }
        else if (other.tag=="jail")
        {
            Destroy(other.GetComponent<MeshCollider>());

            bots.botList.ElementAt(bots.botList.Count - 1).GetComponent<bots>().target = other.gameObject.transform.GetChild(0).gameObject.transform;
            bots.botList.ElementAt(bots.botList.Count - 1).gameObject.transform.GetChild(0).gameObject.GetComponent<handcuff>().enabled = true;
            handCuffStack.Add(bots.botList.ElementAt(bots.botList.Count - 1).gameObject.transform.GetChild(0).gameObject.transform);
            bots.botList.ElementAt(bots.botList.Count - 1).gameObject.transform.GetChild(0).transform.parent = transform;
            gameObject.transform.GetChild(transform.childCount-1).transform.localRotation= Quaternion.Euler(-90, 0, -180);
            bots.botList.RemoveAt(bots.botList.Count-1);
        }
    }

    private void changeCameraToAttack()
    {
        camNormal.Priority = 9;
    }

    private void changeCameraToNormal()
    {
        camNormal.Priority=11;
    }
}
