namespace App.Models
{
    public class PatientId
    {
        public PatientId(int newPatientDicomModelId)
        {
            Id = newPatientDicomModelId;
        }

        public PatientId()
        {
        }

        public int Id { get; set; }
    }
}