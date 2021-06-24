using System;
namespace TodoApi.Domain.Models
{
    public record Id<T>(Guid value)
    {
        public Id() : this(Guid.NewGuid())
        { }

        public Guid ToGuid() => value;

        public override string ToString() => value.ToString();

        public static Id<T> FromString(string value) => new Id<T>(Guid.Parse(value));
    }
}