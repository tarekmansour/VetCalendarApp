namespace SharedKernel.Tests;

public static class DataFixtureGenerator
{
    public static DateTime GenerateStartTimeSlot(int startTimeInHours, int startTimeInMinutes)
    {
        return new DateTime(
            year: DateTime.Now.Year,
            month: DateTime.Now.Month,
            day: DateTime.Now.AddDays(2).Day,
            hour: DateTime.Now.AddHours(startTimeInHours).Hour,
            minute: DateTime.Now.AddMinutes(startTimeInMinutes).Minute,
            second: 0);
    }
}
