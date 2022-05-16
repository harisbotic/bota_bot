using EthereumWatcher;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Watcher>();
    })
    .Build();

await host.RunAsync();
