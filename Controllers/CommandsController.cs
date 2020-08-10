using System.Collections.Generic;
using AutoMapper;
using Commander.Data;
using Commander.Dtos;
using Commander.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace Commander.Controllers
{
    [Route("api/commands")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommanderRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommanderRepo repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        //GET api/commands
        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var value = _repository.GetAllCommands();
            return base.Ok(_mapper.Map<IEnumerable<CommandReadDto>>(value));
        }

        //GET api/commands/{id}
        [HttpGet("{id}", Name = nameof(GetCommandById))]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var commandItem = _repository.GetCommandById(id);
            if (commandItem == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CommandReadDto>(commandItem));
        }

        //POST api/commands
        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto command)
        {
            var commandToCreate = _mapper.Map<Command>(command);
            _repository.CreateCommand(commandToCreate);
            _repository.SaveChanges();
            var commandReadDto = _mapper.Map<CommandReadDto>(commandToCreate);
            var result = CreatedAtRoute(nameof(GetCommandById), new { Id = commandReadDto.Id }, commandReadDto);
            return result;
        }

        //PUT api/commands/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
        {
            var commandToUpdate = _repository.GetCommandById(id);
            if (commandToUpdate == null)
            {
                return NotFound();
            }

            _mapper.Map<CommandUpdateDto, Command>(commandUpdateDto, commandToUpdate);
            _repository.UpdateCommand(commandToUpdate);
            _repository.SaveChanges();
            return NoContent();
        }

        //PATCH api/commands/{id}
        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);

            patchDoc.ApplyTo(commandToPatch, ModelState);
            if (!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map<CommandUpdateDto, Command>(commandToPatch, commandModelFromRepo);

            _repository.UpdateCommand(commandModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCommandById(int id)
        {
            var command = _repository.GetCommandById(id);
            if (command == null)
            {
                return NotFound();
            }
            _repository.DeleteCommand(command);
            _repository.SaveChanges();
            return NoContent();
        }

    }
}