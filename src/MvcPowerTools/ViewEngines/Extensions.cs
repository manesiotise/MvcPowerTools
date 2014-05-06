﻿using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using CavemanTools;
using MvcPowerTools.Html;

namespace MvcPowerTools.ViewEngines
{
    public static class Extensions
    {
        public static FlexibleViewEngineSettings RegisterConventions(this FlexibleViewEngineSettings cfg,params Assembly[] assemblies)
        {
            foreach (var asm in assemblies)
            {
                RegisterConventions(cfg, asm.GetTypesDerivedFrom<IFindViewConvention>(true).ToArray());
            }
            return cfg;
        }

        public static FlexibleViewEngineSettings RegisterConventions(this FlexibleViewEngineSettings cfg,
            params Type[] types)
        {
            var all = types.OrderBy(t =>
            {
                var order = t.GetCustomAttribute<OrderAttribute>();
                if (order == null) return int.MaxValue;
                return order.Value;
            });
            foreach (var type in all)
            {
                cfg.Conventions.Add(type.CreateInstance() as IFindViewConvention);
            }
            return cfg;
        }

        /// <summary>
        /// Returns the partial model used when displaying a partial model template.
        /// Model is set by Html.DisplayTemplate() and ModelInfo.RenderTemplate()
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cc"></param>
        /// <returns></returns>
        public static T TemplateModel<T>(this ControllerContext cc)
        {
            return cc.HttpContext.Get<T>(HtmlHelperExtensions.PartialModelCacheKey);
        }
    }
}