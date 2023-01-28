using System.ComponentModel.DataAnnotations;

namespace PeopleIKnow.Models
{
    public enum GiftType
    {
        [Display(Name = "received")]
        Received,
        [Display(Name = "given")]
        Given,
        [Display(Name = "idea")]
        Idea
    }
}