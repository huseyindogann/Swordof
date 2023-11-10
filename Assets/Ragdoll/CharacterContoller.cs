using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class CharacterContoller : MonoBehaviour
{
    public static CharacterContoller Chs;
    public Animator anim;
    public GameObject animCharacter;
    public Transform animCharacterRot;
    public DynamicJoystick dynamicJoysrick;
    private float rotspeed;
    public float speed;
   

   
    private void Awake()
    {
        Chs = this;
        anim = animCharacter.GetComponent<Animator>();
    }
    private void Update()
    {
       
       
            if (Input.touchCount > 0)
            {
            
                JoystickMovement();
               
            }
            else
            {
                


            }
        }
   
    private void JoystickMovement()
    {
        rotspeed = speed /2;
        float horizontal = dynamicJoysrick.Horizontal;
        float vertical = dynamicJoysrick.Vertical;
        Vector3 addedPos = new Vector3(horizontal * speed * Time.deltaTime, 0, vertical * speed * Time.deltaTime);

        transform.position += addedPos;

        Vector3 direction = Vector3.forward * vertical + Vector3.right * horizontal;
        animCharacterRot.rotation = Quaternion.Slerp(animCharacterRot.rotation, Quaternion.LookRotation(direction), rotspeed * Time.deltaTime);
    }




}