using System;
namespace TodoApi.Domain.Models
{
    public record ItemName 
    {
        public string value { get; init; }

        public ItemName(string value) {
            if (value == null) {
                throw new ArgumentNullException("name is null");
            }

            if (value.Length < 3) {
                throw new ArgumentException("name must be at least 3 characters long");
            }

            if (value.Length > 50) {
                throw new ArgumentException("name cannot exceed 50 charcaters");
            }

            this.value = value;
        }

        public override string ToString() => value;
    }
}