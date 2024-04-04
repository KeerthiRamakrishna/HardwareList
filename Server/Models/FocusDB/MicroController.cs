using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HardwareManagement.Server.Models.FocusDB
{
    [Table("MicroControllers", Schema = "public")]
    public partial class MicroController
    {

        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("@odata.etag")]
        public string ETag
        {
                get;
                set;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HardwareId { get; set; }

        [ConcurrencyCheck]
        public string HardwareName { get; set; }

        [ConcurrencyCheck]
        public int? SiliconVendorID { get; set; }

        public SiliconVendor SiliconVendor { get; set; }

        [ConcurrencyCheck]
        public int? MicroControllerDerivativesID { get; set; }

        public MicroControllerDerivative MicroControllerDerivative { get; set; }

        [ConcurrencyCheck]
        public int? MicroControllerSubDerivativesID { get; set; }

        public MicroControllerSubDerivative MicroControllerSubDerivative { get; set; }

        [ConcurrencyCheck]
        public int? AvailabilityStatusID { get; set; }

        public AvailabilityStatus AvailabilityStatus { get; set; }

    }
}