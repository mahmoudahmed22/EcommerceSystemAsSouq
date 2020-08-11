using Market.Models;
using Market.Models.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Hubs
{
    [HubName("CommentHub")]
    public class CommentHub:Hub
    {
        [HubMethodName("AddComment")]
        public void AddComment(Comment comment )
        {
            
            ApplicationDbContext dbContext = new ApplicationDbContext();            
            comment.dateTime = DateTime.Now;
            dbContext.comments.Add(comment);           
            dbContext.SaveChanges();
            Clients.All.NewCommentAdd(comment);

        }
    }
}