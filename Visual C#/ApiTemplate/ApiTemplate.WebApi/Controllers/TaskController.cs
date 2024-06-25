using ApiTemplate.Lib.Entities;
using ApiTemplate.Lib.Mappers;
using ApiTemplate.Lib.Models;
using ApiTemplate.Lib.Models.Client;
using ApiTemplate.Lib.Services;
using ApiTemplate.Lib.Validation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ApiTemplate.WebApi.Controllers
{
  [Route("api/v1/tasks")]
  [ApiController]
  public class TaskController
    : AppBaseController
  {
    private readonly ITaskMapper _mapper;

    private readonly ITaskService _service;

    public TaskController(
      ITaskService service,
      ITaskMapper mapper)
    {
      _service = service;

      _mapper = mapper;
    }

    // GET api/tasks/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ITask))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
    public ActionResult<ITask> Get(int id)
    {
      //TODO: Need to verify that the user has access to the requested resource
      var entity = _service.GetTask(id); //TODO: UserId needs to be passed

      if (entity == null) throw Lib.Exceptions.NotFound.Task(id);

      return Ok(_mapper.ToModel(entity));
    }
    
    // POST api/tasks
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ITask))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorModel))]
    public async Task<ActionResult<TaskModel>> Post([FromBody] TaskV1CreateModel model)
    {
      var entity = _mapper.ToEntity(UserId, model);

      Validations.IsNotNull(entity, nameof(model));

      var result = (await Task.FromResult(_service.Add(entity)));

      var m = _mapper.ToModel(result);

      return CreatedAtAction(nameof(Get), new { id = m!.TaskId }, m);
    }
    
    // PATCH api/tasks/5
    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorModel))]
    public ActionResult Patch(int id, [FromBody] JsonPatchDocument<TaskV1PatchModel> patchDoc)
    {
      //TODO: Need more sophisticated patching that only updates what has changed #23
      //TODO: Needs proper validation #24
      var db = _service.GetTask(id);

      //Preload with existing DB values
      var model = _mapper.ToPatchModel(db);

      if (model == null) throw Lib.Exceptions.NotFound.Task(id);

      //Apply patch doc to model to overwrite what changed only
      patchDoc.ApplyTo(model);

      //Back to entity so it can be updated
      var entity = _mapper.ToEntity(db!.TaskId, model);

      _service.Edit(entity);

      return NoContent();
    }

    // DELETE api/tasks/5
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult Delete(int id)
    {
      _service.Remove(id);

      return NoContent();
    }
  }
}
