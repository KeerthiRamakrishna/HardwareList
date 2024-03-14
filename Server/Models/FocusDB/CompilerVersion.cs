using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HardwareManagement.Server.Models.FocusDB
{
    [Table("CompilerVersion", Schema = "public")]
    public partial class CompilerVersion
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
        [Required]
        public int CompilerVersionID { get; set; }

        [ConcurrencyCheck]
        public string CompilerVersionName { get; set; }

        public ICollection<TresosAcg> TresosAcgs { get; set; }

        public ICollection<TresosAutoCore> TresosAutoCores { get; set; }

        public ICollection<TresosSafetyO> TresosSafetyOs { get; set; }

    }
}