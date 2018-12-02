using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Application.Data.Context;
using Application.Data.Entity;
using Application.Dicom;

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

            foreach (var file in Directory.GetFiles(dataPath).Where(x => Path.GetExtension(x) == "dcm"))
            {
                var d1 = dcmConverter.OpenDicomAndConvertFromFile(file);

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

                var images = new List<DicomSliceEntity>();

                foreach (var i in images) context.DicomSlices.Add(new DicomSliceEntity()
                {
                    Image = d1.DicomSlices.Image,
                    InstanceNumber = d1.DicomSlices.InstanceNumber,
                    SliceLocation = d1.DicomSlices.SliceLocation,
                    DicomModelId = e1.DicomModelId,
                });
                context.SaveChanges();
            }
        }
    }
}