using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BulletTime : MonoBehaviour
{
    [Header("Bullet Time Settings")]
    public TimeManager timeManager;
    public KeyCode slowMoKey = KeyCode.Mouse1;
    [SerializeField] Slider timeSlider;

    public int maxTime = 15;
    public float reloadTime = 2f;
    public float reloadAfterInactivite = 3f;

    [Header("For Debugging")]
    [SerializeField] private bool isReloading = false;
    [SerializeField] private float currentTime;
    [SerializeField] private float inactiveTime = 0f;
    private void Start()
    {
        currentTime = maxTime;
        timeSlider.maxValue = maxTime;
        timeSlider.value = maxTime;
    }
    void Update()
    {
        if (isReloading)
        {
            return;
        }
        if (currentTime < 0)
        {
            StartCoroutine(ReloadTimer());
            return;
        }
        if (inactiveTime >= reloadAfterInactivite && currentTime < maxTime)
        {
            RefreshNonEmptyTimer();
        }

        if (Input.GetKey(slowMoKey))
        {
            BulletTimeEffect();
        }
        if (!Input.GetKey(slowMoKey) && currentTime < maxTime)
        {
            inactiveTime += Time.deltaTime;
            Mathf.Clamp(inactiveTime, 0, reloadAfterInactivite);
        }
        if (Input.GetKeyUp(slowMoKey) || currentTime <= 0)
        {
            timeManager.StopSlowMotion();
        }
    }
    void BulletTimeEffect()
    {
        inactiveTime = 0;
        currentTime -= Time.deltaTime * 50f;
        timeSlider.value = currentTime;
        timeManager.DoSlowMotion();
    }
    IEnumerator ReloadTimer()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentTime = maxTime;
        timeSlider.value = currentTime;
        isReloading = false;
    }
    void RefreshNonEmptyTimer()
    {
        isReloading = true;
        currentTime = maxTime;
        timeSlider.value = currentTime;
        inactiveTime = 0f;
        isReloading = false;
    }
}
