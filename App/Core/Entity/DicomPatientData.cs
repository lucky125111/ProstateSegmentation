using System.ComponentModel.DataAnnotations.Schema;
using Core.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Core.Model.NewDicom
{
    public class DicomPatientData
    {
        //other properties
        public string DicomPatientId { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DicomModelId { get; set; }

        public DicomModel DicomModel { get; set; }

        public DicomPatientData(string dicomModelId, int dicomPatientId)
        {
            DicomModelId = dicomPatientId;
            DicomPatientId = dicomModelId;
        }
        public DicomPatientData(string dicomModelId)
        {
            DicomPatientId = dicomModelId;
        }
        public DicomPatientData()
        {
            
        }
    }
}