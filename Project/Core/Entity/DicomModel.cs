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
        
        //
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public int NumberOfImages { get; set; }
        public double DistanceBetweenSlices { get; set; }
        public double PixelSize { get; set; }

        //
        public DicomPatientData DicomPatientData { get; set; }
        public ICollection<DicomSlice> DicomImages { get; set; }
        
        public DicomModel()
        {
            
        }
    }
}