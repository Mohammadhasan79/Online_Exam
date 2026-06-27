using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using OnlineExam.DTOs.QuestionDTOs;
using OnlineExam.Service;
using OnlineExam.ServiceInterfaces;

namespace OnlineExam.Controllers
{
    [ApiController]
    [Route("api/exams/{examId}/[controller]")]
    [Authorize(Roles = "Prof")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        
        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion(int examId,CreateQuestionDto questionDto)
        {
            var result = await _questionService.CreateQuestionAsync(examId, questionDto);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{questionId}")]
        public async Task<IActionResult> UpdateQuestion(int examId, int questionId, EditQuestionDto questionDto)
        {
            var result = await _questionService.UpdateQuestionAsync(questionId, questionDto);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{questionId}")]
        public async Task<IActionResult> DeleteQuestion(int examId, int questionId)
        {
            var result = await _questionService.DeleteQuestionAsync(questionId);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}
