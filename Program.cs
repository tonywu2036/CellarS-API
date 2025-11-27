using Amazon;
using Amazon.BedrockRuntime;
using CellarS.Api.Services;
using Amazon.Runtime;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:5147");

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();

//Register for Stub AI service
builder.Services.AddScoped<IRephraseService, StubRephraseService>();

// Register Amazon Bedrock Runtime client
builder.Services.AddSingleton<IAmazonBedrockRuntime>(_ =>
{
    var accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
    var secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");

    if (string.IsNullOrWhiteSpace(accessKey) || string.IsNullOrWhiteSpace(secretKey))
    {
        throw new InvalidOperationException("AWS credentials not found in environment variables.");
    }

    var credentials = new BasicAWSCredentials(accessKey, secretKey);

    return new AmazonBedrockRuntimeClient(credentials, RegionEndpoint.USEast1);
});

builder.Services.AddScoped<BedrockAiService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
