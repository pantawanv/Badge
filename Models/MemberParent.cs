using Badge.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Badge.Models
{
    [PrimaryKey(nameof(MemberId), nameof(ParentId))]
    public class MemberParent
    {
        public string MemberId { get; set; }
        [ForeignKey(nameof(MemberId))]
        public Member Member { get; set; }

        public int ParentId { get; set; }
        [ForeignKey(nameof(ParentId))]
        public Parent Parent { get; set; }
    }
}
