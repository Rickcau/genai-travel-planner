
using api_roadtrip_input.Interfaces;
using api_roadtrip_input.Plugins;
using api_roadtrip_input.Services;
using api_roadtrip_input.Util;
using Azure.Search.Documents.Indexes;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

//
string _apiDeploymentName = Helper.GetEnvironmentVariable("ApiDeploymentName");
string _apiEndpoint = Helper.GetEnvironmentVariable("ApiEndpoint");
string _apiKey = Helper.GetEnvironmentVariable("ApiKey");
string _apiAISearchEndpoint = Helper.GetEnvironmentVariable("AISearchURL");
string _apiAISearchKey = Helper.GetEnvironmentVariable("AISearchKey");
string _textEmbeddingName = Helper.GetEnvironmentVariable("EmbeddingName");

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddTransient<Kernel>(s =>
        {
            var builder = Kernel.CreateBuilder();
            builder.AddAzureOpenAIChatCompletion(
                _apiDeploymentName,
                _apiEndpoint,
                _apiKey
                );
            //builder.Services.AddSingleton<SearchIndexClient>(s =>
            //{
            //    string endpoint = _apiAISearchEndpoint;
            //    string apiKey = _apiAISearchKey;
            //    return new SearchIndexClient(new Uri(endpoint), new Azure.AzureKeyCredential(apiKey));
            //});

            // Add Singleton for AzureAISearch 
            //builder.Services.AddSingleton<IAzureAISearchService, AzureAISearchService>();

//#pragma warning disable SKEXP0011 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
//            builder.AddAzureOpenAITextEmbeddingGeneration(_textEmbeddingName, _apiEndpoint, _apiKey);
//#pragma warning restore SKEXP0011 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

//            // builder.Plugins.AddFromType<DBQueryPlugin>();  
//            builder.Plugins.AddFromType<UniswapV3SubgraphPlugin>();
//            builder.Plugins.AddFromType<AzureAISearchPlugin>();

            return builder.Build();
        });

        services.AddSingleton<IChatCompletionService>(sp =>
                     sp.GetRequiredService<Kernel>().GetRequiredService<IChatCompletionService>());
        const string systemmsg = @$"You are an AI agent that can help with trip planning.";
        services.AddSingleton<ChatHistory>(s =>
        {
            var chathistory = new ChatHistory();
            chathistory.AddSystemMessage(systemmsg);
            return chathistory;
        });

    })
    .Build();

host.Run();

