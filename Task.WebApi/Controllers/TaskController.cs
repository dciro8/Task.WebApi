using Microsoft.AspNetCore.Mvc;
using Task.Application.Services;
using Task.DataInfrastructure.Context;
using Task.DataInfrastructure.Repository;
using Task.Domain.Entities;
using Task.Models.Models;
using Task.WebApi.Validations;

namespace Task.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        TaskDbContext _taskDbContext = new TaskDbContext();
        public TaskController(TaskDbContext taskDbContext) {
            _taskDbContext = taskDbContext;
        }

        TaskService createService()
        {            
            TaskRepository repository = new TaskRepository(_taskDbContext);
            TaskService service = new TaskService(repository);
            return service;

        }

        [HttpGet]
        [Route(nameof(TaskController.Get))]
        public RequestResultModel<List<TaskDTO>> Get()
        {
            RequestResultModel<List<TaskDTO>> response = new Models.Models.RequestResultModel<List<TaskDTO>>();
            var service = createService();
            response.result = service.Get().Select(x => new TaskDTO
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Developer = x.Developer,
                State = x.Status,
                DateCreate = x.Date_Created
            }).ToList();
            return response;
        }


        [HttpPost]
        [Route(nameof(TaskController.Create))]
        public RequestResultModel<TaskDTO> Create(TaskDTO objDto)
        {
            RequestResultModel<TaskDTO> response = new Models.Models.RequestResultModel<TaskDTO>();
            try
            {
                TaskValidation validat = new();
                var validatorResult = validat.Validate(objDto);

                if (validatorResult.Errors.Any())
                {
                    response.isSuccessful = false;
                    response.errorMessage = validatorResult.Errors.Select(s => s.ErrorMessage).Aggregate((a, b) => $"{a}, {b}");

                    return response;
                }

                TASK entity = new TASK
                {
                    Id = objDto.Id,
                    Developer = objDto.Developer,
                    Title = objDto.Title,
                    Description = objDto.Description,
                    Status = objDto.State,
                    Date_Created = DateTime.Now,
                };

                var service = createService();
                service.Add(entity);

                response.isSuccessful = true;
                response.result = new TaskDTO { Title = entity.Title };

                return response;
            }
            catch (Exception e)
            {
                response.isSuccessful = false;
                response.result = objDto;
                response.errorMessage = e.StackTrace + " " + e.Message;
                return response;
            }
        }



        [HttpPost]
        [Route(nameof(TaskController.Edit))]
        public RequestResultModel<TaskDTO> Edit(TaskDTO objDto)
        {
            RequestResultModel<TaskDTO> response = new Models.Models.RequestResultModel<TaskDTO>();
            try
            {
                TaskValidation validat = new();
                var validatorResult = validat.Validate(objDto);

                if (validatorResult.Errors.Any())
                {
                    response.isSuccessful = false;
                    response.errorMessage = validatorResult.Errors.Select(s => s.ErrorMessage).Aggregate((a, b) => $"{a}, {b}");

                    return response;
                }


                TASK entity = new TASK
                {
                    Id = objDto.Id,
                    Developer = objDto.Developer,
                    Title = objDto.Title,
                    Description = objDto.Description,
                    Status = objDto.State,
                    Date_Updated = DateTime.Now,
                    //DATE_LIMIT = DateTime.Now.AddDays(5),
                };


                var service = createService();
                service.Edit(entity);

                response.isSuccessful = true;
                response.result = new TaskDTO { Title = entity.Title };

                return response;
            }
            catch (Exception e)
            {
                response.isSuccessful = false;
                response.result = objDto;
                response.errorMessage = e.StackTrace + " " + e.Message;
                return response;
            }
        }

        [HttpPost]
        [Route(nameof(TaskController.Delete))]
        public RequestResultModel<TaskDTO> Delete(TaskDTO objDto)
        {
            RequestResultModel<TaskDTO> response = new Models.Models.RequestResultModel<TaskDTO>();
            try
            {
                var service = createService();
                service.Delete(objDto.Id);
                response.isSuccessful = true;
                response.result = objDto;

                return response;
            }
            catch (Exception e)
            {
                response.isSuccessful = false;
                response.result = objDto;
                response.errorMessage = e.StackTrace + " " + e.Message;
                return response;
            }
        }
    }
}
