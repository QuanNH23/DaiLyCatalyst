using HealMe.DTO;
using HealMe.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealMe.Controllers
{
    [ApiController]
    [Route("api/feedbacks")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackResponseDto>>> GetFeedbacks()
        {
            try
            {
                var feedbacks = await _feedbackService.GetAllFeedbacksAsync();
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackResponseDto>> GetFeedback(int id)
        {
            try
            {
                var feedback = await _feedbackService.GetFeedbackByIdAsync(id);
                if (feedback == null)
                {
                    return NotFound(new { message = "Không tìm thấy cảm nhận" });
                }
                return Ok(feedback);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<FeedbackResponseDto>> CreateFeedback([FromForm] CreateFeedbackDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var feedback = await _feedbackService.CreateFeedbackAsync(dto);
                return CreatedAtAction(nameof(GetFeedback), new { id = feedback.Id }, feedback);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tạo cảm nhận", error = ex.Message });
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<FeedbackStatsDto>> GetStats()
        {
            try
            {
                var stats = await _feedbackService.GetFeedbackStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            try
            {
                var result = await _feedbackService.DeleteFeedbackAsync(id);
                if (!result)
                {
                    return NotFound(new { message = "Không tìm thấy cảm nhận" });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }
    }
}
