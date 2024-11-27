using System.Net.Http.Headers;
using HW4.Services;

namespace HW4;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Retrieve the api key from user secrets
        string tmdbApiKey = builder.Configuration["TmdbApiKey"];
        string tmdbApiUrl = "https://api.themoviedb.org/3/";

        // Add services to the container.
        builder.Services.AddHttpClient<ITmdbService, TmdbService>((httpClient, services) =>
        {
            httpClient.BaseAddress = new Uri(tmdbApiUrl);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tmdbApiKey);

            return new TmdbService(httpClient, services.GetRequiredService<ILogger<TmdbService>>());
        });

        builder.Services.AddControllersWithViews();
        builder.Services.AddSwaggerGen(); 

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        else
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
