using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TasksApp.Data;
using System.Data.Linq;

namespace TasksApp
{
    public class IndexHub: Hub
    {
        public void AddTask(string taskTitle)
        {
            TaskRepository taskRepo = new TaskRepository(Properties.Settings.Default.conStr);
            taskRepo.AddTask(taskTitle);
            SendTasks();
        }

        public void SendTasks()
        {
            TaskRepository taskRepo = new TaskRepository(Properties.Settings.Default.conStr);
            var tasks=taskRepo.GetUncompletedTasks();
            Clients.All.getTasks(tasks.Select(t => new 
            {
                Id=t.Id,
                Title=t.Title,
                Completed=t.Completed,
                UserId=t.UserId,
                UserDoingIt= t.User != null ? $"{t.User.FirstName} {t.User.LastName}": null
            }));

        }
        
        public void UpdateTaskWithUser(int taskId, int userId)
        {
            TaskRepository taskRepo = new TaskRepository(Properties.Settings.Default.conStr);
            taskRepo.UpdateTaskWithUser(taskId, userId);
            SendTasks();
        }

        public void CompleteTask(int taskId)
        {
            TaskRepository taskRepo = new TaskRepository(Properties.Settings.Default.conStr);
            taskRepo.CompleteTask(taskId);
            SendTasks();
        }
        public void GetAll()
        {
            SendTasks();
        }


    }
}