using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HardwareManagement.Server.Models.FocusDB
{
    [Table("AUTOSARVersion", Schema = "public")]
    public partial class AutosarVersion
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
        public int AUTOSARVersionID { get; set; }

        [Column("AUTOSARVersion")]
        [ConcurrencyCheck]
        public string AUTOSARVersion1 { get; set; }

        public ICollection<TresosAcg> TresosAcgs { get; set; }

    }
}