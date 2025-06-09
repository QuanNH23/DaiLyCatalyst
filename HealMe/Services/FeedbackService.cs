using HealMe.DTO;
using HealMe.Models;
using Microsoft.EntityFrameworkCore;

namespace HealMe.Services
{
    public interface IFeedbackService
    {
        Task<IEnumerable<FeedbackResponseDto>> GetAllFeedbacksAsync();
        Task<FeedbackResponseDto?> GetFeedbackByIdAsync(int id);
        Task<FeedbackResponseDto> CreateFeedbackAsync(CreateFeedbackDto dto);
        Task<FeedbackStatsDto> GetFeedbackStatsAsync();
        Task<bool> DeleteFeedbackAsync(int id);
    }
    public class FeedbackService : IFeedbackService
    {
        private readonly HealMeDbContext _context;
        private readonly IFileService _fileService;
        public FeedbackService(HealMeDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        public async Task<IEnumerable<FeedbackResponseDto>> GetAllFeedbacksAsync()
        {
            var feedbacks = await _context.ProductFeedbacks
                .Where(f => f.IsActive)
                .OrderByDescending(f => f.CreatedAt)
                .Select(f => new FeedbackResponseDto
                {
                    Id = f.Id,
                    UserName = f.UserName,
                    Rating = f.Rating,
                    FeedbackText = f.FeedbackText,
                    ImageUrl = f.ImageUrl,
                    CreatedAt = f.CreatedAt
                })
                .ToListAsync();

            return feedbacks;
        }

        public async Task<FeedbackResponseDto?> GetFeedbackByIdAsync(int id)
        {
            var feedback = await _context.ProductFeedbacks
                .Where(f => f.Id == id && f.IsActive)
                .Select(f => new FeedbackResponseDto
                {
                    Id = f.Id,
                    UserName = f.UserName,
                    Rating = f.Rating,
                    FeedbackText = f.FeedbackText,
                    ImageUrl = f.ImageUrl,
                    CreatedAt = f.CreatedAt
                })
                .FirstOrDefaultAsync();

            return feedback;
        }

        public async Task<FeedbackResponseDto> CreateFeedbackAsync(CreateFeedbackDto dto)
        {
            string? imageUrl = null;

            // Upload image if provided
            if (dto.ImageFile != null)
            {
                imageUrl = await _fileService.SaveImageAsync(dto.ImageFile);
            }

            var feedback = new ProductFeedback
            {
                UserName = dto.UserName,
                Rating = dto.Rating,
                FeedbackText = dto.FeedbackText,
                ImageUrl = imageUrl,
                CreatedAt = DateTime.Now,
                IsActive = true // Set IsActive to true for new feedback
                
            };

            _context.ProductFeedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return new FeedbackResponseDto
            {
                Id = feedback.Id,
                UserName = feedback.UserName,
                Rating = feedback.Rating,
                FeedbackText = feedback.FeedbackText,
                ImageUrl = feedback.ImageUrl,
                CreatedAt = feedback.CreatedAt
            };
        }

        public async Task<FeedbackStatsDto> GetFeedbackStatsAsync()
        {
            var feedbacks = await _context.ProductFeedbacks
                .Where(f => f.IsActive)
                .ToListAsync();

            var totalFeedbacks = feedbacks.Count;
            var averageRating = totalFeedbacks > 0 ? feedbacks.Average(f => f.Rating) : 0;
            var recommendCount = feedbacks.Count(f => f.Rating >= 4);
            var recommendPercent = totalFeedbacks > 0 ? (int)Math.Round((double)recommendCount / totalFeedbacks * 100) : 0;

            return new FeedbackStatsDto
            {
                TotalFeedbacks = totalFeedbacks,
                AverageRating = Math.Round(averageRating, 1),
                RecommendPercent = recommendPercent
            };
        }

        public async Task<bool> DeleteFeedbackAsync(int id)
        {
            var feedback = await _context.ProductFeedbacks.FindAsync(id);
            if (feedback == null) return false;

            feedback.IsActive = false; // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }



    }
}
