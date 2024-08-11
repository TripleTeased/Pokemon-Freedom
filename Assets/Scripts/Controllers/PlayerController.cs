using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerControlller", menuName = "InputController/PlayerController")]
public class PlayerController : InputController
{
    public override bool RetrieveJumpInput()
    {
        return Input.GetButtonDown("Jump");
    }

    public override float RetrieveMoveInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public override float RetrieveVerticalInput()
    {
        return Input.GetAxisRaw("Vertical");
    }

    public override bool RetrieveJumpHoldInput()
    {
        return Input.GetButton("Jump");
    }
}
