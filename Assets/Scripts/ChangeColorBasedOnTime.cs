using System;
using UnityEngine;

public class ChangeColorBasedOnTime : MonoBehaviour
{
    public DateTime now;

    public Color sunriseColor = new(255, 196, 0);
    public Color dayColor = new(41, 134, 206);
    public Color sunsetColor = new(255, 120, 0);
    public Color nightColor = new(12, 12, 75);

    private SpriteRenderer spriteRenderer;

    private DateTime sunriseTime;
    private DateTime sunsetTime;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on this GameObject.");
            return;
        }

        sunriseTime = DateTime.Now.Date.AddHours(6); // Example sunrise time (6 AM)
        sunsetTime = DateTime.Now.Date.AddHours(18); // Example sunset time (6 PM)
    }

    private void Update()
    {
        now = DateTime.Now;

        // Change color based on the time of day
        if (now < sunriseTime)
        {
            spriteRenderer.color = nightColor;
        }
        else if (now >= sunriseTime && now < sunriseTime.AddHours(1))
        {
            spriteRenderer.color = Color.Lerp(nightColor, sunriseColor, (float)(now - sunriseTime).TotalMinutes / 60f);
        }
        else if (now >= sunriseTime.AddHours(1) && now < sunsetTime.AddHours(-1))
        {
            spriteRenderer.color = dayColor;
        }
        else if (now >= sunsetTime.AddHours(-1) && now < sunsetTime)
        {
            spriteRenderer.color = Color.Lerp(sunsetColor, nightColor, (float)(now - sunsetTime.AddHours(-1)).TotalMinutes / 60f);
        }
        else
        {
            spriteRenderer.color = nightColor;
        }
    }
}
