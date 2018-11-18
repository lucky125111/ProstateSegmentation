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
        public int DistanceBetweenSlices { get; set; }
        public double PixelSize { get; set; }

        //todo image details

        public NewDicomPatientData DicomPatientData { get; set; }
        public ICollection<NewDicomSlice> DicomSlices { get; }

        public NewDicomInputModel(int imageWidth, int imageHeight, NewDicomPatientData dicomPatientData, ICollection<NewDicomSlice> dicomSlices)
        {
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;
            DicomPatientData = dicomPatientData;
            DicomSlices = dicomSlices;
        }
    }
}