using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dix.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Dix.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IndexController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IServiceProvider _provider;
        private readonly ILogger<IndexController> _logger;
        private readonly IScope _scope;
        private readonly IAllScope _allScope;

        private readonly Func<int, IScope> _xfactory;
        private readonly IService[] _services;



        public IndexController(IServiceProvider provider, ILogger<IndexController> logger, IScope scope, IAllScope allScope, Func<int, IScope> xfactory, IServiceFactory serviceFactory)
        {
            _provider = provider;
            _logger = logger;
            _scope = scope;
            _allScope = allScope;
            _xfactory = xfactory;
            _services = serviceFactory.GetService(new int[] { 3, 4 });
            var s = xfactory(3);
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var s = _provider.GetService<IScope>();

            _logger.LogInformation(_scope.Say());
            _logger.LogInformation(s.Say());
            _logger.LogInformation(_allScope.Say());

            var xs = _xfactory(4);

            _logger.LogInformation(xs.Say());

            foreach (var item in _services)
            {
                _logger.LogInformation(item.Say());
            }

            //_logger.LogWarning("it's warning.");
            //_logger.LogError("it's trace.");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
