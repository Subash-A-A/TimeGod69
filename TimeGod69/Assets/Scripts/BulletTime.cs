using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class BulletTime : MonoBehaviour
{
    [Header("Bullet Time Settings")]
    public TimeManager timeManager;
    public KeyCode slowMoKey = KeyCode.Mouse1;
    public float t = 0.5f;

    [SerializeField] Slider timeSlider;
    [SerializeField] Volume volume;
    private ChromaticAberration chromaticAberration;
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
        if (volume.profile.TryGet<ChromaticAberration>(out chromaticAberration))
        {
            chromaticAberration.intensity.value = 0f;
        }
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
            StopBulletTimeEffect();
        }
    }

    void BulletTimeEffect()
    {
        chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, 0.7f, 10f * Time.unscaledDeltaTime);
        inactiveTime = 0;
        currentTime -= Time.deltaTime * 50f;
        timeSlider.value = currentTime;
        timeManager.DoSlowMotion();
    }

    void StopBulletTimeEffect()
    {
        chromaticAberration.intensity.value = 0f;
        timeManager.StopSlowMotion();
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
