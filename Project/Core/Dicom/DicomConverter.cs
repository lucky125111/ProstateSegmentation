using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Entity;
using Core.Model.DicomInput;
using Dicom;
using Dicom.Imaging;

namespace Core.Dicom
{
    public class DicomConverter : IDicomConverter
    {
        public NewDicomInputModel OpenDicomAndConvertToModel(string dicomBase64)
        {
            using (var stream = new MemoryStream(Convert.FromBase64String(dicomBase64)))
            {
                var dicomFile = DicomFile.Open(stream);
                var dicomImage = new DicomImage(dicomFile.Dataset);

                return CreateDicomModel(dicomFile, dicomImage);
            }
        }

        public NewDicomInputModel OpenDicomAndConvertFromFile(string path)
        {
            var dicomFile = DicomFile.Open(path);
            var dicomImage = new DicomImage(dicomFile.Dataset);

            return CreateDicomModel(dicomFile, dicomImage);
        }

        private static NewDicomInputModel CreateDicomModel(DicomFile dicomFile, DicomImage dicomImage)
        {
            var patientData = GetPatientData(dicomFile);
            var dicomImages = GetImages(dicomImage);

            return new NewDicomInputModel(patientData, dicomImages)
            {
                ImageWidth = dicomImage.Width,
                ImageHeight = dicomImage.Height,
                PixelSpacingVertical = GetDicomDecimalTag(dicomFile, DicomTag.PixelSpacing, 0),
                PixelSpacingHorizontal = GetDicomDecimalTag(dicomFile, DicomTag.PixelSpacing, 1),
                SliceThickness = GetDicomDecimalTag(dicomFile, DicomTag.SliceThickness, 0),
                SpacingBetweenSlices = GetDicomDecimalTag(dicomFile, DicomTag.SpacingBetweenSlices, 0)
            };
        }

        private static NewDicomPatientData GetPatientData(DicomFile dcm)
        {
            var id = dcm.Dataset.GetValues<string>(DicomTag.PatientID).Join(@"\");
            var name = dcm.Dataset.GetValues<string>(DicomTag.PatientName).Join(@"\");

            return new NewDicomPatientData(id, name)
            {
                IssuerOfPatientID = GetDicomTag(dcm, DicomTag.IssuerOfPatientID),
                TypeOfPatientID = GetDicomTag(dcm, DicomTag.TypeOfPatientID),
                IssuerOfPatientIDQualifiersSequence = GetDicomTag(dcm, DicomTag.IssuerOfPatientIDQualifiersSequence),
                SourcePatientGroupIdentificationSequence =
                    GetDicomTag(dcm, DicomTag.SourcePatientGroupIdentificationSequence),
                GroupOfPatientsIdentificationSequence =
                    GetDicomTag(dcm, DicomTag.GroupOfPatientsIdentificationSequence),
                SubjectRelativePositionInImage = GetDicomTag(dcm, DicomTag.SubjectRelativePositionInImage),
                PatientBirthDate = GetDicomTag(dcm, DicomTag.PatientBirthDate),
                PatientBirthTime = GetDicomTag(dcm, DicomTag.PatientBirthTime),
                PatientBirthDateInAlternativeCalendar =
                    GetDicomTag(dcm, DicomTag.PatientBirthDateInAlternativeCalendar),
                PatientDeathDateInAlternativeCalendar =
                    GetDicomTag(dcm, DicomTag.PatientDeathDateInAlternativeCalendar),
                PatientAlternativeCalendar = GetDicomTag(dcm, DicomTag.PatientAlternativeCalendar),
                PatientSex = GetDicomTag(dcm, DicomTag.PatientSex),
                PatientBirthName = GetDicomTag(dcm, DicomTag.PatientBirthName),
                PatientAge = GetDicomTag(dcm, DicomTag.PatientAge),
                PatientSize = GetDicomTag(dcm, DicomTag.PatientSize),
                PatientSizeCodeSequence = GetDicomTag(dcm, DicomTag.PatientSizeCodeSequence),
                PatientBodyMassIndex = GetDicomTag(dcm, DicomTag.PatientBodyMassIndex),
                MeasuredAPDimension = GetDicomTag(dcm, DicomTag.MeasuredAPDimension),
                MeasuredLateralDimension = GetDicomTag(dcm, DicomTag.MeasuredLateralDimension),
                PatientWeight = GetDicomTag(dcm, DicomTag.PatientWeight),
                PatientAddress = GetDicomTag(dcm, DicomTag.PatientAddress),
                PatientMotherBirthName = GetDicomTag(dcm, DicomTag.PatientMotherBirthName)
            };
        }

        public static string GetDicomTag(DicomFile dcm, DicomTag tag)
        {
            try
            {
                var strings = dcm.Dataset.GetValues<string>(tag);
                return strings.Join(@"\");
            }
            catch (DicomDataException)
            {
                return null;
            }
        }

        public static double? GetDicomDecimalTag(DicomFile dcm, DicomTag tag, int frame)
        {
            try
            {
                return dcm.Dataset.GetValue<double>(tag, frame);
            }
            catch (DicomDataException)
            {
                return null;
            }
        }

        private static ICollection<NewDicomSlice> GetImages(DicomImage dcm)
        {
            var list = GetImagesAsByteList(dcm).Select((x, index) => new NewDicomSlice(x, index)).ToList();
            return list;
        }

        public static List<byte[]> GetImagesAsByteList(DicomImage dcm)
        {
            var l = new List<byte[]>();

            for (var i = 0; i < dcm.NumberOfFrames; i++) l.Add(dcm.ToBytes(i).RenderBitmap(dcm.Width, dcm.Height).ToBytesPng());

            return l;
        }
    }
}