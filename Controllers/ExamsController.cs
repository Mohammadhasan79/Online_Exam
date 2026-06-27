using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExam.DTOs.ExamDTOs;
using OnlineExam.Entity;
using OnlineExam.Service;
using OnlineExam.ServiceInterfaces;

namespace OnlineExam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Prof,Student")]
    public class ExamsController : ControllerBase
    {
        private readonly IExamService _examService;
        public ExamsController(IExamService examService)
        {
            _examService = examService;
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Prof")]
        public async Task<IActionResult> GetUserAndExamList()
        {
            var userId = UserId();
            var result = await _examService.GetUserExamListAsync(userId!);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetStudentAssignList()
        {
            var userId = UserId();
            var userRole = UserRole();
            var result = await _examService.GetStudentAssignListAsync(userId!, userRole!);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("[action]")]
        [Authorize(Roles ="Prof")]
        public async Task<IActionResult> AddStudentAssign(string studentId, int examId)
        {
            var userId = UserId();
            var result = await _examService.AddExamToStudentAsync(userId!, studentId, examId);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("[action]/{studentAssignId}")]
        [Authorize(Roles = "Prof")]
        public async Task<IActionResult> DeleteStudentAssign(int studentAssignId)
        {
            var userId = UserId();
            var result = await _examService.DeleteStudentAssignAsync(userId!, studentAssignId);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("[action]/{examId}")]
        public async Task<IActionResult> GetExamById(int examId)
        {
            var userId = UserId();
            var userRole = UserRole();
            var result = await _examService.GetExamAndQuestionByIdAsync(examId, userId!, userRole!);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("[action]")]
        [Authorize(Roles = "Prof")]
        public async Task<IActionResult> GetExamByUserId()
        {
            var userId = UserId();
            var result = await _examService.GetExamAndQuestionByUserIdAsync(userId!);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Prof")]
        public async Task<IActionResult> CreateExam(CreateExamDto examDto)
        {
            var userId = UserId();
            var result = await _examService.CreateExamAsync(userId!, examDto);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpPut("[action]/{examId}")]
        [Authorize(Roles = "Prof")]
        public async Task<IActionResult> CreateExam(int examId, CreateExamDto examDto)
        {
            var userId = UserId();
            var result = await _examService.UpdateExamAsync(examId, userId!, examDto);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("[action]")]
        [Authorize(Roles = "Prof")]
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
        private string? UserRole()
        {
            return User.FindFirstValue(ClaimTypes.Role);
        }
    }
}
