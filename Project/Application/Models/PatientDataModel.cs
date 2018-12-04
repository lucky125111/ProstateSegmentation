namespace Application.Models
{
    public class PatientDataModel
    {
        public int DicomModelId { get; set; }
        public string PatientId { get; set; }

        public string PatientName { get; set; }

        public string IssuerOfPatientID { get; set; }

        public string TypeOfPatientID { get; set; }

        public string IssuerOfPatientIDQualifiersSequence { get; set; }

        public string SourcePatientGroupIdentificationSequence { get; set; }

        public string GroupOfPatientsIdentificationSequence { get; set; }

        public string SubjectRelativePositionInImage { get; set; }

        public string PatientBirthDate { get; set; }

        public string PatientBirthTime { get; set; }

        public string PatientBirthDateInAlternativeCalendar { get; set; }

        public string PatientDeathDateInAlternativeCalendar { get; set; }

        public string PatientAlternativeCalendar { get; set; }

        public string PatientSex { get; set; }

        public string PatientBirthName { get; set; }

        public string PatientAge { get; set; }

        public string PatientSize { get; set; }

        public string PatientSizeCodeSequence { get; set; }

        public string PatientBodyMassIndex { get; set; }

        public string MeasuredAPDimension { get; set; }

        public string MeasuredLateralDimension { get; set; }

        public string PatientWeight { get; set; }

        public string PatientAddress { get; set; }

        public string PatientMotherBirthName { get; set; }
    }
}