using OpenQA.Selenium;
using System.Data;
using System;
using System.Collections.Generic;
using Dapper;
using System.Security.Cryptography;
using AdamOneilSoftware;

namespace Gander.Unit.Abstract
{
	public abstract class Environment
	{
		private readonly string _name;
		private readonly string _url;

		public Environment(string name, string url)
		{
			_name = name;
			_url = url;
		}

		public string Name { get { return _name; } }
		public string Url { get { return _url; } }

		public abstract IDbConnection GetConnection();

		public abstract Credentials DefaultCredentials();

		public string GetUrl(string url)
		{
			return SeleniumHelper.CombineUrl(Url, url);
		}

		public INavigation NavigateTo(IWebDriver driver, string url)
		{
			driver.Url = GetUrl(url);
			return driver.Navigate();
		}

		public bool AssertQuery<T>(string query, object parameters, Func<IEnumerable<T>, bool> condition)
		{
			using (var cn = GetConnection())
			{
				var results = cn.Query<T>(query, parameters);
				return condition.Invoke(results);
			}
		}		

		public bool AssertQuerySingle<T>(string query, object parameters, Func<T, bool> condition)
		{
			using (var cn = GetConnection())
			{
				var result = cn.QuerySingle<T>(query, parameters);
				return condition.Invoke(result);
			}
		}

		public bool AssertExists(string query, object parameters)
		{
			using (var cn = GetConnection())
			{
				var result = cn.QuerySingleOrDefault(query, parameters);
				return (result != null);
			}
		}
	}
}