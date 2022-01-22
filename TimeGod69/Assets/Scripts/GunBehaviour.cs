using System.Collections;
using UnityEngine;

public class GunBehaviour : MonoBehaviour
{
    [Header("Animations")]
    private Animator anim;
    [SerializeField] KeyCode aimKey = KeyCode.Mouse1;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKey(aimKey))
        {
            anim.SetBool("isAiming", true);
        }
        else
        {
            anim.SetBool("isAiming", false);
        }
    }

}
