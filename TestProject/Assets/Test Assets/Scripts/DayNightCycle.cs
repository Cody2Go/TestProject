using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time")]
    [Tooltip("Day length in minutes")]
    [SerializeField]
    private float targetDayLength = 13;
    public float GetTargetDayLength()
    {
        return targetDayLength;
    }
    [SerializeField]
    [Range(0f, 1f)]
    private float timeOfDay;
    public float GetTimeOfDay()
    {
        return timeOfDay;
    }
    [SerializeField]
    private int dayNumber = 1;
    public int GetDayNumber()
    {
        return dayNumber;
    }
    [SerializeField]
    private int yearNumber = 1;
    public int GetYearNumber()
    {
        return yearNumber;
    }
    private float timeScale = 100f;
    [SerializeField]
    private int yearLength = 100;
    public int GetYearLength()
    {
        return yearLength;
    }
    public bool pause = false;

    [Header("Sun Light")]
    [SerializeField]
    private Transform sunDailyRotation;
    [SerializeField]
    private Light sun;
    private float sunIntensity;
    [SerializeField]
    private float sunBaseIntensity = 1f;
    [SerializeField]
    private float sunVariation = 1.5f;
    [SerializeField]
    private Gradient sunColor;

    [Header("Moon Light")]
    [SerializeField]
    private Transform moonDailyRotation;
    [SerializeField]
    private Light moon;
    private float moonIntensity;
    [SerializeField]
    private float moonBaseIntensity = 0.25f;
    [SerializeField]
    private float moonVariation = 1.5f;
    [SerializeField]
    private Gradient moonColor;

    private void Update()
    {
        if (!pause)
        {
            UpdateTimescale();
            UpdateTime();
        }

        AdjustSunRotation();
        AdjustSunIntensity();
        AdjustSunColor();

        AdjustMoonRotation();
        AdjustMoonIntensity();
        AdjustMoonColor();

        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseTime();
        }
    }

    private void UpdateTimescale()
    {
        timeScale = 24 / (targetDayLength / 60);
    }

    private void UpdateTime()
    {
        timeOfDay += Time.deltaTime * timeScale / 86400; //number of seconds in a day
        if (timeOfDay > 1)
        {
            dayNumber++;
            timeOfDay -= 1;
        }

        if (dayNumber > yearLength)
        {
            yearNumber++;
            dayNumber = 1;
        }
    }

    private void AdjustSunRotation()
    {
        float sunAngle = timeOfDay * 360;
        sunDailyRotation.transform.localRotation = Quaternion.Euler(new Vector3(0f, 135f, sunAngle));
    }

    private void AdjustSunIntensity()
    {
        sunIntensity = Vector3.Dot(sun.transform.forward, Vector3.down);
        sunIntensity = Mathf.Clamp01(sunIntensity);

        sun.intensity = sunIntensity * sunVariation + sunBaseIntensity;
    }

    private void AdjustSunColor()
    {
        sun.color = sunColor.Evaluate(sunIntensity);
    }

    private void AdjustMoonRotation()
    {
        float moonAngle = timeOfDay * 360;
        moonDailyRotation.transform.localRotation = Quaternion.Euler(new Vector3(0f, -135f, moonAngle));
    }

    private void AdjustMoonIntensity()
    {
        moonIntensity = Vector3.Dot(moon.transform.forward, Vector3.down);
        moonIntensity = Mathf.Clamp01(moonIntensity);

        moon.intensity = moonIntensity * moonVariation + moonBaseIntensity;
    }

    private void AdjustMoonColor()
    {
        moon.color = moonColor.Evaluate(moonIntensity);
    }

    private void PauseTime()
    {
        Debug.Log("Pausing time");

        if (!pause)
        {
            pause = true;
        }

        else
        {
            pause = false;
        }
    }
}
