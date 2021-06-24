using System;

namespace TodoApi.Domain.Models
{
    public record TodoItem(ItemName name, bool IsCompleted) {
        
    }
}