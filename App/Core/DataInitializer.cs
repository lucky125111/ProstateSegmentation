using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core.Context;
using Core.Entity;
using Core.Model.NewDicom;

namespace Core
{
    public class DataInitializer
    {
        public static void Initialize(DicomContext context)
        {
            context.Database.EnsureCreated();

            if (context.DicomModels.Any())
            {
                return;   // DB has been seeded
            }

            var dcmConverter = new DicomConverter();

            var d1 = dcmConverter.OpenDicomAndConvertFromFile(
                @"D:\Inzynierka\src\Data\CT.rtp1.12.20080627A.CHESTPHANTOM.T.-.3.CT.dcm");
            var d2 = dcmConverter.OpenDicomAndConvertFromFile(
                @"D:\Inzynierka\src\Data\DOSE.20080627A.TRAINING4FLD.dcm");

            var e1 = new DicomModel(d1.ImageWidth, d1.ImageHeight, d1.PatientId);
            var e2 = new DicomModel(d2.ImageWidth, d2.ImageHeight, d2.PatientId);

            var patients = new DicomModel[]
            {
                e1,
                e2, 
            };

            foreach (var s in patients)
            {
                context.DicomModels.Add(s);
            }

            context.SaveChanges();

            var patientsData = new DicomPatientData[]
            {
                new DicomPatientData(d1.PatientId, e1.DicomModelId),
                new DicomPatientData(d2.PatientId, e2.DicomModelId), 
            };

            foreach (var s in patientsData)
            {
                context.DicomPatientDatas.Add(s);
            }

            var images = new List<DicomSlice>();

            foreach (var newDicomSlice in d1.DicomSlices)
            {
                images.Add(new DicomSlice(newDicomSlice.Image, newDicomSlice.SliceIndex, e1.DicomModelId));
            }
            foreach (var newDicomSlice in d2.DicomSlices)
            {
                images.Add(new DicomSlice(newDicomSlice.Image, newDicomSlice.SliceIndex, e2.DicomModelId));
            }
            
            foreach (var i in images)
            {
                context.DicomSlices.Add(i);
            }
            context.SaveChanges();

        }
    }
}