using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Model.DicomInput;
using Core.Model.NewDicom;

namespace Core.Entity
{
    public class NewDicomInputModel
    {
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public double? PixelSize { get; set; }
        public double? PixelSpacingVertical {get; set;}
        public double? PixelSpacingHorizontal {get; set;}
        public double? SliceThickness {get; set;}
        public double? SpacingBetweenSlices {get; set;}
        //todo image details

        public NewDicomPatientData DicomPatientData { get; set; }
        public ICollection<NewDicomSlice> DicomSlices { get; }

        public NewDicomInputModel(NewDicomPatientData dicomPatientData, ICollection<NewDicomSlice> dicomSlices)
        {
            DicomPatientData = dicomPatientData;
            DicomSlices = dicomSlices;
        }
    }
}