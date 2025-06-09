using System.ComponentModel.DataAnnotations;

namespace HealMe.DTO
{
    public class CreateFeedbackDto
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        [MaxLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Đánh giá không được để trống")]
        [Range(1, 5, ErrorMessage = "Đánh giá phải từ 1 đến 5 sao")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Nội dung cảm nhận không được để trống")]
        [MaxLength(1000, ErrorMessage = "Nội dung không được vượt quá 1000 ký tự")]
        public string FeedbackText { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
