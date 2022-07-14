using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todolist.Entities;

namespace Todolist.Service
{
    public interface ITodoService
    {
        Task<bool> Create(Todo todo);
        Task<bool> Delete(int id);
    }
}
