using System;
using System.Threading.Tasks;
using MiniSwitch.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace MiniSwitch.Helpers
{
    public class AppSettingsActionFilter : IAsyncActionFilter
    {
        private AppSettings _appSettings;
        public AppSettingsActionFilter(IConfiguration configuration)
        {
            _appSettings = new AppSettings();
            configuration.Bind(_appSettings);
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ((Microsoft.AspNetCore.Mvc.Controller)context.Controller).ViewBag.AppSettings = _appSettings;
            await next();
        }
    }
}
