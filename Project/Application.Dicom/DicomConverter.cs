using System;
using System.IO;
using Application.Dicom.DicomModels;
using Dicom;
using Dicom.Imaging;

namespace Application.Dicom
{
    public class DicomConverter : IDicomConverter
    {
        public NewDicomModel OpenDicomAndConvertFromFile(string path)
        {
            var content = File.ReadAllBytes(path);
            return OpenDicomAndConvertFromByte(content);
        }

        public NewDicomModel OpenDicomAndConvertFromBase64(string base64Dicom)
        {
            var bytes = Convert.FromBase64String(base64Dicom);
            return OpenDicomAndConvertFromByte(bytes);
        }

        public NewDicomModel OpenDicomAndConvertFromByte(byte[] dicomBytes)
        {
            using (var stream = new MemoryStream(dicomBytes))
            {
                try
                {
                    var dicomFile = DicomFile.Open(stream);

                    var dicomImage = new DicomImage(dicomFile.Dataset);

                    return CreateDicomModel(dicomFile, dicomImage);
                }
                catch (ApplicationException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Invalid dicom header", e);
                }
            }
        }

        private bool HasValidHeader(MemoryStream stream)
        {
            stream.Seek(128, SeekOrigin.Begin);
            return stream.ReadByte() == 'D' && stream.ReadByte() == 'I' && stream.ReadByte() == 'C' &&
                   stream.ReadByte() == 'M';
        }

        private NewDicomModel CreateDicomModel(DicomFile dicomFile, DicomImage dicomImage)
        {
            var newDicomModel = GetDicomTagsForDicomModel(dicomFile, dicomImage);
            newDicomModel.DicomPatientData = GetDicomTagsForPatientData(dicomFile);
            newDicomModel.DicomSlices = GetDicomImage(dicomFile, dicomImage);
            return newDicomModel;
        }

        private NewDicomSlice GetDicomImage(DicomFile dicomFile, DicomImage dicomImage)
        {
            return new NewDicomSlice
            {
                Image = dicomImage.ToBytes().RenderBitmap(dicomImage.Width, dicomImage.Height).ToBytesPng(),
                InstanceNumber = dicomFile.Dataset.GetValue<int>(DicomTag.InstanceNumber, 0),
                SliceLocation = dicomFile.Dataset.GetValue<double>(DicomTag.SliceLocation, 0)
            };
        }

        private NewDicomPatientData GetDicomTagsForPatientData(DicomFile dicomFile)
        {
            return new NewDicomPatientData
            {
                PatientId = dicomFile.GetDicomTag(DicomTag.PatientID),
                PatientName = dicomFile.GetDicomTag(DicomTag.PatientName),
                IssuerOfPatientID = dicomFile.GetDicomTag(DicomTag.IssuerOfPatientID),
                TypeOfPatientID = dicomFile.GetDicomTag(DicomTag.TypeOfPatientID),
                IssuerOfPatientIDQualifiersSequence =
                    dicomFile.GetDicomTag(DicomTag.IssuerOfPatientIDQualifiersSequence),
                SourcePatientGroupIdentificationSequence =
                    dicomFile.GetDicomTag(DicomTag.SourcePatientGroupIdentificationSequence),
                GroupOfPatientsIdentificationSequence =
                    dicomFile.GetDicomTag(DicomTag.GroupOfPatientsIdentificationSequence),
                SubjectRelativePositionInImage = dicomFile.GetDicomTag(DicomTag.SubjectRelativePositionInImage),
                PatientBirthDate = dicomFile.GetDicomTag(DicomTag.PatientBirthDate),
                PatientBirthTime = dicomFile.GetDicomTag(DicomTag.PatientBirthTime),
                PatientBirthDateInAlternativeCalendar =
                    dicomFile.GetDicomTag(DicomTag.PatientBirthDateInAlternativeCalendar),
                PatientDeathDateInAlternativeCalendar =
                    dicomFile.GetDicomTag(DicomTag.PatientDeathDateInAlternativeCalendar),
                PatientAlternativeCalendar = dicomFile.GetDicomTag(DicomTag.PatientAlternativeCalendar),
                PatientSex = dicomFile.GetDicomTag(DicomTag.PatientSex),
                PatientBirthName = dicomFile.GetDicomTag(DicomTag.PatientBirthName),
                PatientAge = dicomFile.GetDicomTag(DicomTag.PatientAge),
                PatientSize = dicomFile.GetDicomTag(DicomTag.PatientSize),
                PatientSizeCodeSequence = dicomFile.GetDicomTag(DicomTag.PatientSizeCodeSequence),
                PatientBodyMassIndex = dicomFile.GetDicomTag(DicomTag.PatientBodyMassIndex),
                MeasuredAPDimension = dicomFile.GetDicomTag(DicomTag.MeasuredAPDimension),
                MeasuredLateralDimension = dicomFile.GetDicomTag(DicomTag.MeasuredLateralDimension),
                PatientWeight = dicomFile.GetDicomTag(DicomTag.PatientWeight),
                PatientAddress = dicomFile.GetDicomTag(DicomTag.PatientAddress),
                PatientMotherBirthName = dicomFile.GetDicomTag(DicomTag.PatientMotherBirthName)
            };
        }

        private NewDicomModel GetDicomTagsForDicomModel(DicomFile dicomFile, DicomImage dicomImage)
        {
            return new NewDicomModel
            {
                ImageWidth = dicomImage.Width,
                ImageHeight = dicomImage.Height,
                PixelSpacingVertical = dicomFile.GetDicomDecimalTag(DicomTag.PixelSpacing, 0),
                PixelSpacingHorizontal = dicomFile.GetDicomDecimalTag(DicomTag.PixelSpacing, 1),
                SliceThickness = dicomFile.GetDicomDecimalTag(DicomTag.SliceThickness, 0),
                SpacingBetweenSlices = dicomFile.GetDicomDecimalTag(DicomTag.SpacingBetweenSlices, 0)
            };
        }
    }
}