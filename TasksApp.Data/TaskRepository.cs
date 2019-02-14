using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksApp.Data
{
    public class TaskRepository
    {
        private string _connectionString;

        public TaskRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddTask(String taskTitle)
        {
            using(var context = new UsersTasksDataContext(_connectionString))
            {
                Task task = new Task();
                task.Title = taskTitle;

                context.Tasks.InsertOnSubmit(task);
                context.SubmitChanges();
            }

            
        }

        public IEnumerable<Task> GetUncompletedTasks()
        {
            using (var context = new UsersTasksDataContext(_connectionString))
            {
                DataLoadOptions loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Task>(t => t.User);
                context.LoadOptions = loadOptions;
                return context.Tasks.Where(t => t.Completed == false).ToList();
            }
        }

        public void UpdateTaskWithUser(int taskId, int userId)
        {
            using (var context = new UsersTasksDataContext(_connectionString))
            {
                context.ExecuteCommand("Update tasks set userId={0} where Id={1}", userId, taskId);
             
            }
        }

        public void CompleteTask(int taskId)
        {
            using(var context=new UsersTasksDataContext(_connectionString))
            {
                context.ExecuteCommand("Update tasks set completed=1 where Id={0}", taskId);
            }
        }
    }
}
