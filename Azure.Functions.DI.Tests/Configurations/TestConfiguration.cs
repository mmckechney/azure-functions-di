using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.Functions.DI.Tests.Configurations
{
    [TestClass]
    public sealed class TestConfiguration
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // Set Environment variables for testing (obtained from 'Environment.runsettings' file)
            var properties = context.Properties;

            foreach (var property in properties)
            {
                Environment.SetEnvironmentVariable(property.Key, property.Value.ToString(), EnvironmentVariableTarget.Process);
            }
        }
    }
}
