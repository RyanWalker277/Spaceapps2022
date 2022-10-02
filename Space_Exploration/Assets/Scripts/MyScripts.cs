using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  
public class MyScripts: MonoBehaviour {  
    protected Joystick joystick;  
    protected Joybutton joybutton;  
    protected bool jump;  
    //public float Horizontal { get; private set; }  
    // Start is called before the first frame update  
    void Start() {  
        joystick = FindObjectOfType < Joystick > ();  
        joybutton = FindObjectOfType < Joybutton > ();  
    }  
    // Update is called once per frame  
    void Update() {  
        var rigidbody = GetComponent < Rigidbody > ();  
        rigidbody.velocity = new Vector3(joystick.Horizontal * 300f + (Input.GetAxis("Horizontal") * 300f), rigidbody.velocity.y, joystick.Vertical * 300f + Input.GetAxis("Vertical") * 300f);  
        if (!jump && (joybutton.pressed || Input.GetButton("Fire2"))) {  
            jump = true;  
            rigidbody.velocity += Vector3.up * 300f;  
        }  
        if (jump && !joybutton.pressed || Input.GetButton("Fire2")) {  
            jump = false;  
        }  
    }  
}   