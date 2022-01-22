using UnityEngine;

public class GunShooting : MonoBehaviour
{
    [Header("Bullet")]
    public GameObject bullet;

    [Header("Forces")]
    public float shootForce, upwardForce;

    [Header("Gun Settings")]
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magSize, bulletsPerTap;
    public bool fullAuto;

    int bulletsLeft, bulletsShot;
    bool shooting, readyToShoot, reloading;

    [Header("Refrence")]
    [SerializeField] Camera fpsCam;
    [SerializeField] Transform attackPoint;
    [SerializeField] KeyCode shootKey = KeyCode.Mouse0;
    [SerializeField] KeyCode reloadKey = KeyCode.R;

    [Header("Debugging")]
    public bool allowInvoke = true;

    private void Awake()
    {
        bulletsLeft = magSize;
        readyToShoot = true;
    }
    private void Update()
    {
        MyInput();
    }
    private void MyInput()
    {
        if (fullAuto)
        {
            shooting = Input.GetKey(shootKey);
        }
        else
        {
            shooting = Input.GetKeyDown(shootKey);
        }

        // Realoding
        if (Input.GetKeyDown(reloadKey) && bulletsLeft < magSize && !reloading)
        {
            Reload();
        }
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
        {
            Reload();
        }

        // Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        // A Ray through middle of Camera
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        RaycastHit targetHit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out targetHit))
        {
            targetPoint = targetHit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75);
        }

        // Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //Calculate Spread
        float spreadX = Random.Range(-spread, spread);
        float spreadY = Random.Range(-spread, spread);

        //Calculate new Direction with Spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(spreadX, spreadY, 0);

        //Instantiate bullet
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;

        // Add force to bullet;
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            // Invoke will invoke the function after certain time;
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magSize;
        reloading = false;
    }

}
