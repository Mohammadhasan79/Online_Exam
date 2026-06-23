using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using OnlineExam.DTOs.ExamDTOs;
using OnlineExam.Entity;
using OnlineExam.Service;
using OnlineExam.ServiceInterfaces;

namespace OnlineExam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamsController : ControllerBase
    {
        private readonly IExamService _examService;
        public ExamsController(IExamService examService)
        {
            _examService = examService;
        }
        //[HttpGet("{examId}")]
        //public async Task<IActionResult> GetExamById(int examId)
        //{

        //}


        [HttpPost]
        public async Task<IActionResult> CreateExam(CreateExamDto examDto)
        {
            var userId = UserId();
            var result = await _examService.CreateExamAsync(userId!, examDto);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpPut("{examId}")]
        public async Task<IActionResult> CreateExam(int examId, CreateExamDto examDto)
        {
            var userId = UserId();
            var result = await _examService.UpdateExamAsync(examId, userId!, examDto);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteExam(int examId)
        {
            var userId = UserId();
            var result = await _examService.DeleteExamAsync(examId, userId!);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        private string? UserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
