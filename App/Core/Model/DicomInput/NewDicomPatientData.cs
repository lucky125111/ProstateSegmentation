namespace Core.Model.DicomInput
{
    public class NewDicomPatientData
    {
        public string Id { get; set; }

        //other properties
        public NewDicomPatientData(string id)
        {
            Id = id;
        }
    }
}