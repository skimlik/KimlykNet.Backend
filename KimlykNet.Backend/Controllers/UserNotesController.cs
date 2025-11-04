using KimlykNet.Backend.Models;
using KimlykNet.Contracts.Auth;
using KimlykNet.Data.Abstractions;
using KimlykNet.Data.Abstractions.Entities;
using KimlykNet.Data.Abstractions.Repositories;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace KimlykNet.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors]
public class UserNotesController(
    IIdEncoder idEncoder,
    IUserNotesRepository notesRepository,
    IUserContextAccessor contextAccessor) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateAsync([FromBody] CreateUserNoteModel note, CancellationToken cancellationToken)
    {
        var user = contextAccessor.GetUserInfo()?.Email;
        var created = await notesRepository.CreateAsync(user, note.Title, note.Text, note.IsPublic, cancellationToken);
        return CreatedAtRoute("UserNotesController_GetUserNoteAsync", new { noteId = idEncoder.Encode(created.Id.ToString()) }, created);
    }

    [HttpGet("{noteId}", Name = "UserNotesController_GetUserNoteAsync")]
    public async Task<IActionResult> GetUserNoteAsync([FromRoute] string noteId, CancellationToken cancellationToken)
    {
        var id = int.TryParse(idEncoder.SafeDecode(noteId), out var val) ? val : 0;
        if (id == 0)
        {
            return BadRequest("Invalid id");
        }

        var data = await GetEntityAsync(id, cancellationToken);
        return data is null ? NotFound() : Ok(data);
    }

    [HttpPut("{noteId}/share")]
    public async Task<IActionResult> ShareUserNoteAsync([FromRoute] string noteId, CancellationToken cancellationToken)
    {
        var id = int.TryParse(idEncoder.SafeDecode(noteId), out var val) ? val : 0;
        if (id == 0)
        {
            return BadRequest("Invalid id");
        }

        var data = await GetEntityAsync(id, cancellationToken);
        if (data is null)
        {
            return NotFound();
        }

        if (data.User != contextAccessor.GetUserInfo()?.Email)
        {
            return Forbid();
        }

        var shared = await notesRepository.ShareAsync(id, true, cancellationToken);
        return Ok(shared);
    }

    private async Task<UserNote> GetEntityAsync(int id, CancellationToken cancellationToken)
    {
        var data = await notesRepository.GetAsync(id, cancellationToken);
        if (contextAccessor.IsAuthenticated)
        {
            if (data is null || data.User != contextAccessor.GetUserInfo()?.Email)
            {
                return null;
            }

            return data;
        }

        return data?.IsPublic ?? false ? data : null;
    }
}