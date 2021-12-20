using System.Collections.Generic;
using CNTO.Launcher.Identity;
using NUnit.Framework;

namespace CNTO.Launcher.Test
{
    public class ModSetTests
    {
        private Repository _mainRepo;
        private Repository _devRepo;

        [SetUp]
        public void Setup()
        {
            _mainRepo = new ClientRepository(new RepositoryId("main"), @"c:\cnto\main", 1);
            _mainRepo.HasMod("@mod1");
            _mainRepo.HasMod("@mod2");
            _mainRepo.HasMod("@mod4");

            _devRepo = new ClientRepository(new RepositoryId("dev"), @"c:\cnto\dev", 2);
            _devRepo.HasMod("@mod1");
            _devRepo.HasMod("@mod3");
        }
    }
}