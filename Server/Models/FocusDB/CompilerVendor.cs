using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HardwareManagement.Server.Models.FocusDB
{
    [Table("CompilerVendor", Schema = "public")]
    public partial class CompilerVendor
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
        public int CompilerVendorID { get; set; }

        [ConcurrencyCheck]
        public string CompilerVendorName { get; set; }

        public ICollection<TresosAcg> TresosAcgs { get; set; }

        public ICollection<TresosAutoCore> TresosAutoCores { get; set; }

        public ICollection<TresosSafetyO> TresosSafetyOs { get; set; }

    }
}