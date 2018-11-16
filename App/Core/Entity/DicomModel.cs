using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Model.NewDicom;

namespace Core.Entity
{
    public class DicomModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DicomModelId { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public string DicomPatientDataId { get; set; }

        public DicomPatientData DicomPatientData { get; set; }
        public ICollection<DicomSlice> DicomImages { get; set; }

        public DicomModel(int imageWidth, int imageHeight, string dicomPatientDataId)
        {
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;
            DicomPatientDataId = dicomPatientDataId;
        }

        public DicomModel()
        {
            
        }
    }
}