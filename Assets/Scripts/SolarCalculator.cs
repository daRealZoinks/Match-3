using System;

public class SolarCalculator
{
    public static (DateTime? sunrise, DateTime? sunset, bool? polarDayOrNight) CalculateSunriseSunset(
        DateTime currentDateTime, double latitude, double longitude, double elevation = 0.0)
    {
        double currentUnixTimestamp = (currentDateTime - new DateTime(1970, 1, 1)).TotalSeconds;

        // Julian date
        double julianDate = currentUnixTimestamp / 86400.0 + 2440587.5;

        // Julian day
        double julianDay = Math.Ceiling(julianDate - (2451545.0 + 0.0009) + 69.184 / 86400.0);

        // Mean solar time
        double meanSolarTime = julianDay + 0.0009 - longitude / 360.0;

        // Solar mean anomaly
        double solarMeanAnomalyDegrees = (357.5291 + 0.98560028 * meanSolarTime) % 360;
        double solarMeanAnomalyRadians = Math.PI / 180 * solarMeanAnomalyDegrees;

        // Equation of the center
        double equationOfCenterDegrees = 1.9148 * Math.Sin(solarMeanAnomalyRadians) +
                                         0.02 * Math.Sin(2 * solarMeanAnomalyRadians) +
                                         0.0003 * Math.Sin(3 * solarMeanAnomalyRadians);

        // Ecliptic longitude
        double eclipticLongitudeDegrees = (solarMeanAnomalyDegrees + equationOfCenterDegrees + 180.0 + 102.9372) % 360;
        double eclipticLongitudeRadians = Math.PI / 180 * eclipticLongitudeDegrees;

        // Solar transit (Julian date)
        double solarTransitJulianDate = 2451545.0 + meanSolarTime +
                                        0.0053 * Math.Sin(solarMeanAnomalyRadians) -
                                        0.0069 * Math.Sin(2 * eclipticLongitudeRadians);

        // Declination of the Sun
        double sunDeclinationSin = Math.Sin(eclipticLongitudeRadians) * Math.Sin(Math.PI / 180 * 23.4397);
        double sunDeclinationCos = Math.Cos(Math.Asin(sunDeclinationSin));

        // Hour angle
        double hourAngleCos = (Math.Sin(Math.PI / 180 * (-0.833 - 2.076 * Math.Sqrt(elevation) / 60.0)) -
                               Math.Sin(Math.PI / 180 * latitude) * sunDeclinationSin) /
                              (Math.Cos(Math.PI / 180 * latitude) * sunDeclinationCos);

        if (hourAngleCos < -1 || hourAngleCos > 1)
        {
            // Polar day or night
            return (null, null, hourAngleCos > 0);
        }

        double hourAngleRadians = Math.Acos(hourAngleCos);
        double hourAngleDegrees = 180 / Math.PI * hourAngleRadians;

        // Sunrise and sunset (Julian date)
        double sunriseJulianDate = solarTransitJulianDate - hourAngleDegrees / 360.0;
        double sunsetJulianDate = solarTransitJulianDate + hourAngleDegrees / 360.0;

        // Convert Julian date to timestamp
        DateTime sunrise = new DateTime(1970, 1, 1).AddSeconds((sunriseJulianDate - 2440587.5) * 86400);
        DateTime sunset = new DateTime(1970, 1, 1).AddSeconds((sunsetJulianDate - 2440587.5) * 86400);

        return (sunrise, sunset, null);
    }
}
