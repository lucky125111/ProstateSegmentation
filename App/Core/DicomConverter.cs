using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Core.Entity;
using Core.Model;
using Core.Model.DicomInput;
using Core.Model.NewDicom;
using Dicom;
using Dicom.Imaging;

namespace Core
{
    public class DicomConverter : IDicomConverter
    {
        public NewDicomInputModel OpenDicomAndConvertToModel(string dicomBase64)
        {
            using (var stream = new MemoryStream((Convert.FromBase64String(dicomBase64))))
            {
                var dicomFile = DicomFile.Open(stream);
                var dicomImage = new DicomImage(dicomFile.Dataset);

                return CreateDicomModel(dicomFile, dicomImage);
            }
        }

        private static NewDicomInputModel CreateDicomModel(DicomFile dicomFile, DicomImage dicomImage)
        {
            var patientData = GetPatientData(dicomFile);
            var dicomImages = GetImages(dicomImage);
            return new NewDicomInputModel(dicomImage.Width, dicomImage.Height, patientData.Id, patientData, dicomImages);
        }

        public NewDicomInputModel OpenDicomAndConvertFromFile(string path)
        {
            var dicomFile = DicomFile.Open(path);
            var dicomImage = new DicomImage(dicomFile.Dataset);

            return CreateDicomModel(dicomFile, dicomImage);
        }

        private static NewDicomPatientData GetPatientData(DicomFile dcm)
        {
            var id = dcm.Dataset.GetValue<string>(DicomTag.PatientID, 0);
            //todo get other properties
            return new NewDicomPatientData(id);
        }

        private static ICollection<NewDicomSlice> GetImages(DicomImage dcm)
        {
            return GetImagesAsByteList(dcm).Select((x, index) => new NewDicomSlice(x, index)) as ICollection<NewDicomSlice>;
        }

        private static List<byte[]> GetImagesAsByteList(DicomImage dcm)
        {
            var l = new List<byte[]>();

            for (int i = 0; i < dcm.NumberOfFrames; i++)
            {
                l.Add(dcm.ToBytes(i));
            }

            return l;
        }
    }
}
