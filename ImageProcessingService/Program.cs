using ImageProcessingCore;
using ImageProcessingService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.RegisterCoreServices();
builder.Services.RegisterOptions(builder.Configuration);
builder.Services.AddHostedService<ImageProcessor>();

var host = builder.Build();
host.Run();