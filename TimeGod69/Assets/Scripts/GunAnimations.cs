using UnityEngine;

public class GunAnimations : MonoBehaviour
{
    private Animator anim;
    [SerializeField] KeyCode aimKey = KeyCode.Mouse1;
    private void Awake()
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
