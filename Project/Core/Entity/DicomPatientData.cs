using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entity;

namespace Core.Model.NewDicom
{
    public class DicomPatientData
    {
        public DicomPatientData(string modelId, int dicomPatientId)
        {
            DicomModelId = dicomPatientId;
            PatientId = modelId;
        }

        public DicomPatientData(string modelId)
        {
            PatientId = modelId;
        }

        public DicomPatientData()
        {
        }

        [StringLength(150)] public string PatientId { get; set; }

        //other properties
        [StringLength(150)] public string PatientName { get; set; }

        [StringLength(255)] public string IssuerOfPatientID { get; set; }

        [StringLength(255)] public string TypeOfPatientID { get; set; }

        [StringLength(255)] public string IssuerOfPatientIDQualifiersSequence { get; set; }

        [StringLength(255)] public string SourcePatientGroupIdentificationSequence { get; set; }

        [StringLength(255)] public string GroupOfPatientsIdentificationSequence { get; set; }

        [StringLength(255)] public string SubjectRelativePositionInImage { get; set; }

        [StringLength(255)] public string PatientBirthDate { get; set; }

        [StringLength(255)] public string PatientBirthTime { get; set; }

        [StringLength(255)] public string PatientBirthDateInAlternativeCalendar { get; set; }

        [StringLength(255)] public string PatientDeathDateInAlternativeCalendar { get; set; }

        [StringLength(255)] public string PatientAlternativeCalendar { get; set; }

        [StringLength(255)] public string PatientSex { get; set; }

        [StringLength(255)] public string PatientBirthName { get; set; }

        [StringLength(255)] public string PatientAge { get; set; }

        [StringLength(255)] public string PatientSize { get; set; }

        [StringLength(255)] public string PatientSizeCodeSequence { get; set; }

        [StringLength(255)] public string PatientBodyMassIndex { get; set; }

        [StringLength(255)] public string MeasuredAPDimension { get; set; }

        [StringLength(255)] public string MeasuredLateralDimension { get; set; }

        [StringLength(255)] public string PatientWeight { get; set; }

        [StringLength(255)] public string PatientAddress { get; set; }

        [StringLength(255)] public string PatientMotherBirthName { get; set; }

        //
        public double ProstateVolume { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DicomModelId { get; set; }

        public DicomModel DicomModel { get; set; }
    }
}