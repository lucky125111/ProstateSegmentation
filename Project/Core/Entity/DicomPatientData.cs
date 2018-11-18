using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Core.Model.NewDicom
{
    public class DicomPatientData
    {
        [StringLength(150)]
        public string PatientId { get; set; }
        
        //other properties
        [StringLength(150)]
        public string PatientName { get; set; }

        //
        public double ProstateVolume { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DicomModelId { get; set; }
        public DicomModel DicomModel { get; set; }

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
    }
}