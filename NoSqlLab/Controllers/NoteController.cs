using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoSqlLab.Models.Persistance;
using NoSqlLab.Services.Repositories;
using NoSqlLab.ViewModels;

namespace NoSqlLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly NoteRepository _noteRepository;
        public NoteController(NoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var dbNotes = _noteRepository.GetAll();
            return Ok(dbNotes);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
      
          var dbNote = _noteRepository.GetById(id);
          return Ok(dbNote);
        }

        [HttpGet("userId/{userId}")]
        public IActionResult GetByUserId(Guid userId)
        {
            var dbNotes = _noteRepository.GetByUserId(userId);
            return Ok(dbNotes);
        }

        [HttpGet("search")]
        public IActionResult GetSearchResult(string searchString)
        {
            var dbNotes = _noteRepository.NoteSearch(searchString);
            return Ok(dbNotes);
        }

        [HttpPut]
        public IActionResult Update(Note note)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            note.UpdateTime = DateTime.Now;
            note.UserId = userId;
            _noteRepository.UpdateNote(note);

            return NoContent();
        }

        [Authorize] // атрибут для авторизации
        [HttpPost]
        public IActionResult Post([FromBody] NoteApiModel noteModel)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var dbNote = _noteRepository.Insert(new Note
            {
                Text = noteModel.Text,
                Title = noteModel.Title,
                UserId = userId
            });
            return Ok(dbNote);
        }
    }
}