namespace Smart_ELearning.Models
{
    public class StudentInClassModel
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public ClassModel ClassModel { get; set; }
        public string UserId { get; set; }
        public AppUserModel AppUserModel { get; set; }
    }
}