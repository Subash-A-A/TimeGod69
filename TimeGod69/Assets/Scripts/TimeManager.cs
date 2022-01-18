using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Header("Time Manager Settings")]
    public float slowDownFactor = 0.05f;
    public void DoSlowMotion()
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
    public void StopSlowMotion()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}
