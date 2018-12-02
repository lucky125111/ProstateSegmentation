namespace Application.Interfaces
{
    public interface ISegmentationService
    {
        byte[] Calculate(int dicomId, int sliceId);
        byte[] Calculate(byte[] image);
    }
}