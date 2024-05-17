using System.ComponentModel.DataAnnotations;

namespace SuperCat.MyObjects
{
    public class GroupInfo
    {
        [Key]
        public int Id { get; set; }
        public List<int> GroupMembers { get; set; } = null!;
        public List<int> Chat { get; set; } = null!;
    }
}
