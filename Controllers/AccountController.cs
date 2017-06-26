using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Extensions.FileProviders;
using WebAPITs.Models;

namespace WebAPITs.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IFileProvider _fileProvider;

        public AccountController(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        /// <summary>
        /// Get a list of all users
        /// </summary>
        /// <returns>List of all users</returns>
        [Route("AllUsers")]
        [HttpGet]
        public IEnumerable<string> GetAllUsers()
        {
            SqlConnection connection = new SqlConnection();
            //connection.op

            var settingsFile = _fileProvider.GetFileInfo("accounts.json");
            if (!settingsFile.Exists)
            {
                yield break;
            }

            var serializer = new JsonSerializer();

            using (var fs = settingsFile.CreateReadStream())
            using (var sr = new StreamReader(fs))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                var accounts = serializer.Deserialize<List<Account>>(jsonTextReader);
                if (accounts == null)
                {
                    yield break;
                }
                foreach (var account in accounts)
                {
                    if (account?.AccountParameters?.Username != null)
                    {

                        yield return account.AccountParameters.Username;
                    }
                }
            }
        }

        [Route("Add")]
        [HttpPost]
        public void AddUser([FromBody] LoginParameters param)
        {
            var settingsFile = _fileProvider.GetFileInfo("accounts.json");
            if (!settingsFile.Exists)
            {
                List<Account> accounts = new List<Account>
                {
                    new Account()
                    {
                        AccountParameters = param,
                        Money = 0
                    }
                };
                System.IO.File.WriteAllText(settingsFile.PhysicalPath, JsonConvert.SerializeObject(accounts));
                return;
            }
            else
            {
                List<Account> accounts = null;
                var serializer = new JsonSerializer();

                using (var fs = settingsFile.CreateReadStream())
                using (var sr = new StreamReader(fs))
                using (var jsonTextReader = new JsonTextReader(sr))
                {
                    accounts = serializer.Deserialize<List<Account>>(jsonTextReader);
                    if (accounts == null)
                    {
                        return;
                    }
                    accounts.Add(new Account()
                    {
                        AccountParameters = param,
                        Money = 0
                    });
                }
                System.IO.File.WriteAllText(settingsFile.PhysicalPath, JsonConvert.SerializeObject(accounts));
            }
        }
    }
}
