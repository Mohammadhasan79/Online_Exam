using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExam.DTOs.AnswerDTOs;
using OnlineExam.Service;
using OnlineExam.ServiceInterfaces;

namespace OnlineExam.Controllers
{
    [ApiController]
    [Route("api/exams/{examId}/[controller]")]
    [Authorize(Roles = "Prof,Student")]
    public class AnswersController : ControllerBase
    {
        private readonly IAnswerService _answerService;
        public AnswersController(IAnswerService answerService)
        {
            _answerService = answerService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> SubmitAnswers(int examId,[FromBody] List<CreateAnswerDto> answerList)
        {
            var userId = UserId();
            var result = await _answerService.AddAnswerListAsync(examId, userId!, answerList);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("[action]")]
        [Authorize(Roles = "Prof")]
        public async Task<IActionResult> GetStudentsCompeletExamList(int examId)
        {
            var userId = UserId();
            var result = await _answerService.GetStudCompeleteExamList(examId, userId!);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("[action]")]
        [Authorize(Roles = "Prof")]
        public async Task<IActionResult> GetStudentAnswer(int examId,string studId)
        {
            var userId = UserId();
            var result = await _answerService.GetStudentAnswerAsync(examId, userId!,studId);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("[action]")]
        [Authorize(Roles = "Prof")]
        public async Task<IActionResult> DeleteStudentAnswer(int examId, string studId)
        {
            var userId = UserId();
            var result = await _answerService.DeleteStudentAnswerAsync(examId, userId!, studId);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        private string? UserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
