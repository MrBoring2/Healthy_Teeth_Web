using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Patient
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public required string FirstName { get; set; }
        [MaxLength(30)]
        public required string LastName { get; set; }
        [MaxLength(30)]
        public required string MiddleName { get; set; }
        [MaxLength(1)]
        public required string Gender { get; set; }
        public required DateTime DateOfBirth { get; set; }
        [MaxLength(40)]
        public required string City { get; set; }
        [MaxLength(50)]
        public required string District { get; set; }
        [MaxLength(50)]
        public required string Street { get; set; }
        [MaxLength(10)]
        public required string Home { get; set; }
        public required int ApartmentNuber { get; set; }
        [MaxLength(11)]
        public required string Phone { get; set; }
        [MaxLength(4)]
        public required string PassportNumber { get; set; }
        [MaxLength(6)]
        public required string PassportCode { get; set; }
        [MaxLength(16)]
        public required string MedicalPolicy { get; set; }
        public virtual List<Visit>? Visits { get; set; }
    }
}
