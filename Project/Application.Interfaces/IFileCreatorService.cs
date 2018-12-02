using System;
using Application.Models;

namespace Application.Interfaces
{
    public interface IFileCreatorService : IDisposable
    {
        DicomFile CreateDicom(int id);
    }
}