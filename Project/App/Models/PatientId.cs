namespace App.Models
{
    public class PatientId
    {
        public int Id { get; set; }

        public PatientId(int newPatientDicomModelId)
        {
            Id = newPatientDicomModelId;
        }

        public PatientId()
        {
            
        }
    }
}