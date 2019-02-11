
using System.Collections.Generic;
using dgen.Models;

namespace dgen.Services {

    public interface IProjectDiscoveryService
    {
        ProjectDiscoveryResult DiscoverProject(string path);
    }
}