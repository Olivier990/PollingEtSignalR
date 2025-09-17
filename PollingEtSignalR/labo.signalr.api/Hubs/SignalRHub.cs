using labo.signalr.api.Data;
using labo.signalr.api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace labo.signalr.api.Hubs
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignalRHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private int userCount;

        public SignalRHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
            var userCountEnStringValue = Context.GetHttpContext().Request.Query["userCount"];
            int userCount = int.Parse(userCountEnStringValue);
            await base.OnConnectedAsync();
            // TODO: Ajouter votre logique
            userCount++;
            UpdateUserCount(userCount);
            await TaskList();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            base.OnDisconnectedAsync(exception);
            // TODO: Ajouter votre logique
            userCount--;
            //UpdateUserCount();
        }

        public async Task UpdateUserCount(int userCount)
        {
            Clients.Caller.SendAsync("UserCount", userCount);
        }

        public async Task TaskList()
        {            
            var tasks = await _context.UselessTasks.ToListAsync();
            await Clients.Caller.SendAsync("TaskList", tasks);
        }

        public async Task Add(string taskName)
        {
            UselessTask uselessTask = new UselessTask()
            {
                Completed = false,
                Text = taskName
            };
            _context.UselessTasks.Add(uselessTask);
            await _context.SaveChangesAsync();
            await TaskList();
        }

        public async Task Complete(int id)
        {
            UselessTask? task = await _context.FindAsync<UselessTask>(id);
            if (task != null)
            {
                task.Completed = true;
                await _context.SaveChangesAsync();
                await TaskList();
            }
        }
    }
}
