using Dogstagram.WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace Dogstagram.WebApi.Infrastructures.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerMidleware(this IApplicationBuilder app)
            => app.UseSwagger()
                  .UseSwaggerUI(c =>
                  {
                      c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dogstagram V1");
                      c.RoutePrefix = string.Empty;
                  });
    }
}
