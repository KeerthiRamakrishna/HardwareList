using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HardwareManagement.Server.Models.FocusDB
{
    [Table("SiliconVendor", Schema = "public")]
    public partial class SiliconVendor
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
        public int SiliconVendorID { get; set; }

        [ConcurrencyCheck]
        public string SiliconVendorName { get; set; }

        public ICollection<MicroControllerDerivative> MicroControllerDerivatives { get; set; }

        public ICollection<MicroController> MicroControllers { get; set; }

        public ICollection<TresosAcg> TresosAcgs { get; set; }

    }
}