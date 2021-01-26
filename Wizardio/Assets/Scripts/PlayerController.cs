using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public Transform CamTransform;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var spell = Utils.SpellsMapping[(int)KeyCode.Mouse0];
            if (Utils.IsCooldownFinished(spell))
            {
                ClientSend.PlayerShoot(transform.position, transform.forward, spell);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            var spell = Utils.SpellsMapping[(int)KeyCode.Mouse1];
            if (Utils.IsCooldownFinished(spell))
            {
                ClientSend.PlayerShoot(transform.position, transform.forward, spell);
            }
        }
    }
    private void FixedUpdate()
    {
        SendInputToServer();
    }

    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
            Input.GetKey(KeyCode.Space)
        };

        ClientSend.PlayerMovement(_inputs);
    }

}
