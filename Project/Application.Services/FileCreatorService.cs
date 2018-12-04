using System;
using Application.Interfaces;
using Application.Models;

namespace Application.Services
{
    public class FileCreatorService : IFileCreatorService
    {
        public DicomFile CreateDicom(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}