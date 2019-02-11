using System.Collections.Generic;

namespace dgen.Models {

    public class ProjectDiscoveryResult {
        public string ProjPath { get; set; } = string.Empty;
        public string RootNamespace { get; set; } = string.Empty;
        public List<string> Paths { get; set; }
    }
}