﻿using Microsoft.Practices.Unity;
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
    using UltimateGenSearch.Services.Connections;
    using UltimateGenSearch.Services.Login;

    public static class DependencyConfig
    {
        internal static IUnityContainer Container { get; private set; }

        internal static void Init(HttpConfiguration config)
        {
            Container = new UnityContainer();

            //Container.RegisterType<IScraper, DummyScraper>("DummyScraper");
            Container.RegisterType<IScraper, AcomScraper>("AcomScraper");
            Container.RegisterType<IScraper, FindMyPastScraper>("FindMyPastScraper");
            Container.RegisterType<IScraper, FamilySearchScraper>("FamilySearchScraper");


            Container.RegisterType<IAggregator, SimpleAggregator>();

            Container.RegisterType<ISearchService, SearchService>();

            Container.RegisterType<IConnectionFactory, ConnectionFactory>();
            Container.RegisterType<ILogin, NullLogin>();


            config.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(Container);
        }
    }
}