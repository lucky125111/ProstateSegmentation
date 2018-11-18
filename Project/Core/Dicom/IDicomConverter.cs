using Core.Entity;

namespace Core.Dicom
{
    public interface IDicomConverter
    {
        NewDicomInputModel OpenDicomAndConvertToModel(string dicomBase64);
        NewDicomInputModel OpenDicomAndConvertFromFile(string path);
    }
}