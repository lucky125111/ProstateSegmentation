using Application.Dicom.DicomModels;

namespace Application.Dicom
{
    public interface IDicomConverter
    {
        NewDicomModel OpenDicomAndConvertFromFile(string path);
        NewDicomModel OpenDicomAndConvertFromByte(byte[] dicomBytes);
        NewDicomModel OpenDicomAndConvertFromBase64(string base64Dicom);
    }
}