using Dicom;

namespace Application.Dicom
{
    public static class DicomExtensions
    {
        public static string GetDicomTag(this DicomFile dcm, DicomTag tag)
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

        public static double? GetDicomDecimalTag(this DicomFile dcm, DicomTag tag, int frame)
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
    }
}