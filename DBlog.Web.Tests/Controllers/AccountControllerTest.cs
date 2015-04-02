using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using System.Threading.Tasks;
using DBlog.Web.Controllers;
using DBlog.Web.Models;

namespace DBlog.Web.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        [TestMethod]
        public void LogIn()
        {
            AccountController controller = new AccountController();

            var account = new LoginViewModel()
            {
                Email = "dragon.ice@foxmail.com",
                Password = "Kulong@995",
                RememberMe = true,
            }; 
            var result = controller.Login(account,"") as Task<ActionResult>; 

        }
    }
}
