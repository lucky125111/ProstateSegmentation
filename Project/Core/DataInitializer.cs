using System.Collections.Generic;
using System.Linq;
using Core.Context;
using Core.Dicom;
using Core.Entity;
using Core.Model.NewDicom;

namespace Core
{
    public class DataInitializer
    {
        public static void Initialize(DicomContext context)
        {
            context.Database.EnsureCreated();

            if (context.DicomModels.Any()) return; // DB has been seeded

            var dcmConverter = new DicomConverter();

            var d1 = dcmConverter.OpenDicomAndConvertFromFile(
                @"D:\Inzynierka\src\Data\CT.rtp1.12.20080627A.CHESTPHANTOM.T.-.3.CT.dcm");
            var d2 = dcmConverter.OpenDicomAndConvertFromFile(
                @"D:\Inzynierka\src\Data\DOSE.20080627A.TRAINING4FLD.dcm");

            var e1 = new DicomModel
            {
                ImageHeight = d1.ImageHeight,
                ImageWidth = d1.ImageWidth,
                NumberOfImages = d1.DicomSlices.Count
            };
            var e2 = new DicomModel
            {
                ImageHeight = d2.ImageHeight,
                ImageWidth = d2.ImageWidth,
                NumberOfImages = d2.DicomSlices.Count
            };

            var patients = new[]
            {
                e1,
                e2
            };

            foreach (var s in patients) context.DicomModels.Add(s);

            context.SaveChanges();

            var patientsData = new[]
            {
                new DicomPatientData(d1.DicomPatientData.PatientId, e1.DicomModelId)
                {
                    PatientName = d1.DicomPatientData.PatientName
                },
                new DicomPatientData(d2.DicomPatientData.PatientId, e2.DicomModelId)
                {
                    PatientName = d2.DicomPatientData.PatientName
                }
            };

            foreach (var s in patientsData) context.DicomPatientDatas.Add(s);

            var images = new List<DicomSlice>();

            foreach (var newDicomSlice in d1.DicomSlices)
                images.Add(new DicomSlice(newDicomSlice.Image, newDicomSlice.SliceIndex, e1.DicomModelId));
            foreach (var newDicomSlice in d2.DicomSlices)
                images.Add(new DicomSlice(newDicomSlice.Image, newDicomSlice.SliceIndex, e2.DicomModelId));

            foreach (var i in images) context.DicomSlices.Add(i);
            context.SaveChanges();
        }
    }
}