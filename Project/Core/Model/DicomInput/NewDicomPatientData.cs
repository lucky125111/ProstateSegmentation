namespace Core.Model.DicomInput
{
    public class NewDicomPatientData
    {
        public string PatientId { get; set; }

        //todo
        public string PatientName { get; set; }

        //other properties
        public NewDicomPatientData(string patientId, string patientName)
        {
            PatientId = patientId;
            PatientName = patientName;
        }
    }
}