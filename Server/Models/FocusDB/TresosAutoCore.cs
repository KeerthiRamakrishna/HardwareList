using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HardwareManagement.Server.Models.FocusDB
{
    [Table("TresosAutoCore", Schema = "public")]
    public partial class TresosAutoCore
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
        public int TresosAutoCoreID { get; set; }

        [ConcurrencyCheck]
        public int? MicroControllerSubDerivativesID { get; set; }

        public MicroControllerSubDerivative MicroControllerSubDerivative { get; set; }

        [ConcurrencyCheck]
        public int? ArchitectureID { get; set; }

        public Architecture Architecture { get; set; }

        [ConcurrencyCheck]
        public int? AvailabilityStatusID { get; set; }

        public AvailabilityStatus AvailabilityStatus { get; set; }

        [ConcurrencyCheck]
        public int? CompilerVendorID { get; set; }

        public CompilerVendor CompilerVendor { get; set; }

        [ConcurrencyCheck]
        public int? CompilerVersionID { get; set; }

        public CompilerVersion CompilerVersion { get; set; }

        [ConcurrencyCheck]
        public string MultiCore { get; set; }

        [ConcurrencyCheck]
        public string DevDrop { get; set; }

        [ConcurrencyCheck]
        public string RFM { get; set; }

    }
}