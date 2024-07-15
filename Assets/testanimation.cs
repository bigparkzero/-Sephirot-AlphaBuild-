using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testanimation : MonoBehaviour
{
    Animator an;
    public Vector2 movedir;
    public float MoveHorizontal, MoveVertical;
    public float inputSmoother;
    float XcurrentVelocity;
    float YcurrentVelocity;
    
    private void Start()
    {
        an = GetComponent<Animator>();
    }
    private void Update()
    {

        movedir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized
                              * (Input.GetKey(KeyCode.LeftShift) ? 2 : 1);

        MoveHorizontal = Mathf.SmoothDamp(MoveHorizontal,movedir.x,ref XcurrentVelocity, inputSmoother);
        MoveVertical = Mathf.SmoothDamp(MoveVertical, movedir.y,ref YcurrentVelocity, inputSmoother);

        an.SetFloat("MoveX",MoveHorizontal);
        an.SetFloat("MoveY", MoveVertical);
    }
}
