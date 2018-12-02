using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Application.Data.Context;
using Application.Data.Entity;
using Application.Dicom;
using Application.Dicom.DicomModels;

namespace Application.Data
{
    public class DataInitializer
    {
        public static void Initialize(DicomContext context)
        {
            context.Database.EnsureCreated();

            if (context.DicomModels.Any())
                return;

            var dcmConverter = new DicomConverter();

            var dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SeedData");

            foreach (var file in Directory.GetFiles(dataPath))
            {
                var d1 = dcmConverter.OpenDicomAndConvertFromFile(file);

                var dicomPatientDataEntity = context.DicomPatientDatas.FirstOrDefault(x => x.PatientId == d1.DicomPatientData.PatientId);

                if (dicomPatientDataEntity == null)
                {
                    AddNewPatient(context, d1);
                }
                else
                {
                    AddSlices(context, d1, dicomPatientDataEntity.DicomModelId);
                }
            }
        }

        private static void AddNewPatient(DicomContext context, NewDicomModel d1)
        {
            var e1 = new DicomModelEntity
            {
                ImageHeight = d1.ImageHeight,
                ImageWidth = d1.ImageWidth,
                NumberOfImages = 1
            };

            context.DicomModels.Add(e1);
            context.SaveChanges();

            var p = new DicomPatientDataEntity()
            {
                PatientId = d1.DicomPatientData.PatientId,
                DicomModelId = e1.DicomModelId,
                PatientName = d1.DicomPatientData.PatientName
            };

            context.DicomPatientDatas.Add(p);
            AddSlices(context, d1, p.DicomModelId);
            context.SaveChanges();
        }

        private static void AddSlices(DicomContext context, NewDicomModel d1, int dicomModelId)
        {
            context.DicomSlices.Add(new DicomSliceEntity()
            {
                Image = d1.DicomSlices.Image,
                InstanceNumber = d1.DicomSlices.InstanceNumber,
                SliceLocation = d1.DicomSlices.SliceLocation,
                DicomModelId = dicomModelId,
            });
            context.SaveChanges();
        }
    }
}