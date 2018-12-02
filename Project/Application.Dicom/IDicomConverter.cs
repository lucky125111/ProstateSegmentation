using Application.Dicom.DicomModels;

namespace Application.Dicom
{
    public interface IDicomConverter
    {
        NewDicomModel OpenDicomAndConvertFromFile(string combine);
        NewDicomModel OpenDicomAndConvertFromByte(byte[] combine);
    }
}