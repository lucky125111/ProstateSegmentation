using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Model.DicomInput;
using Core.Model.NewDicom;

namespace Core.Entity
{
    public class NewDicomInputModel
    {
        public string PatientId { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }

        public NewDicomPatientData DicomPatientData { get; set; }
        public ICollection<NewDicomSlice> DicomSlices { get; }

        public NewDicomInputModel(int imageWidth, int imageHeight, string patientId, NewDicomPatientData dicomPatientData, ICollection<NewDicomSlice> dicomSlices)
        {
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;
            PatientId = patientId;
            DicomPatientData = dicomPatientData;
            DicomSlices = dicomSlices;
        }
    }
}