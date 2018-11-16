using System.ComponentModel.DataAnnotations.Schema;
using Core.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Core.Model.NewDicom
{
    public class DicomPatientData
    {
        //other properties
        public string DicomModelId { get; set; }

        public DicomModel DicomModel { get; set; }

        public DicomPatientData(string dicomModelId)
        {
            DicomModelId = dicomModelId;
        }

        public DicomPatientData()
        {
            
        }
    }
}