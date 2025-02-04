/*using Microsoft.AspNetCore.Mvc;
using TaskManagementStystem.Interfaces;
using TaskManagementStystem.Models;

public class TaskController : Controller
{
    private readonly ITaskRepository _taskRepository;

    public TaskController(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IActionResult> Index()
    {
        var tasks = await _taskRepository.GetAllTasksAsync();
        return View(tasks);
    }

    public async Task<IActionResult> CreateTask()
    {
        return View();
    }

    public async Task<IActionResult> EditTask(int id)
    {
        var task = await _taskRepository.GetTaskByIdAsync(id);
        if (task == null) return NotFound();
        return View(task);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaskModel task)
    {
        if (ModelState.IsValid)
        {
            await _taskRepository.AddTaskAsync(task);
            return RedirectToAction("Index");
        }
        return View(task);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TaskModel task)
    {
        if (ModelState.IsValid)
        {
            await _taskRepository.UpdateTaskAsync(task);
            return RedirectToAction("Index");
        }
        return View(task);
    }

    public async Task<IActionResult> Delete(int id)
    {
        await _taskRepository.DeleteTaskAsync(id);
        return RedirectToAction("Index");
    }
}
*/


using Microsoft.AspNetCore.Mvc;

namespace TaskManagementStystem.Controllers
{
    public class TaskController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult CreateTask() => View();
        public IActionResult EditTask() => View();
        public IActionResult LogView() => View();
        public IActionResult Delete() => View();
    }
}
