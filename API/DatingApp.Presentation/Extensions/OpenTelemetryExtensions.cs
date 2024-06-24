using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ExportProcessorType = OpenTelemetry.ExportProcessorType;

namespace DatingApp.Presentation.Extensions;

public static class OpenTelemetryExtensions
{
    public static WebApplicationBuilder AddOpenTelemetryObservability(this WebApplicationBuilder builder)
    {   
        builder.Services
            .AddOpenTelemetry()
            .WithTracing(b =>
            {
                b
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true;
                    })
                    .AddHttpClientInstrumentation()
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("DatingApp"))
                    .AddSource("DatingApp")
                    .AddEntityFrameworkCoreInstrumentation(options =>
                    {
                        options.SetDbStatementForText = true;
                        options.SetDbStatementForStoredProcedure = true;
                        options.EnrichWithIDbCommand = (activity, command) =>
                        {
                            var stateDisplayName = $"{command.CommandType} main";
                            activity.DisplayName = stateDisplayName;
                            activity.SetTag("db.name", stateDisplayName);
                        };
                    })
                    .AddOtlpExporter();
            });
        
        return builder;
    }
}