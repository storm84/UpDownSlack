using UpDown.Worker;
using UpDown.Worker.Clients;
using UpDown.Worker.Configurations;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.Configure<SlackConfig>(ctx.Configuration.GetSection(SlackConfig.ConfigurationSection));
        services.Configure<UpDownConfig>(ctx.Configuration.GetSection(UpDownConfig.ConfigurationSection));
        services.AddHttpClient<ISlackClient, SlackClient>();
        services.AddHttpClient<IPingService, PingService>();
            //.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { AllowAutoRedirect = false });
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();