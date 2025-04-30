namespace WebSite.Models
{
    public class Weekday
    {
        public Weekday(int weekdayNumber, string weekdayTitle)
        {
            WeekdayNumber = weekdayNumber;
            WeekdayTitle = weekdayTitle;
        }

        public int WeekdayNumber { get; set; }
        public string WeekdayTitle { get; set; }   
    }
}
