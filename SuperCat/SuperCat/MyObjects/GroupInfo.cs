using System.ComponentModel.DataAnnotations;
using System.DirectoryServices;

namespace SuperCat.MyObjects
{
    public class GroupInfo
    {
        [Key]
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string GroupJoin { get; set; } = string.Empty;
        public string GroupMembers { get; set; } = string.Empty;
        public int MaxPeople { get; set; }
        public byte[]? Icon { get; set; } = null!;
        public string Messages { get; set; } = string.Empty;

        public GroupInfo()
        {
            
        }
        public GroupInfo(int owner, string name, int maxPlace, byte[] icon)
        {
            this.OwnerId = owner;
            this.Name = name;
            this.MaxPeople = maxPlace;
            this.Icon = icon;

            GroupMembers = ".1." + owner;

        }
    }
}
