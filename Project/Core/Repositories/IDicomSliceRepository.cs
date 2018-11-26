using System;
using System.Collections.Generic;
using Core.Entity;

namespace Core.Repositories
{
    public interface IDicomSliceRepository : IDisposable
    {
        DicomSlice GetDicomSlice(int patientId, int sliceId);
        IEnumerable<byte[]> GetSlices(int patientId);
        IEnumerable<byte[]> GetMasks(int patientId);
        byte[] GetMaskById(int patientId, int sliceId);
        byte[] GetSliceById(int patientId, int sliceId);
        void UpdateMask(DicomSlice dicomSlice);
        void InsertSlice(DicomSlice dicomSlice);
        void InsertSlices(IEnumerable<DicomSlice> dicomSlices);
        void Save();
    }
}