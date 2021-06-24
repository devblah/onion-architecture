using System.Threading.Tasks;
using TodoApi.Domain.Models;

namespace TodoApi.Domain.Repositories
{
    // Because this repository is in domain layer, it should not consist of infrasctructure-specific things like "loading" and "saving"
    // It should only caputure business critical evaluations on db's or external services, which you can't skip to remain a valid state
    // The actual implementations of this repo can be quite heavy, because you in contrast to 3-layer architecture you're not following
    // the data flow of your data layer in your service layer, but vice versa.
    public interface ITodoListDomainRepository
    {
         Task<bool> HasUnarchivedListWithTitle(ItemName title);
    }
}