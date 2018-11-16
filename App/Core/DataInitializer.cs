using System.Linq;
using AutoMapper;
using Core.Context;
using Core.Entity;

namespace Core
{
    public class DataInitializer
    {
        public static void Initialize(DicomContext context)
        {
            context.Database.EnsureCreated();

            if (context.DicomModels.Any())
            {
                return;   // DB has been seeded
            }

            var dcmConverter = new DicomConverter();


            var d1 = dcmConverter.OpenDicomAndConvertFromFile(
                @"D:\Inzynierka\src\Data\CT.rtp1.12.20080627A.CHESTPHANTOM.T.-.3.CT.dcm");
            var d2 = dcmConverter.OpenDicomAndConvertFromFile(
                @"D:\Inzynierka\src\Data\CT.rtp1.12.20080627A.CHESTPHANTOM.T.2.3.CT.dcm");



            var patients = new DicomModel[]
            {

            };
        }
    }
}