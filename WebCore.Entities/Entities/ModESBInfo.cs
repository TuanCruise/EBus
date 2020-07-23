using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class ModESBInfo : ModuleInfo
    {
        [DataMember, Column(Name = "BASEURL")]
        public string BaseURL { get; set; }
        [DataMember, Column(Name = "FUNC")]
        public string Func { get; set; }
        [DataMember, Column(Name = "METHOD")]
        public string Method { get; set; }
        [DataMember, Column(Name = "TYPE")]
        public string Type { get; set; }
    }
}

