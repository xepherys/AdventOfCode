
namespace AdventOfCode2018
{
    /// <summary>
    /// Collection class to contain each individual guard's data for the List<Day4aSchedule>
    /// </summary>
    public class Day4aGuard
    {
        public string id { get; set; }
        public int numSleeps { get; set; }
        public int minutesSleeping { get; set; }
        private int[] sleepMinutes = new int[60];
        public int this[int i]
        {
            get
            {
                return sleepMinutes[i];
            }

            set
            {
                sleepMinutes[i] = value;
            }
        }
    }

    #region Enumerations
    /// <summary>
    /// Enumeration of state's used to control parsing of Day4aSchedule into Day4aGuard
    /// </summary>
    public enum GuardState
    {
        NONE,
        Starts,
        FallsAsleep,
        WakesUp,
    }
    #endregion
}
