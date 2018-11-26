using System.Collections.Generic;
using Core.Model.DicomInput;

namespace Core.Entity
{
    public class NewDicomInputModel
    {
        public NewDicomInputModel(NewDicomPatientData dicomPatientData, ICollection<NewDicomSlice> dicomSlices)
        {
            DicomPatientData = dicomPatientData;
            DicomSlices = dicomSlices;
        }

        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public double? PixelSize { get; set; }
        public double? PixelSpacingVertical { get; set; }
        public double? PixelSpacingHorizontal { get; set; }
        public double? SliceThickness { get; set; }

        public double? SpacingBetweenSlices { get; set; }
        //todo image details

        public NewDicomPatientData DicomPatientData { get; set; }
        public ICollection<NewDicomSlice> DicomSlices { get; }
    }
}