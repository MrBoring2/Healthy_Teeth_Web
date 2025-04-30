namespace WebSite.Models
{
    public class Appointment
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int VisitId { get; set; }
        public int VisitStatusId { get; set; }
        public string PatientFullName { get; set; }
        public string VisitPurpose { get; set; }

        public string Text => $"Пациент: \n{PatientFullName}, Цель: \n{VisitPurpose}, Статус: \n{Status}";
        public string Status { get; set; }
    }
}
