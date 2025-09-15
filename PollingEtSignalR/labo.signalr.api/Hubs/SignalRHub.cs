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

        public SignalRHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
            base.OnConnectedAsync();
            // TODO: Ajouter votre logique
            TaskList();
        }

        public async Task TaskList()
        {
            var tasks =  _context.UselessTasks.ToListAsync();
            await Clients.Caller.SendAsync("taskList", tasks);
        }

        //[HttpPost]
        //public async Task<ActionResult<UselessTask>> Add(string taskText)
        //{
        //    UselessTask uselessTask = new UselessTask()
        //    {
        //        Completed = false,
        //        Text = taskText
        //    };
        //    _context.UselessTasks.Add(uselessTask);
        //    await _context.SaveChangesAsync();

        //    return Ok(uselessTask);
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> Complete(int id)
        //{
        //    UselessTask? task = await _context.FindAsync<UselessTask>(id);
        //    if (task != null)
        //    {
        //        task.Completed = true;
        //        await _context.SaveChangesAsync();
        //        return NoContent();
        //    }
        //    return NotFound();
        //}
    }
}
