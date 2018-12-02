using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Data.Entity
{
    public class DicomModelEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DicomModelId { get; set; }

        //
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public int NumberOfImages { get; set; }
        public double? PixelSize { get; set; }
        public double? PixelSpacingVertical { get; set; }
        public double? PixelSpacingHorizontal { get; set; }
        public double? SliceThickness { get; set; }
        public double? SpacingBetweenSlices { get; set; }
        public double ProstateVolume { get; set; }

        //
        public DicomPatientDataEntity DicomPatientDataEntity { get; set; }
        public ICollection<DicomSliceEntity> DicomImages { get; set; }
    }
}