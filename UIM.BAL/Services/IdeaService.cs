using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UIM.BAL.Services.Interfaces;
using UIM.Common;
using UIM.Common.ResponseMessages;
using UIM.DAL.Repositories.Interfaces;
using UIM.Model.Entities;

namespace UIM.BAL.Services
{
    public class IdeaService : IIdeaService
    {
        private readonly IIdeaRepository _ideaRepository;

        public IdeaService(IIdeaRepository ideaRepository)
        {
            _ideaRepository = ideaRepository;
        }

        public async Task CreateAsync(Idea idea)
        {
            if (idea == null)
                throw new HttpStatusException(HttpStatusCode.BadRequest,
                    ErrorResponseMessages.BadRequest);

            var succeeded = await _ideaRepository.AddAsync(idea);
            if (!succeeded)
                throw new HttpStatusException(HttpStatusCode.InternalServerError,
                    ErrorResponseMessages.UnexpectedError);
        }

        public async Task DeleteAsync(Idea idea)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Idea> GetByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Idea>> ListAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}