using Scalar.AspNetCore;

namespace Web.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerWithUi(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }

        public static IApplicationBuilder UserScalarWithUi(this WebApplication app)
        {
            app.MapScalarApiReference(options =>
            {
                options.Title = "This is my Scalar API";
                options.DarkMode = true;
                options.Favicon = "path";
                options.DefaultHttpClient = new KeyValuePair<ScalarTarget, ScalarClient>(ScalarTarget.CSharp, ScalarClient.RestSharp);
                options.HideModels = false;
                options.Layout = ScalarLayout.Modern;
                options.ShowSidebar = true;

                options.Authentication = new ScalarAuthenticationOptions
                {
                    PreferredSecurityScheme = "Bearer"
                };
            });

            return app;
        }
    }

}
