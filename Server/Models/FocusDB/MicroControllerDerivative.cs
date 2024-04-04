using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HardwareManagement.Server.Models.FocusDB
{
    [Table("MicroControllerDerivatives", Schema = "public")]
    public partial class MicroControllerDerivative
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
        public int MicroControllerDerivativesID { get; set; }

        [ConcurrencyCheck]
        public string MicroControllerDerivativesName { get; set; }

        [ConcurrencyCheck]
        public int? SiliconVendorID { get; set; }

        public SiliconVendor SiliconVendor { get; set; }

        public ICollection<MicroController> MicroControllers { get; set; }

    }
}