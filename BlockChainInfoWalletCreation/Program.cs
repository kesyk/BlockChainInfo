using Info.Blockchain.API.Client;
using Info.Blockchain.API.Wallet;
using Info.Blockchain.API.Models;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BlockChainApiTest
{
    class Program
    {
        public static IConfiguration Config { get; set; }

        static void Main(string[] args)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json");
                Config = builder.Build();

                var httpClient = new BlockchainHttpClient(Config["BlockChainInfo:ApiCode"], Config["BlockChainInfo:WalletServiceLink"]);
                var walletCreator = new WalletCreator(httpClient);
                CreateWalletResponse walletResponce = walletCreator.CreateAsync(Config["BlockChainInfo:MainPassword"], null, Config["BlockChainInfo:Label"]).Result;

                StreamWriter w = new StreamWriter(Config["WritePath"]);
                w.Write(JsonConvert.SerializeObject(walletResponce));
                w.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
            
        }
    }
}
