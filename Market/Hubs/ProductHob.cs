using Market.ViewModel;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market.Models;
using Market.Models.Model;
using System.IO;

namespace Market.Hubs
{
    [HubName("ProductHob")]
    public class ProductHob : Hub
    {
        [HubMethodName("AddProduct")]
        public void AddProduct()
        {

           
            Clients.All.NewProductAdd();
            
        }

    }
}