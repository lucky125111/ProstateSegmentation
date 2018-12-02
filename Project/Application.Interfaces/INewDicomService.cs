using System;
using System.Collections.Generic;
using Application.Models;

namespace Application.Interfaces
{
    public interface INewDicomService : IDisposable
    {
        int UploadNewDicom(NewDicomFileModel value);
        void AddToDicom(int id, NewDicomFileModel value);
        int UploadNewDicoms(IEnumerable<NewDicomFileModel> value);
    }
}