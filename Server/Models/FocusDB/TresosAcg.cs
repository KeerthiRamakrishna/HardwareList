using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HardwareManagement.Server.Models.FocusDB
{
    [Table("TresosACG", Schema = "public")]
    public partial class TresosAcg
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
        public int TresosACGID { get; set; }

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
        public int? AUTOSARVersionID { get; set; }

        public AutosarVersion Autosarversion { get; set; }

        [ConcurrencyCheck]
        public int? SiliconVendorID { get; set; }

        public SiliconVendor SiliconVendor { get; set; }

        [ConcurrencyCheck]
        public string MCALVersion { get; set; }

        [ConcurrencyCheck]
        public int? CompilerVendorID { get; set; }

        public CompilerVendor CompilerVendor { get; set; }

        [ConcurrencyCheck]
        public int? CompilerVersionID { get; set; }

        public CompilerVersion CompilerVersion { get; set; }

        [ConcurrencyCheck]
        public string EBtresosBoardSupportPackageMCALavailability { get; set; }

        [ConcurrencyCheck]
        public string EBtresosBoardSupportPackageAutoCoreOS { get; set; }

        [ConcurrencyCheck]
        public string EBtresosBoardSupportPackageSafetyOS { get; set; }

        [ConcurrencyCheck]
        public string EBtresosBoardSupportPackageFOTA { get; set; }

        [ConcurrencyCheck]
        public string SUPPackageforApplicationEssentials { get; set; }

        [ConcurrencyCheck]
        public string SUPPackageforBootloaderEssentials { get; set; }

    }
}