using Application.Models;

namespace Application.Interfaces
{
    public interface IFileCreatorService
    {
        DicomFile CreateDicom(int id);
    }
}