using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using UltimateGenSearch.Services;
using UltimateGenSearch.Services.Aggregator;
using UltimateGenSearch.Services.Scrapers;

namespace UltimateGenSearch.App_Start
{
    public static class DependencyConfig
    {
        internal static IUnityContainer Container { get; private set; }

        internal static void Init(HttpConfiguration config)
        {
            Container = new UnityContainer();

            Container.RegisterType<IScraper, DummyScraper>("DummyScraper");

            Container.RegisterType<IAggregator, SimpleAggregator>();

            Container.RegisterType<ISearchService, SearchService>();

            config.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(Container);
        }
    }
}