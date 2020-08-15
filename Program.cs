using System;
using System.Threading;
using TitaniumAS.Opc.Client;
using TitaniumAS.Opc.Client.Common;
using TitaniumAS.Opc.Client.Da;
using TitaniumAS.Opc.Client.Da.Browsing;

namespace OPC_test
{
    class Program
    {
        static void Main()
        {
            Bootstrap.Initialize();

            Uri url = UrlBuilder.Build("Matrikon.OPC.Simulation.1");
            using (var server = new OpcDaServer(url))
            {
                // Connect to the server first.
                server.Connect();

                // Create a group with items.
                OpcDaGroup group = server.AddGroup("MyGroup");
                group.IsActive = true;

                var definition1 = new OpcDaItemDefinition
                {
                    ItemId = "Random.Boolean",
                    IsActive = true
                };
                var definition2 = new OpcDaItemDefinition
                {
                    ItemId = "Random.Int1",
                    IsActive = true
                };
                var definition3 = new OpcDaItemDefinition
                {
                    ItemId = "Random.Int2",
                    IsActive = true
                };
                OpcDaItemDefinition[] definitions = { definition1, definition2, definition3 };
                OpcDaItemResult[] results = group.AddItems(definitions);

                // Handle adding results.
                foreach (OpcDaItemResult result in results)
                {
                    if (result.Error.Failed)
                        Console.WriteLine("Error adding items: {0}", result.Error);
                }

                while (true)
                {
                    OpcDaItemValue[] values = group.Read(group.Items, OpcDaDataSource.Device);
                    Console.WriteLine("Random.Boolean: {0} \n Random.Int1: {1} \n Random.Int2: {2}", values[0].Value, values[1].Value, values[2].Value);
                    Thread.Sleep(1000);
                    Console.Clear();
                }


                /*int count = server.Groups.Count;
                Console.WriteLine(count);*/
            }
        }

    }
}
