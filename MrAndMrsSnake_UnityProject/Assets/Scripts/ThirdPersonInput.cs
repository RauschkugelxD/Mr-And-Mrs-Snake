using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class ThirdPersonInput : MonoBehaviour
{
    public FixedJoystick LeftJoystick;
    public FixedTouchField TouchField;
    protected ThirdPersonUserControl Control;

    protected Camera Cam;
    protected float CameraAngleX = -90;
    protected float CameraAngleY = 1;
    protected float CameraAngleSpeed = 0.2f;

    void Start()
    {
        Control = GetComponent<ThirdPersonUserControl>();
        Cam = Camera.main;
    }

    void LateUpdate()
    {
        // Snake movement
        Control.Hinput = LeftJoystick.input.x; 
        Control.Vinput = LeftJoystick.input.y; 

        // Camera rotation
        CameraAngleX += TouchField.TouchDist.x * CameraAngleSpeed; // horizonal 
        CameraAngleY += TouchField.TouchDist.y * 0.003f; // vertical 

        // Set camera position and rotation according to user input
        Cam.transform.position = transform.position + Quaternion.AngleAxis(CameraAngleX, Vector3.up) * new Vector3(0, 3, 5);
        Cam.transform.rotation = Quaternion.LookRotation(transform.position + new Vector3(0, CameraAngleY, 0)* 2f - Cam.transform.position, Vector3.up); 
    }
}



